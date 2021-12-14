using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState
{
    Prewave,
    Wave
};
public class GameManager : MonoBehaviour
{

    public float antWaveCoefficient = 2.0f;
    public GameObject playerInstance;
    public GameObject antPrefab;

    public Text waveCounter;

    public float timeBetweenWaves = 3f;

    private int _enemiesAlive = 0;
    private bool _playerAlive = true;
    private int _currentWave = 1;
    private SpawnManager _spawnManager;
    private GameState state = GameState.Prewave;
    // Start is called before the first frame update
    void Start()
    {
        SetCurrentWave();
        _spawnManager = GetComponent<SpawnManager>();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemiesAlive == 0 && state == GameState.Wave)
        {
            StartCoroutine(StartNewWave());
        }
    }

    void SpawnEnemies()
    {
        for(int i = 0; i < Mathf.FloorToInt(_currentWave*antWaveCoefficient); i++)
        {
            _spawnManager.SpawnEnemy(antPrefab);
        }
        _enemiesAlive += Mathf.FloorToInt(_currentWave*antWaveCoefficient);
        state = GameState.Wave;
    }

    IEnumerator StartNewWave()
    {
        _currentWave++;
        SetCurrentWave();
        state = GameState.Prewave;
        yield return new WaitForSeconds(timeBetweenWaves);
        SpawnEnemies();
    }

    public void SetCurrentWave()
    {
        waveCounter.text = $"{_currentWave}";
    }

    public void EnemyDied()
    {
        _enemiesAlive--;
    }
}
