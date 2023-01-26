using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputStorage : Storage
{
    private FactoryNeedProducts _factory;
    private Product _fixedProduct;

    private void Start()
    {
        _factory = transform.parent.GetComponent<FactoryNeedProducts>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStorage playerStorage))
        {
            if (_fixedProduct == null)
            {
                _fixedProduct = _factory.GetFixedProduct(_fixedProduct, playerStorage);
                if (_fixedProduct == null)
                    return;
            }
            StartCoroutine(InputProducts(playerStorage));
        }
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
        Product product = playerStorage.GetNeedProduct(_fixedProduct);
        if (product != null)
             return product;
        if (!CheckAvailabilityProductStorage())
        {
            _fixedProduct = _factory.GetFixedProduct(_fixedProduct, playerStorage);
            return playerStorage.GetNeedProduct(_fixedProduct);
        }
        return null;
    }
}
