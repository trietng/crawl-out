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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            weaponSprites = Resources.LoadAll<Sprite>("Weapon");
            globalLight = GameObject.Find("Lighting").GetComponent<Light2D>();
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
                PlayerWin();
                break;
            case GameState.Dead:
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(_state);
    }

    void PlayerWin()
    {
        Time.timeScale = 0;
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

    public void PlaySound(AudioClip _audiclip)
    {
        GetComponent<AudioSource>().PlayOneShot(_audiclip);
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

    // Do not use this method
    public IEnumerator TurnOffLight()
    {
        for (int i = 0; i < 10; i++ )
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
