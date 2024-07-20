using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Pause(){
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Home(){
        GameManager.Instance.DestroyAll();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Resume(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart(){
        ChangeRoomScript.ReloadRoom();
        Time.timeScale = 1;
    }

    
}
