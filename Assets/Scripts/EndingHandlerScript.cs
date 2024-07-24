using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingHandlerScript : MonoBehaviour
{
    public void Win()
    {
        var text = End();
        text.text = "You win!";
        transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
        Time.timeScale = 0;
        PlayerScript.Instance.audi.Stop();
        StartCoroutine(GameManager.Instance.TurnOffLight());
    }

    public void Die()
    {
        var text = End();
        // Set the text to "Game Over"
        text.text = "Game Over";
    }

    private TMPro.TextMeshProUGUI End()
    {
        // Disable Pause button
        // GameObject.Find("Pause Button").SetActive(false);
        // Get TextMeshPro component
        transform.GetChild(0).gameObject.SetActive(true);
        return GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void Home()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GameManager.Instance.DestroyAll();
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        PlayerScript.Instance.Resurrect();
        ChangeRoomScript.ReloadRoom();
        Time.timeScale = 1;
    }
}
