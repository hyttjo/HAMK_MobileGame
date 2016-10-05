using UnityEngine;
using System.Collections;

public class LevelFinish : MonoBehaviour {

    GameManager GM;

    public Transition nextLevelTransition = Transition.HorizontalIn;
    public Transition quitGameTransition = Transition.BoxIn;

    private CameraControl camControl;

    private Texture2D levelFinish;
    private Texture2D background;

    void Awake() {
        GM = FindObjectOfType<GameManager>();

        camControl = Camera.main.GetComponent<CameraControl>();

        levelFinish = (Texture2D)Resources.Load("Textures/LevelFinished");
        background = (Texture2D)Resources.Load("Textures/Background");
    }

    public void OnGUI() {
        if (levelFinish != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - levelFinish.width / 2, Screen.height / 2 - 225, levelFinish.width, levelFinish.height), levelFinish);
        }

        GUI.DrawTexture(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 75, 250, 225), background);

        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 800));
            if (GUI.Button(new Rect(10, 40, 180, 40), "Next Level")) {
                PlayNextLevelTransition();
            }
            if (GUI.Button(new Rect(10, 120, 180, 40), "Quit")) {
                PlayQuitGameTransition();
            }
        GUI.EndGroup();
    }

    private void PlayNextLevelTransition() {
        if (camControl != null) {
            CameraControl.transitionFinishDelegate += NextLevel;
            camControl.transition = nextLevelTransition;
        }
    }

    private void NextLevel() {
        CameraControl.transitionFinishDelegate -= NextLevel;
        if (GM != null) {
            GM.SetGameState(GameState.NextLevel);
        }
    }

    private void PlayQuitGameTransition() {
        if (camControl != null) {
            CameraControl.transitionFinishDelegate += QuitGame;
            camControl.transition = quitGameTransition;
        }
    }

    private void QuitGame() {
        CameraControl.transitionFinishDelegate -= QuitGame;
        GM.SetGameState(GameState.QuitGame);
    }
}