using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    [SerializeField] private Product _createProductPrefab;
    [SerializeField] protected OutputStorage OutputStorage;
    [SerializeField] protected Transform SpawnPointProducts;
    [SerializeField] protected float TimeToCreate = 0.5f;

    private void Awake()
    {
        SpawnPointProducts = SpawnPointProducts.GetComponent<Transform>();
        OutputStorage = OutputStorage.GetComponent<OutputStorage>();
    }

    protected void CreateProdcut()
    {
        Product spawnProduct = Instantiate(_createProductPrefab, SpawnPointProducts.position, Quaternion.identity);
        spawnProduct.GetComponent<BoxCollider>().enabled = false;
        OutputStorage.AddProductStorage(spawnProduct);
    }
}
