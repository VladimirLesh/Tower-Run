using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Human : MonoBehaviour
{
    [SerializeField] private float explosionForse;
    private float explosionRadius = 100;
    private Animator _animator;
    private Jumper _jumper;
    private Rigidbody _rigidBody;

    [SerializeField] private Transform _fixationPoint;

    public Transform FixationPoint => _fixationPoint;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _jumper = FindObjectOfType<Jumper>();
    }

    public void OnBounce()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            _rigidBody = gameObject.AddComponent<Rigidbody>();
        }
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        _rigidBody.isKinematic = false;
        _rigidBody.AddExplosionForce(explosionForse, new Vector3(Random.Range(-3, 3),Random.Range(-3, 3), Random.Range(-3, 3)),
            explosionRadius, 3, ForceMode.Impulse);
        _animator.SetTrigger("Fall");
        StartCoroutine(DestroyHuman());
    }

    IEnumerator DestroyHuman()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void Run()
    {
        _animator.SetBool("Run", true);
        _animator.SetBool("Idle", false);
    } 
    public void StopRun()
    {
        _animator.SetBool("Run", false);
        _animator.SetBool("Idle", true);
    }

    public void Jump()
    {
        _animator.SetTrigger("Jump");
    }
}
