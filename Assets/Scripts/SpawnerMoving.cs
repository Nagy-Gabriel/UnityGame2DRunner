using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMoving : MonoBehaviour
{
    public GameObject movingObstaclePrefab;
    public Vector3 spawnPointPosition = new Vector3(0f, 0f, 0f); // Define spawn point position here or adjust in the inspector
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;

    private float timeUntilObstacleSpawn;

    private void Update()
    {
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;

        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            SpawnMovingObstacle();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void SpawnMovingObstacle()
    {
        GameObject spawnedObstacle = Instantiate(movingObstaclePrefab, spawnPointPosition, Quaternion.identity);
        MovingObstacle movingObstacle = spawnedObstacle.GetComponent<MovingObstacle>();

        if (movingObstacle != null)
        {
           //viteza cu care se misca obstacolele
            movingObstacle.speed = obstacleSpeed;
        }
        else
        {
            Debug.LogWarning("Moving obstacle prefab does not have a MovingObstacle component.");
        }
    }
}
