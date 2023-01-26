using System.Collections;
using UnityEngine;

public class InputStorage : Storage
{
    private FactoryNeedProduct _factory;
    private Product[] _needProducts;
    private bool[] _lockProducts;
    private Product _fixedProduct;
    private int _previousFixedproduct;

    private void Start()
    {
        _factory = transform.parent.GetComponent<FactoryNeedProduct>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStorage playerStorage))
            StartCoroutine(InputProducts(playerStorage));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerStorage>())
            StopAllCoroutines();
    }

    private IEnumerator InputProducts(PlayerStorage playerStorage)
    {
        while (true)
        {
            yield return new WaitUntil(() => CheckFreeSpaceStorage() && playerStorage.CheckAvailabilityProductStorage());
            Product product = CheckNeedProduct(playerStorage);
            if (product == null)
                break;
            AddProductStorage(product);
            yield return new WaitForSeconds(TimeToTransitProduct);
        }
        
    }

    private Product CheckNeedProduct(PlayerStorage playerStorage)
    {
        if (_fixedProduct == null)
        {
            for (int i = 0; i < _needProducts.Length; i++)
            {
                if(!_lockProducts[i] && playerStorage.CheckNeedProductPlayerStorage(_needProducts[i]))
                {
                    _previousFixedproduct = i;
                    _lockProducts[i] = true;
                    _fixedProduct = _needProducts[i];
                    return playerStorage.GetNeedProduct(_needProducts[i]);
                }
            }
            return null;
        }
        else
        {
            if (playerStorage.CheckNeedProductPlayerStorage(_fixedProduct))
                return playerStorage.GetNeedProduct(_fixedProduct);
            if(!CheckAvailabilityProductStorage())
            {
                for (int i = 0; i < _needProducts.Length; i++)
                {
                    if (!_lockProducts[i] && playerStorage.CheckNeedProductPlayerStorage(_needProducts[i]))
                    {
                        _lockProducts[_previousFixedproduct] = false;
                        _lockProducts[i] = true;
                        _previousFixedproduct = i;
                        _fixedProduct = _needProducts[i];
                        return playerStorage.GetNeedProduct(_needProducts[i]);
                    }
                }
            }
            return null;
        }
    }

    public void Init(Product[] needProducts,ref bool[] lockProducts)
    {
        _needProducts = needProducts;
         _lockProducts = lockProducts;
    }

}
