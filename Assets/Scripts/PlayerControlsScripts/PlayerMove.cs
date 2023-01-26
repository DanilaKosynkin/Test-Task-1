using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private float _playerMovementSpeed = 10f;

    private Rigidbody _playerRigidbody;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _joystick = _joystick.GetComponent<FixedJoystick>();
    }

    private void FixedUpdate()
    {
        _playerRigidbody.velocity = new Vector3(_joystick.Horizontal * _playerMovementSpeed,
            _playerRigidbody.velocity.y, _joystick.Vertical * _playerMovementSpeed);

        _playerRigidbody.rotation = Quaternion.LookRotation(_playerRigidbody.velocity);
    }
}
