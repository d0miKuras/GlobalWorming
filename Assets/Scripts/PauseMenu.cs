using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    [HideInInspector]
    public bool gamePaused = false;
    Keyboard keyboard = Keyboard.current;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gamePaused = false;
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

    void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
    }

    void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        gamePaused = false;
    }
}
