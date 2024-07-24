using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ChangeRoomScript : MonoBehaviour
{
    public int sceneBuildIndex;

    private static int previousSceneBuildIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeRoom();
        }
    }

    private void ChangeRoom()
    {
        previousSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        print("Switch to " + sceneBuildIndex);
        SceneManager.sceneLoaded += OnLoadedCallback;
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        PlayerScript.Instance.SaveHealth();
        PlayerAttackScript.Instance.SaveInventory();
    }

    public static void ReloadRoom()
    {
        SceneManager.sceneLoaded += OnLoadedCallback;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    static void OnLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            return;
        // Find the player object
        var player = GameObject.FindGameObjectWithTag("Player");
        // Get current main camera
        var mainCam = Camera.main;

        PlayerScript.Instance.mainCam = mainCam;
        PlayerAttackScript.Instance.mainCam = mainCam;
        PlayerScript.Instance.SaveHealth();
        PlayerAttackScript.Instance.SaveInventory();
        GameManager.globalLight = GameObject.Find("Lighting").GetComponent<Light2D>();

        // Find the virtual camera object
        var virtualCamera = GameObject.Find("Virtual Camera");
        // Set the virtual camera's follow target to the player
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        // Find ExitAnchor where buildIndex is the previousSceneBuildIndex
        print("Previous scene build index: " + previousSceneBuildIndex);
        var exitAnchor = GameObject.FindGameObjectsWithTag("ExitAnchor")
        .Where(item => item.GetComponent<ChangeRoomScript>().sceneBuildIndex == previousSceneBuildIndex)
        .FirstOrDefault();
        if (exitAnchor != null)
        {
            // Set the player's position to the exit anchor's position
            var offset = new Vector3(0, 2, 0) * (scene.buildIndex > previousSceneBuildIndex ? -1 : 1);
            player.transform.position = exitAnchor.transform.position + offset;
        }
        PlayerScript.Instance.RevokeInvincibility();
    }

}
