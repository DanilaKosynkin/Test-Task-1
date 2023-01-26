using System.Collections;
using UnityEngine;

public class FactoryNoNeedProduct : Factory
{
    private void Start()
    {
        StartCoroutine(CreateProductCoroutine());
    }

    private IEnumerator CreateProductCoroutine()
    {
        while(true)
        {
            yield return new WaitUntil(() => OutputStorage.CheckFreeSpaceStorage());
            CreateProdcut();
            yield return new WaitForSeconds(TimeToCreate);
        }
    }
}
