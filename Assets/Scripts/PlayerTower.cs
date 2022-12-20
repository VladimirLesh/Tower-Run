using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTower : MonoBehaviour
{
    public event UnityAction<int> HumanAdded;

    public static PlayerTower instance;

    [SerializeField] private Human _startHuman; 
    [SerializeField] private Transform _distanceChecker;
    [SerializeField] private float _fixationMaxDistance;
    [SerializeField] private BoxCollider _checkCollider;

    public List<Human> _humans;
    public Jumper _jumper;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        
        _jumper = GetComponent<Jumper>();
        _humans = new List<Human>();
        Vector3 spownPoint = transform.position;
        _humans.Add(Instantiate(_startHuman, spownPoint, Quaternion.identity, transform));
        _humans[0].Run();
        HumanAdded?.Invoke(_humans.Count);
        
        _jumper.HumanJump += OnHumanJump;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Human human))
        {
            Tower collisionTower = human.gameObject.GetComponentInParent<Tower>();
            if (collisionTower != null)
            {
                List<Human> collectedHumans = collisionTower.CollectHuman(_distanceChecker, _fixationMaxDistance);

                if (collectedHumans != null)
                {
                    _humans[0].StopRun();
                    for (int i = collectedHumans.Count - 1; i >= 0; i--)
                    {
                        Human insertHuman = collectedHumans[i];

                        InsertHuman(insertHuman);
                        DisplaceChackers(insertHuman, true);
                    }
                    HumanAdded?.Invoke(_humans.Count);
                    _humans[0].Run();
                    collisionTower.Break();
                }
                else
                {
                    collisionTower.Break();
                }
            }            
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ObstaclePack obstaclePack))
        {
            int count = obstaclePack.Int;
            _humans[0].StopRun();

            // Метод убирает количество Human, равное количеству цилиндров.
            if (count == 1)
            {
                DisplaceChackers(_humans[0], false);
                _humans[0].OnBounce();
                _humans.Remove(_humans[0]);
            }
            else if (count >= _humans.Count)
            {
                for (int i = 0; i < _humans.Count; i++)
                {
                    DisplaceChackers(_humans[0], false);
                    RemoveHuman(i);
                }
            }
            else if (count < _humans.Count)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    DisplaceChackers(_humans[0], false);
                    RemoveHuman(i);
                }
            }

            if (_humans.Count >= 1) _humans[0].Run();
        }
    }

    private void RemoveHuman(int i)
    {
        _humans[i].OnBounce();
        _humans.Remove(_humans[i]);
        HumanAdded?.Invoke(_humans.Count);
    }
    
    private void InsertHuman(Human collectedHumans)
    {
        _humans.Insert(0, collectedHumans);
        SetHumanPosition(collectedHumans);
    }

    private void SetHumanPosition(Human human)
    {
        human.transform.parent = transform;
        human.transform.localPosition = new Vector3(0, human.transform.localPosition.y, 0);
        human.transform.localRotation = Quaternion.identity;
    }

    private void DisplaceChackers (Human human, bool insert)
    {
        float displaceScale = 2.8f;
        Vector3 distanceCheckerNewPosition = _distanceChecker.position;
        if (insert)
        {
            distanceCheckerNewPosition.y -= human.transform.localScale.y * displaceScale;
        }
        else
        {
            distanceCheckerNewPosition.y += human.transform.localScale.y * displaceScale;
        }
        _distanceChecker.position = distanceCheckerNewPosition;
        _checkCollider.center = _distanceChecker.localPosition;
    }

    private void OnHumanJump()
    {
        _humans[0].Jump();
    }    
}
