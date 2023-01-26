using System.Collections;
using UnityEngine;

public class FactoryNoNeedProduct : MonoBehaviour
{
    [SerializeField] private float _timeToCreate = 0.5f;
    [SerializeField] private Product _createProductPrefab;
    [SerializeField] private OutputStorage _outputStorage;
    [SerializeField] private Transform _spawnPointProducts;

    private void Start()
    {
        _spawnPointProducts = _spawnPointProducts.GetComponent<Transform>();
        _outputStorage = _outputStorage.GetComponent<OutputStorage>();
        StartCoroutine(CreateProductCoroutine());
    }

    private IEnumerator CreateProductCoroutine()
    {
        while(true)
        {
            yield return new WaitUntil(() => _outputStorage.CheckFreeSpaceStorage());
            Product spawnProduct = Instantiate(_createProductPrefab, _spawnPointProducts.position, Quaternion.identity);
            spawnProduct.GetComponent<BoxCollider>().enabled = false;
            _outputStorage.AddProductStorage(spawnProduct);
            yield return new WaitForSeconds(_timeToCreate);
        }
    }
}
