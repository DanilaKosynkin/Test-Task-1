using System.Collections;
using TMPro;
using UnityEngine;

public class FactoryNoNeedProductInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text _factoryInfoTexst;
    [SerializeField] private FactoryNoNeedProduct _factory;

    private void Start()
    {
        StartCoroutine(FactoryInfoView());
    }

    private IEnumerator FactoryInfoView()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _factoryInfoTexst.text = _factory.FactoryOutputInfoCheck();
        }
    }
}
