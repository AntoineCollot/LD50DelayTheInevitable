using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisSpawner : MonoBehaviour
{
    public GameObject mantisPrefab;
    public float firstMantisSpawn = 30;

    public Transform[] spawns;

    private void Start()
    {
        GameManager.Instance.onGameStart.AddListener(OnGameStart);
    }

    void OnGameStart()
    {
        StartCoroutine(SpawnMantisLoop());
    }

    IEnumerator SpawnMantisLoop()
    {
        yield return new WaitForSeconds(firstMantisSpawn);

        while(!GameManager.GameIsOver)
        {
            int randomSpawnID = Random.Range(0, spawns.Length);
            Vector3 spawnPos = spawns[randomSpawnID].position;
            Instantiate(mantisPrefab, spawnPos, Quaternion.identity, transform);

            yield return new WaitForSeconds(DifficultyManager.MantisSpawnInterval);
        }
    }
}
