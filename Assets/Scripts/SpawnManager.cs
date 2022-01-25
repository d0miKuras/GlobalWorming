using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // public Transform enemySpawn;
    public GameObject playerInstance;
    public List<Transform> enemySpawns; 

    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        var randomSpawnIndex = Random.Range(0, enemySpawns.Count - 1);

        var _enemyInstance = Instantiate(enemyPrefab, enemySpawns[randomSpawnIndex].position, Quaternion.identity);
        _enemyInstance.GetComponent<Navigation>().goal = playerInstance.transform;
        return _enemyInstance;
    }
}
