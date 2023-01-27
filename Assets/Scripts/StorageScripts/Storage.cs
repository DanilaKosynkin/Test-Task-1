using System.Collections.Generic;
using UnityEngine;

public abstract class Storage : MonoBehaviour
{
    [SerializeField] private int _lengthStorage = 10;
    [SerializeField] private int _widthStorage = 2;
    [SerializeField] private Vector3 _productLengthOffset = new(0,0,0.5f);
    [SerializeField] private Vector3 _productWithsOffset = new(1, 0, 0);
    [SerializeField] private Transform _spawmPoint;
    [SerializeField] protected float TimeToTransitProduct = 1;

    protected Transform[] SpawnPointArray;
    protected List<Product> ProductStorageList;

    private void Awake()
    {
        if (_lengthStorage % _widthStorage == 1)
            _lengthStorage++;
        ProductStorageList = new List<Product>(_lengthStorage * _widthStorage);
        SpawnPointArray = GenerateSpawnPoint(_widthStorage, _lengthStorage, _productLengthOffset, _productWithsOffset);
    }

    private Transform[] GenerateSpawnPoint(int widthStorage, int lengthStorage, Vector3 productLengthOffset, Vector3 productWithsOffset)
    {
        Transform[] spawnPointArray = new Transform[widthStorage * lengthStorage];
        for (int i = 0; i < widthStorage; i++)
        {
            if (i == 0)
                spawnPointArray[0] = _spawmPoint;
            else spawnPointArray[i * lengthStorage] = Instantiate(_spawmPoint.gameObject, _spawmPoint.position + (productWithsOffset * i),
                Quaternion.identity, transform).transform;

            for (int j = 1; j < lengthStorage; j++)
            {
                spawnPointArray[j + (i * lengthStorage)] = Instantiate(_spawmPoint.gameObject, spawnPointArray[i * lengthStorage].position + (productLengthOffset * j),
                    Quaternion.identity, transform).transform;
            }
        }
        return spawnPointArray;
    }

    public void AddProductStorage(Product product)
    {
        product.StartMovementCoroutines(SpawnPointArray[ProductStorageList.Count]);
        ProductStorageList.Add(product);
    }

    public Product GetLastProductStorage()
    {
        Product product = ProductStorageList[ProductStorageList.Count - 1];
        product.transform.parent = null;
        ProductStorageList.RemoveAt(ProductStorageList.Count - 1);
        return product;
    }

    public bool CheckFreeSpaceStorage()
    {
        if (ProductStorageList.Count < _lengthStorage * _widthStorage)
            return true;
        return false;
    }

    public bool CheckAvailabilityProductStorage()
    {
        if (ProductStorageList.Count > 0)
            return true;
        return false;
    }
}
