using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadSceneAsync(2);
    }
    
    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
}
