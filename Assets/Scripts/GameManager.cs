using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }
    public static Light2D globalLight;
    [NonSerialized] public Sprite[] weaponSprites;

    public static event Action<GameState> OnGameStateChanged;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            weaponSprites = Resources.LoadAll<Sprite>("Weapon");
            globalLight = GameObject.Find("Lighting").GetComponent<Light2D>();
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateGameState(GameState _state)
    {
        State = _state;
        switch (_state)
        {
            case GameState.Nor:
                break;
            case GameState.Win:
                // TODO: Implement the Win method
                break;
            case GameState.Dead:
                // Find the EndingHandlerScript object in the scene
                var endingHandler = FindObjectOfType<EndingHandlerScript>();
                // Call the Die method
                endingHandler.Die();
                break;
            default:
                break;
        }
        //OnGameStateChanged?.Invoke(_state);
    }

    public enum GameState
	{
        Nor,
		Win,
		Dead
	}
    
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayLowPitchSound(AudioClip _audiclip)
    {
        audioSource.pitch = 0.7f;
        audioSource.PlayOneShot(_audiclip);
    }

    public void PlayNormalPitchSound(AudioClip _audiclip)
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(_audiclip);
    }

    public IEnumerator TempRemoveCollider(GameObject coll, float sec)
    {
        coll.GetComponent<BoxCollider2D>().enabled = false;
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(sec);
        }
        coll.GetComponent<BoxCollider2D>().enabled = true;

    }

    // Call this method to turn off the light
    public IEnumerator TurnOffLight()
    {
        for (int i = 0; i < 5; i++ )
        {
            globalLight.intensity -= 0.1f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void DestroyAll()
    {
        Destroy(PlayerScript.Instance.gameObject);
        Destroy(PlayerUIScript.Instance.gameObject);
        Destroy(gameObject);
    }

}
