using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoskitoSpawner : MonoBehaviour
{
    public Transform[] spawns;
    public GameObject moskitoPrefab;
    public float firstMoskitoSpawn = 40;

    private void Start()
    {
        GameManager.Instance.onGameStart.AddListener(OnGameStart);
    }

    void OnGameStart()
    {
        StartCoroutine(SpawnMoskitoLoop());
    }

    IEnumerator SpawnMoskitoLoop()
    {
        yield return new WaitForSeconds(firstMoskitoSpawn);

        SpawnMoskito();

        while(!GameManager.GameIsOver)
        {
            yield return new WaitForSeconds(DifficultyManager.MoskitoSpawnInterval);

            SpawnMoskito();
        }
    }

    void SpawnMoskito()
    {
        int randomID = Random.Range(0, spawns.Length);
        Vector2 position = spawns[randomID].position;
        Instantiate(moskitoPrefab, position, Quaternion.identity, transform);
    }
}
