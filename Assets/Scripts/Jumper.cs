using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Jumper : MonoBehaviour
{
    public event UnityAction HumanJump;
    [SerializeField] private float _jumpForse;
    private Rigidbody _rigidbody;
    private bool _isGround;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isGround == true)
        {
            _isGround = false;
            _rigidbody.AddForce(Vector3.up * _jumpForse);
            HumanJump?.Invoke();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Road road))
        {
            _isGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Booster booster))
        {
            _jumpForse *= Random.Range(1.1f, 1.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Booster booster))
        {
            _jumpForse = 30;
        }
    }
}
