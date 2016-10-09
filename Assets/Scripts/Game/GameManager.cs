using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum GameState { Intro, MainMenu, GameOver, Paused, LoadLevel, NextLevel, Playing, PlayerDied, LevelFinished, GameFinished, QuitGame }

public class GameManager : MonoBehaviour {
    public GameState gameState { get; private set; }
    public GameObject player;

    public Dictionary<string, Score> scores;
    public string[] levels = new string[] { "Level-0", "Level-1", "Level-2" };
    public int level_index = 0;

    public Scene scene;

    public int playerLives = 5;

	private static GameManager manager = null;
	public static GameManager Manager {
		get { return manager; }
	}
 
	void Awake() {
        scores = new Dictionary<string, Score>();

		GetThisGameManager();
	}
	
	void Update() {
        scene = SceneManager.GetActiveScene();

	    switch (gameState) {
            case GameState.Intro:
                break;

            case GameState.MainMenu:
                scores.Clear();

                if (scene.name != "MainMenu") {
                    SceneManager.LoadScene("MainMenu");
                }
                break;

            case GameState.GameOver:
                player = GameObject.FindGameObjectWithTag("Player");

                if (player != null) {
                    GameOver gameOver = player.GetComponent<GameOver>();

                    if (gameOver == null) {
                        player.AddComponent<GameOver>();
                    }
                }
                level_index = 0;
                break;

            case GameState.Paused:
                break;

            case GameState.NextLevel:
                if (levels != null && levels.Length > 0) {
                    if (level_index < levels.Length) {
                        level_index++;
                        SetGameState(GameState.LoadLevel);
                    } else {
                        SetGameState(GameState.GameFinished);
                    }
                }
                break;

            case GameState.LoadLevel:
                if (levels != null && levels.Length > 0) {
                    if (level_index < levels.Length) {
                        SceneManager.LoadScene(levels[level_index]);
                        SetGameState(GameState.Playing);
                    }
                }  
                break;

            case GameState.Playing:
                break;

           case GameState.PlayerDied:
                playerLives--;

                if (playerLives > 0) { 
                    SetGameState(GameState.LoadLevel);
                } else {
                    SetGameState(GameState.GameOver);
                }
                break;

            case GameState.LevelFinished:
                player = GameObject.FindGameObjectWithTag("Player");

                if (player != null) {
                    if (level_index == levels.Length - 1) {
                        SetGameState(GameState.GameFinished);
                    } else {
                        LevelFinish levelFinish = player.GetComponent<LevelFinish>();

                        if (levelFinish == null) {
                            player.AddComponent<LevelFinish>();
                        }
                    }
                }
                break;

            case GameState.GameFinished:
                Debug.Log("Congratulations, you have finished the Game!");
                break;

            case GameState.QuitGame:
                Debug.Log("Application Quit!");
                Application.Quit();
                break;
        }
	}
 
	void GetThisGameManager() {
		if (manager != null && manager != this) {
			Destroy(this.gameObject);
			return;
		} else {
			manager = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

    public void SetGameState(GameState state) {
        this.gameState = state;
    }
}
