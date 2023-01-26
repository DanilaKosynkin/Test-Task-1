using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _cameraPoinTransform;

    private void Start()
    {
        _playerTransform = _playerTransform.GetComponent<Transform>();
        _cameraPoinTransform = _cameraPoinTransform.GetComponent<Transform>();
    }
    private void Update()
    {
        _cameraPoinTransform.position = _playerTransform.position;
    }
}
