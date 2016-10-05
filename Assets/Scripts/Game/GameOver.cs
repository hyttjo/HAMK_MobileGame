using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    GameManager GM;

    public Transition mainMenuTransition = Transition.BoxIn;
    public Transition quitGameTransition = Transition.HorizontalIn;

    private CameraControl camControl;

    private Texture2D gameOver;
    private Texture2D background;

    void Awake() {
        GM = FindObjectOfType<GameManager>();

        camControl = Camera.main.GetComponent<CameraControl>();

        gameOver = (Texture2D)Resources.Load("Textures/GameOver");
        background = (Texture2D)Resources.Load("Textures/Background");
    }

    void OnEnable() {
        if (GM != null) {
            GM.SetGameState(GameState.GameOver);
        }
    }

    public void OnGUI() {
        if (gameOver != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - gameOver.width / 2, Screen.height / 2 - 225, gameOver.width, gameOver.height), gameOver);
        }

        GUI.DrawTexture(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 75, 250, 225), background);

        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 800));
            if (GUI.Button(new Rect(10, 40, 180, 40), "Main Menu")) {
                PlayMainMenuTransition();
            }
            if (GUI.Button(new Rect(10, 120, 180, 40), "Quit")) {
                PlayQuitGameTransition();
            }
        GUI.EndGroup();
    }

    private void PlayMainMenuTransition() {
        if (camControl != null) {
            CameraControl.OnTransitionFinish += GoToMainMenu;
            camControl.transition = mainMenuTransition;
        }
    }

    public void GoToMainMenu() {
        CameraControl.OnTransitionFinish -= GoToMainMenu;
        GM.SetGameState(GameState.MainMenu);
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