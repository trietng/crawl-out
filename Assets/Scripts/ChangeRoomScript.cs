using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeRoomScript : MonoBehaviour
{
    public int sceneBuildIndex;

    private static int previousSceneBuildIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            previousSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            print("Switch to " + sceneBuildIndex);
            SceneManager.sceneLoaded += OnLoadedCallback;
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }

    static void OnLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        // Find the player object
        var player = GameObject.FindGameObjectWithTag("Player");
        // Get current main camera
        var mainCamera = Camera.main;

        PlayerScript.Instance.mainCam = mainCamera;
        PlayerAttack.PlayerAttackScript.Instance.mainCam = mainCamera;
        FlashLightScript.mainCam = mainCamera;

        // Find the virtual camera object
        var virtualCamera = GameObject.Find("Virtual Camera");
        // Set the virtual camera's follow target to the player
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        // Find ExitAnchor where buildIndex is the previousSceneBuildIndex
        var exitAnchor = GameObject.FindGameObjectsWithTag("ExitAnchor")
        .Where(item => item.GetComponent<ChangeRoomScript>().sceneBuildIndex == previousSceneBuildIndex)
        .FirstOrDefault();
        if (exitAnchor != null)
        {
            // Set the player's position to the exit anchor's position
            var offset = new Vector3(0, 2, 0) * (scene.buildIndex > previousSceneBuildIndex ? -1 : 1);
            player.transform.position = exitAnchor.transform.position + offset;
        }
    }

}
