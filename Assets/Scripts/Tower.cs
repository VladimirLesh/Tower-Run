using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Vector2Int _humanInTownRange;
    [SerializeField] private Human[] _humans;

    private List<Human> _humansInTower;

    private void Awake()
    {
        _humansInTower = new List<Human>();
        int humanInTownCount = Random.Range(_humanInTownRange.x, _humanInTownRange.y);
        SpownHumans(humanInTownCount);
    }

    private void SpownHumans(int human)
    {
        Vector3 spownPoint = transform.position;

        for (int i = 0; i < human; i++)
        {
            Human spownedHuman = _humans[Random.Range(0, _humans.Length)];
            
            _humansInTower.Add(Instantiate(spownedHuman,spownPoint, Quaternion.Euler(0,90,0), transform));
            _humansInTower[i].transform.localPosition = new Vector3(0, _humansInTower[i].transform.localPosition.y, 0);
            spownPoint = _humansInTower[i].FixationPoint.position;
        }
    }

    public List<Human> CollectHuman(Transform distanceCheker, float fixationMaxDistance)
    {
        for (int i = 0; i < _humansInTower.Count; i++)
        {
            float distanceBetweenPoint = CheckDistanceY(distanceCheker, _humansInTower[i].FixationPoint.transform);

            if (distanceBetweenPoint < fixationMaxDistance)
            {
                List<Human> collectedHumans = _humansInTower.GetRange(0, i + 1);
                _humansInTower.RemoveRange(0, i + 1);
                return collectedHumans;
            }
        }
        return null;
    }

    private float CheckDistanceY(Transform distanceChecker, Transform humanFixationPoint)
    {
        Vector3 distanceCheckerY = new Vector3(0, distanceChecker.position.y, 0);
        Vector3 humanFixationPointY = new Vector3(0, humanFixationPoint.position.y, 0);
        return Vector3.Distance(distanceCheckerY, humanFixationPointY);
    }

    public void Break()
    {
        for (int i = 0; i < _humansInTower.Count; i++)
        {
            _humansInTower[i].OnBounce();
        }
    }
}
