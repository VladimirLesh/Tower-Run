using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Tower _tower;
    [SerializeField] private int _humanTowerCount;
    [SerializeField] private Booster _booster;
    [SerializeField] private Obstacle _obstacle;
    [SerializeField] private ObstacleCreator _obstacleCreator;
    [SerializeField] private Transform _startSpownPlane;
    [SerializeField] private GameObject _plane;
    [SerializeField] private ObstaclePack[] _obstaclesPack;
    [SerializeField] private ObstaclePack _obstaclesPack1;
    private Transform _lastPositionPlane = null;

    private void Start()
    {
        GenerateLevel();
        for (int i = 0; i < 20; i++)
        {
            GeneratePlane();
        }
    }
    
    private void GenerateLevel()
    {
        float roadLength = _pathCreator.path.length;
        float distanceBetweenTower = roadLength / _humanTowerCount;
        float distanceTraveled = 0;
        Vector3 spownPoint;
        Vector3 spownPointBooster;
        Vector3 spownPointObstacle;

        for (int i = 0; i < _humanTowerCount; i++)
        {
            distanceTraveled += distanceBetweenTower;
            float distaanceBetweenBoosters = distanceTraveled - 4f;
            float distanceBetweenObstacle = distanceTraveled - 10f;
            spownPoint = _pathCreator.path.GetPointAtDistance(distanceTraveled, EndOfPathInstruction.Stop);
            spownPointBooster = _pathCreator.path.GetPointAtDistance(distaanceBetweenBoosters, EndOfPathInstruction.Loop);
            spownPointObstacle = _pathCreator.path.GetPointAtDistance(distanceBetweenObstacle, EndOfPathInstruction.Loop);
            Tower newTower = Instantiate(_tower, spownPoint, Quaternion.identity);
            Instantiate(_booster, spownPointBooster,
                Quaternion.FromToRotation(transform.position, newTower.transform.position));
            CreateObstacle(spownPointObstacle, i, _humanTowerCount);
        }
    }

    private void GeneratePlane()
    {
        Vector3 pos = (_lastPositionPlane == null) ?
            _startSpownPlane.position
            : _lastPositionPlane.GetComponent<PlatfotmController>().EndPos.position;
        GameObject obj = Instantiate(_plane, pos, Quaternion.identity);
        _lastPositionPlane = obj.transform;
    }

    private void CreateObstacle(Vector3 spownPoint, int count, int humansCount)
    {
        int randomStartIndex = Random.Range(0, 1);
        int randomIndex = Random.Range(0, _obstaclesPack.Length - 1);
        if (count < 3) Instantiate(_obstaclesPack[randomStartIndex],spownPoint, Quaternion.identity);
        else if (count == humansCount) Instantiate(_obstaclesPack[_obstaclesPack.GetUpperBound(0)],spownPoint, Quaternion.identity);
        else Instantiate(_obstaclesPack[randomIndex],spownPoint, Quaternion.identity);
    }
}
