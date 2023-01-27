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
            yield return new WaitUntil(() => CheckAvailabilityProducts() && OutputStorage.CheckFreeSpaceStorage());
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

    private bool CheckAvailabilityProducts()
    {
        foreach (InputStorage inputStorage in _inputStorage)
        {
            if (inputStorage.CheckAvailabilityProductStorage() == false)
                return false;
        }
        return true;
    }
    private void CheckFixedProduct(Product fixedProduct)
    {
        if (fixedProduct != null)
            for (int i = 0; i < _lockProducts.Length; i++)
            {
                if (_needProducts[i].CompareTag(fixedProduct.tag))
                {
                    _lockProducts[i] = false;
                    return;
                }
            }
    }

    public Product GetFixedProduct(Product fixedProduct, PlayerStorage playerStorage)
    {
        for (int i = 0; i < _needProducts.Length; i++)
        {
            if (!_lockProducts[i] && playerStorage.CheckNeedProductPlayerStorage(_needProducts[i]))
            {
                CheckFixedProduct(fixedProduct);
                _lockProducts[i] = true;
                return _needProducts[i];
            }
        }
        CheckFixedProduct(fixedProduct);
        return null;
    }

    public string FactoryInputInfoCheck()
    {
        if (!OutputStorage.CheckFreeSpaceStorage())
            return gameObject.name + " - " + "Output Storage Full";
        if (!CheckAvailabilityProducts())
            return gameObject.name + " - " + "Input Storage waiting for the right products";
        return "";
    }
}

