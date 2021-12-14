using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform enemySpawn;
    public GameObject playerInstance;


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
        var _enemyInstance = Instantiate(enemyPrefab, enemySpawn.position, Quaternion.identity);
        _enemyInstance.GetComponent<Navigation>().goal = playerInstance.transform;
        return _enemyInstance;
    }
}
