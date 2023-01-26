using System.Collections;
using UnityEngine;

public class Product : MonoBehaviour
{
    private IEnumerator MovementCoroutines(Transform targetPoint)
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, targetPoint.position) <= 0.5f)
            {
                transform.SetPositionAndRotation(targetPoint.position, targetPoint.rotation);
                transform.parent = targetPoint.parent;
                StopAllCoroutines();
                yield return null;
            }
            yield return new WaitUntil(() => transform.position != targetPoint.position);
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetPoint.position,  Time.fixedDeltaTime * 2),
                Quaternion.Lerp(transform.rotation, targetPoint.localRotation, Time.fixedDeltaTime * 2));
        }
    }

    public void StartMovementCoroutines(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(MovementCoroutines(target));
    }

    public void Destoy()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
