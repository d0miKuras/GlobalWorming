using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject[] creditsObjects;
    public GameObject[] mainMenuObjects;

    // Start is called before the first frame update
    void Start()
    {
        BackToMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowCredits()
    {
        foreach (GameObject gm in creditsObjects)
        {
            gm.SetActive(true);
        }
        foreach (GameObject gm in mainMenuObjects)
        {
            gm.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        foreach (GameObject gm in creditsObjects)
        {
            gm.SetActive(false);
        }
        foreach (GameObject gm in mainMenuObjects)
        {
            gm.SetActive(true);
        }
    }

    public void Quit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
