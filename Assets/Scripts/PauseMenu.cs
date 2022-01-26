using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector]
    public bool gamePaused = false;
    Keyboard keyboard = Keyboard.current;

    [Header("When paused:")]
    public GameObject[] objectsToEnable;
    public AudioSource[] soundsToPause;

    // Start is called before the first frame update
    void Start()
    {
        keyboard = Keyboard.current;
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if(keyboard.escapeKey.wasPressedThisFrame)
        {
            if(!gamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gamePaused = true;
        Time.timeScale = 0.0f;
        foreach (GameObject gm in objectsToEnable)
            gm.SetActive(true);
        foreach (AudioSource ac in soundsToPause)
            ac.Pause();
    }

    public void Resume()
    {
        foreach (GameObject gm in objectsToEnable)
            gm.SetActive(false);
        foreach (AudioSource ac in soundsToPause)
            ac.UnPause();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

    public void Quit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
