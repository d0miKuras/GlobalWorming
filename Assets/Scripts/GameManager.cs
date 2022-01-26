using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum GameState
{
    Pregame,
    Prewave,
    Wave
};
public class GameManager : MonoBehaviour
{

    public float antWaveCoefficient = 2.0f;
    public GameObject playerInstance;
    public GameObject antPrefab;

    public Text waveCounter;
    public Text scoreUI;

    public float timeBetweenWaves = 3f;

    private int _enemiesAlive = 0;
    private bool _playerAlive = true;
    private int _currentWave = 1;
    private SpawnManager _spawnManager;
    private GameState state = GameState.Pregame;

    public Animation startAnimation;

    public GameObject[] objectsToDisableOnStart;
    public GameObject[] objectsToEnableOnStart;
    public GameObject[] objectsToDisableOnDeath;
    GameObject deathScreen;
    public float playerScore = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0.0f;
        deathScreen = GameObject.Find("HUD").transform.Find("DeathScreen").gameObject;
        deathScreen.SetActive(false);
        state = GameState.Pregame;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(state == GameState.Pregame && other.tag == "Player")
        {
            startAnimation.Play();
            foreach (GameObject gm in objectsToDisableOnStart)
                gm.SetActive(false);
            foreach (GameObject gm in objectsToEnableOnStart)
                gm.SetActive(true);
            StartWaves();
        }
    }

    void StartWaves()
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
        gameObject.GetComponent<AudioSource>().Play();
        _currentWave++;
        state = GameState.Prewave;
        yield return new WaitForSeconds(timeBetweenWaves);
        SetCurrentWave();
        SpawnEnemies();
    }

    public void SetCurrentWave()
    {
        waveCounter.text = $"{_currentWave}";
    }

    public void SetScore()
    {
        scoreUI.text = $"{playerScore}";
    }

    public void AddPoints(float points)
    {
        playerScore += points;
        SetScore();
    }

    public void EnemyDied()
    {
        _enemiesAlive--;
        AddPoints(_currentWave);
    }

    public void PlayerDied()
    {
        deathScreen.GetComponentInChildren<Text>().text = $"YOU DIED\n\nYOUR SCORE:\n{playerScore}";
        deathScreen.SetActive(true);
        foreach (GameObject gm in objectsToDisableOnDeath)
            gm.SetActive(false);
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSecondsRealtime(5);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

}
