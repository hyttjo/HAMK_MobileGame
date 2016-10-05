using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameState { Intro, MainMenu, GameOver, Paused, LoadLevel, NextLevel, Playing, GameFinished, QuitGame }

public class GameManager : MonoBehaviour {
    public GameState gameState { get; private set; }

    public string[] levels = new string[] { "LevelEditor", "Level-1" };
    public int level_index = 0;

    public Scene scene;

	private static GameManager manager = null;
	public static GameManager Manager {
		get { return manager; }
	}
 
	void Awake() {
		GetThisGameManager();
	}
	
	void Update() {
        scene = SceneManager.GetActiveScene();

	    switch (gameState) {
            case GameState.Intro:
                break;

            case GameState.MainMenu:
                if (scene.name != "MainMenu") {
                    SceneManager.LoadScene("MainMenu");
                }
                break;

            case GameState.GameOver:
                break;

            case GameState.Paused:
                break;

            case GameState.NextLevel:
                if (levels != null && levels.Length > 0) {
                    if (level_index < levels.Length) {
                        level_index++;
                    } else {
                        SetGameState(GameState.GameFinished);
                    }
                }
                SetGameState(GameState.LoadLevel);
                break;

            case GameState.LoadLevel:
                if (levels != null && levels.Length > 0) {
                    string level = levels[level_index];

                    if (scene.name != level) {
                        SceneManager.LoadScene(levels[level_index]);
                    }
                }
                SetGameState(GameState.Playing);
                break;

            case GameState.Playing:
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
