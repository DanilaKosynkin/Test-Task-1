public class PlayerStorage : Storage
{
    private void ProductOffset(int startPoint, int lastPoint)
    {
        for (int i = startPoint; i < lastPoint - 1; i++)
        {
            ProductStorageList[i + 1].StartMovementCoroutines(SpawnPointArray[i]);
            ProductStorageList[i] = ProductStorageList[i + 1];
        }
    }

    public Product GetNeedProduct(Product needProducts)
    {
        for (int i = ProductStorageList.Count - 1; i >= 0; i--)
        {
            if (needProducts.CompareTag(ProductStorageList[i].tag))
            {
                Product product = ProductStorageList[i];
                product.transform.parent = null;
                ProductOffset(i, ProductStorageList.Count);
                ProductStorageList.RemoveAt(ProductStorageList.Count - 1);
                return product;
            }
        }
        return null;
    }

    public bool CheckNeedProductPlayerStorage(Product needProducts)
    {
        for (int i = ProductStorageList.Count - 1; i >= 0; i--)
        {
            if (needProducts.CompareTag(ProductStorageList[i].tag))
                return true;
        }
        return false;
    }

    


}
