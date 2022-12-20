using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
public class ObstacleCreator : MonoBehaviour
{
    [SerializeField] private MainObstacle _mainObstacle;
    public int ObstacleCount;

    private void Start()
    {
        ObstacleCount = Random.Range(1, 3);
        createMainObstacle(ObstacleCount);
    }
    
    public MainObstacle createMainObstacle(int obstacleCount)
    {
        List<MainObstacle> obj = new List<MainObstacle>();

        MainObstacle mainOstacle = GetComponent<MainObstacle>();
        float scale = _mainObstacle.transform.localScale.y;
        Vector3 targetPos = transform.position;

        for (int i = 0; i < obstacleCount; i++)
        {
            if (i == 0)
            {
                obj.Add(Instantiate(_mainObstacle, targetPos, Quaternion.AngleAxis(-90, Vector3.left), transform));
            }
            else
            {
                targetPos.y += scale;
                obj.Add(Instantiate(_mainObstacle, targetPos, Quaternion.AngleAxis(-90, Vector3.left), transform));
            }

        }
        return mainOstacle;
    }
}
