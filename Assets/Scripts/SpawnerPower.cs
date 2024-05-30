using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColectibleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] collectiblePrefabs;
    [SerializeField] private Transform collectibleParent;
    public float collectibleSpawnTime = 2f;
    [Range(0, 1)] public float collectibleSpawnTimeFactor = 0.01f;
    public float collectibleSpeed = 1f;
    [Range(0, 1)] public float collectibleSpeedFactor = 0.02f;

    private float _collectibleSpawnTime;
    private float _collectibleSpeed;
    private float timeAlive;
    private float timeUntilCollectibleSpawn;

    private void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearCollectibles);
        GameManager.Instance.onPlay.AddListener(ResetFactors);
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            timeAlive += Time.deltaTime;

            CalculateFactors();
            SpawnLoop();
        }
    }

    private void SpawnLoop()
    {
        timeUntilCollectibleSpawn += Time.deltaTime;

        if (timeUntilCollectibleSpawn >= _collectibleSpawnTime)
        {
            Spawn();
            timeUntilCollectibleSpawn = 0f;
        }
    }

    private void ClearCollectibles()
    {
        foreach (Transform child in collectibleParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void CalculateFactors()
    {
        _collectibleSpawnTime = collectibleSpawnTime / Mathf.Pow(timeAlive, collectibleSpawnTimeFactor);
        _collectibleSpeed = collectibleSpeed * Mathf.Pow(timeAlive, collectibleSpeedFactor);
    }

    private void ResetFactors()
    {
        timeAlive = 1f;
        _collectibleSpawnTime = collectibleSpawnTime;
        _collectibleSpeed = collectibleSpeed;
    }

    private void Spawn()
    {
        GameObject collectibleToSpawn = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];
        GameObject spawnedCollectible = Instantiate(collectibleToSpawn, transform.position, Quaternion.identity);
        spawnedCollectible.transform.parent = collectibleParent;
        Rigidbody2D collectibleRB = spawnedCollectible.GetComponent<Rigidbody2D>();

        collectibleRB.velocity = Vector2.left * _collectibleSpeed;
    }
}
