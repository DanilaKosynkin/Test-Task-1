using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryNeedProducts : Factory
{
    [SerializeField] private InputStorage[] _inputStorage;
    [SerializeField] private Product[] _needProducts;

    private bool[] _lockProducts;

    private void Start()
    {
        _lockProducts = new bool[_needProducts.Length];
        for (int i = 0; i < _inputStorage.Length; i++)
        {
            _inputStorage[i] = _inputStorage[i].GetComponent<InputStorage>();
        }

        StartCoroutine(WaitProductMove());
    }

    private IEnumerator WaitProductMove()
    {
        while (true)
        {
            yield return new WaitUntil(() => CheckAvailabilityProducts());
            Product[] products = new Product[_needProducts.Length];
            for (int i = 0; i < products.Length; i++)
            {
                products[i] = _inputStorage[i].GetLastProductStorage();
                products[i].StartMovementCoroutines(SpawnPointProducts);
            }
            yield return new WaitUntil(() => products[products.Length - 1].transform.position == SpawnPointProducts.transform.position);
            foreach (Product product in products)
            {
                product.Destoy();
            }
            CreateProdcut();
            yield return new WaitForSeconds(TimeToCreate);
        }
    }

    public Product GetFixedProduct(Product fixedProduct, PlayerStorage playerStorage)
    {
        for (int i = 0; i < _needProducts.Length; i++)
        {
            if (!_lockProducts[i] && playerStorage.CheckNeedProductPlayerStorage(_needProducts[i]))
            {
                if (fixedProduct != null)
                {
                    for (int j = 0; j < _lockProducts.Length; j++)
                    {
                        if (_needProducts[j].CompareTag(fixedProduct.tag))
                        {
                            _lockProducts[j] = false;
                            break;
                        } 
                    }
                }
                _lockProducts[i] = true;
                return _needProducts[i];
            }
        }

        if (fixedProduct != null)
        {
            for (int i = 0; i < _lockProducts.Length; i++)
            {
                if (_needProducts[i].CompareTag(fixedProduct.tag))
                {
                    _lockProducts[i] = false;
                    break;
                }
            }
        }
        return null;
    }

    private bool CheckAvailabilityProducts()
    {
        foreach (InputStorage inputStorage in _inputStorage)
        {
            if (inputStorage.CheckAvailabilityProductStorage() == false)
                return false;
        }
        return true;
    }
}
