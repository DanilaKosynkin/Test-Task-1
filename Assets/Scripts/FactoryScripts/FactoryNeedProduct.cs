using System.Collections;
using UnityEngine;

public class FactoryNeedProduct : MonoBehaviour
{
    [SerializeField] private float _timeToCreate = 0.5f;
    [SerializeField] private Product _createProductPrefab;
    [SerializeField] private Product[] _needProducts;
    [SerializeField] private OutputStorage _outputStorage;
    [SerializeField] private InputStorage[] _inputStorage;
    [SerializeField] private Transform _spawnPointProducts;

    private bool[] _lockProducts;

    private void Start()
    {
        _spawnPointProducts = _spawnPointProducts.GetComponent<Transform>();
        _outputStorage = _outputStorage.GetComponent<OutputStorage>();
        _lockProducts = new bool[_needProducts.Length];
        for (int i = 0; i < _inputStorage.Length; i++)
        {
            _inputStorage[i] = _inputStorage[i].GetComponent<InputStorage>();
            _inputStorage[i].Init(_needProducts, ref _lockProducts);
        }

        StartCoroutine(WaitProductMove());
    }

    private void CreateProdcut()
    {
        Product spawnProduct = Instantiate(_createProductPrefab, _spawnPointProducts.position, Quaternion.identity);
        spawnProduct.GetComponent<BoxCollider>().enabled = false;
        _outputStorage.AddProductStorage(spawnProduct);
    }

    private IEnumerator WaitProductMove()
    {
        while (true)
        {
            yield return new WaitUntil(()=>  CheckAvailabilityProducts());

            Product[] products = new Product[_needProducts.Length];
            for (int i = 0; i < products.Length; i++)
            {
                products[i] = _inputStorage[i].GetLastProductStorage();
                products[i].StartMovementCoroutines(_spawnPointProducts);
            }
            yield return new WaitUntil(() => products[products.Length - 1].transform.position == _spawnPointProducts.transform.position);
            foreach (Product product in products)
            {
                product.Destoy();
            }
            CreateProdcut();
            yield return new WaitForSeconds(_timeToCreate);
        }
    }

    private bool CheckAvailabilityProducts()
    {
        foreach (InputStorage inputStorage in _inputStorage)
        {
            if(inputStorage.CheckAvailabilityProductStorage() == false)
                return false;
        }
        return true;
    }
}
