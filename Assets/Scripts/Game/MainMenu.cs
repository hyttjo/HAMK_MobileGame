using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    GameManager GM;

    private enum Difficulty { Easy, Normal, Hard }
    private Difficulty difficulty = Difficulty.Normal;

    public Transition startGameTransition = Transition.BoxIn;
    public Transition quitGameTransition = Transition.HorizontalIn;

    private CameraControl camControl;

    private Texture2D logo;
    private Texture2D background;

    void Awake() {
        GM = FindObjectOfType<GameManager>();
        GM.playerLives = 3;

        camControl = Camera.main.GetComponent<CameraControl>();

        logo = (Texture2D)Resources.Load("Textures/MobileGame");
        background = (Texture2D)Resources.Load("Textures/Background");
    }

    public void OnGUI() {
        if (logo != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width / 2, Screen.height / 2 - 250, logo.width, logo.height), logo);
        }

        GUI.DrawTexture(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 90, 250, 245), background);

        GUI.BeginGroup (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 800));
            GUI.Label(new Rect(27, 0, 160, 40), "Main Menu");

            if (GUI.Button (new Rect (10, 40, 180, 40), "New game")) {
                PlayStartGameTransition();
            }
            if (difficulty == Difficulty.Easy) {
                if (GUI.Button (new Rect (10, 100, 180, 40), "Easy")) {
                    difficulty = Difficulty.Normal;
                    GM.playerLives = 5;
                }
            } else if (difficulty == Difficulty.Normal) {
                if (GUI.Button (new Rect (10, 100, 180, 40), "Normal")) {
                    difficulty = Difficulty.Hard;
                    GM.playerLives = 3;
                }
            } else if (difficulty == Difficulty.Hard) {
                if (GUI.Button (new Rect (10, 100, 180, 40), "Hard")) {
                    difficulty = Difficulty.Easy;
                    GM.playerLives = 10;
                }
            }
            if (GUI.Button (new Rect (10, 160, 180, 40), "Quit")) {
                PlayQuitGameTransition();
            }
        GUI.EndGroup();
    }

    private void PlayStartGameTransition() {
        if (camControl != null) {
            CameraControl.OnTransitionFinish += StartGame;
            camControl.transition = startGameTransition;
        }
    }

    public void StartGame() {
        CameraControl.OnTransitionFinish -= StartGame;
        GM.SetGameState(GameState.LoadLevel);
    }

    private void PlayQuitGameTransition() {
        if (camControl != null) {
            CameraControl.OnTransitionFinish += QuitGame;
            camControl.transition = quitGameTransition;
        }
    }

    public void QuitGame() {
        CameraControl.OnTransitionFinish -= QuitGame;
        GM.SetGameState(GameState.QuitGame);
    }
}