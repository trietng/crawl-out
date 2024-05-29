using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	public static GameManager _instance { get; private set; }

    public GameState State { get; private set; }

    public static event Action<GameState> OnGameStateChanged;
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this);
        else _instance = this;
    }
    public void UpdateGameState(GameState _state)
    {
        this.State = _state;
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
}
