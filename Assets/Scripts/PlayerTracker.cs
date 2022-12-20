using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private PlayerTower _playerTower;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector3 _offsetPosition;
    [SerializeField] private Vector3 _offsetRotation;

    private Vector3 _targetOffsetPos;

    private void OnEnable()
    {
        _playerTower.HumanAdded += OnHumanAdded;
    }

    private void OnDisable()
    {
        _playerTower.HumanAdded -= OnHumanAdded;

    }

    private void Update()
    {
        UpdatePosition();
        _offsetPosition = Vector3.MoveTowards(_offsetPosition, _targetOffsetPos, _moveSpeed * Time.deltaTime);
    }

    private void UpdatePosition()
    {
        transform.position = _playerTower.transform.position;
        transform.localPosition += _offsetPosition;

        Vector3 lookAtPoint = _playerTower.transform.position + _offsetRotation;
        
        transform.LookAt(lookAtPoint);
    }

    private void OnHumanAdded(int count)
    {
        _targetOffsetPos = _offsetPosition + (Vector3.up + Vector3.back) * count / 5;
        UpdatePosition();
    }
}
