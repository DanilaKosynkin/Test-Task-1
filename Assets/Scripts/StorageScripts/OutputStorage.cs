using System.Collections;
using UnityEngine;

public class OutputStorage : Storage
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStorage playerStorage))
            StartCoroutine(OutputProducts(playerStorage));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerStorage>())
            StopAllCoroutines();
    }

    private IEnumerator OutputProducts(PlayerStorage playerStorage)
    {
        while(true)
        {
            yield return new WaitUntil(() => CheckAvailabilityProductStorage() && playerStorage.CheckFreeSpaceStorage());
            playerStorage.AddProductStorage(GetLastProductStorage());
            yield return new WaitForSeconds(TimeToTransitProduct);
        }
    }
}
