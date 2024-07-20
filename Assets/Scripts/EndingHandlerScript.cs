using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingHandlerScript : MonoBehaviour
{
    public void Win()
    {
        // TODO: Implement the Win method
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
        GameObject.Find("Pause Button").SetActive(false);
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
        ChangeRoomScript.ReloadRoom();
        PlayerScript.Instance.Resurrect();
        Time.timeScale = 1;
    }
}
