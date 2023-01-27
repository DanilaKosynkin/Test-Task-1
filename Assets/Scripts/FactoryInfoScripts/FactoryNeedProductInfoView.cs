using System.Collections;
using TMPro;
using UnityEngine;

public class FactoryNeedProductInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text _factoryInfoTexst;
    [SerializeField] private FactoryNeedProducts _factory;

    private void Start()
    {
        StartCoroutine(FactoryInfoView());
    }

    private IEnumerator FactoryInfoView()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _factoryInfoTexst.text = _factory.FactoryInputInfoCheck();
        }
    }
}
