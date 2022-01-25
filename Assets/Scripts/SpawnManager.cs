using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // public Transform enemySpawn;
    public GameObject playerInstance;
    public List<Transform> enemySpawns; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        var randomSpawnIndex = Random.Range(0, enemySpawns.Count - 1);

        var _enemyInstance = Instantiate(enemyPrefab, enemySpawns[randomSpawnIndex].position, Quaternion.identity);
        _enemyInstance.GetComponent<Navigation>().goal = playerInstance.transform;
        return _enemyInstance;
    }
}
