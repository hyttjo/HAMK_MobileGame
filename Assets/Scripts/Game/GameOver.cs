using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    GameManager GM;

    public Transition mainMenuTransition = Transition.BoxIn;
    public Transition quitGameTransition = Transition.HorizontalIn;

    private CameraControl camControl;
    private Score score;

    private Texture2D gameOver;
    private Texture2D background;

    private GUIStyle scoreStyle;

    void Awake() {
        GM = FindObjectOfType<GameManager>();

        camControl = Camera.main.GetComponent<CameraControl>();
        if (GM != null) {
            score = GM.scores[GM.levels[GM.level_index]];
        }

        scoreStyle = new GUIStyle();
        scoreStyle.normal.textColor = Color.white;

        gameOver = (Texture2D)Resources.Load("Textures/GameOver");
        background = (Texture2D)Resources.Load("Textures/Background");
    }

    public void OnGUI() {
        if (gameOver != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - gameOver.width / 2, Screen.height / 2 - 225, gameOver.width, gameOver.height), gameOver);
        }

        if (background != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 135, Screen.height / 2 - 75, 270, 320), background);
        }

        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 75, 250, 300));
            if (score != null) {
                scoreStyle.fontSize = 16;

                GUI.Label(new Rect(20, 20, 180, 20), "Score: " + score.GetTotalScore(), scoreStyle);
                
                scoreStyle.fontSize = 10;

                GUI.DrawTexture(new Rect(20, 60, 20, 25), score.GetEnemyTexture());
                GUI.Label(new Rect(55, 65, 180, 20), "killed: " + score.GetTotalEnemies(), scoreStyle);

                GUI.DrawTexture(new Rect(20, 90, 20, 20), score.GetCoinTexture());
                GUI.Label(new Rect(55, 93, 180, 20), "collected: " + score.GetTotalCoins(), scoreStyle);

                GUI.DrawTexture(new Rect(20, 115, 20, 20), score.GetHeartTexture());
                GUI.Label(new Rect(55, 119, 180, 20), "collected: " + score.GetTotalHearts(), scoreStyle);

                GUI.DrawTexture(new Rect(20, 138, 20, 25), score.GetPowerUpTexture());
                GUI.Label(new Rect(55, 146, 180, 20), "collected: " + score.GetTotalPowerUps(), scoreStyle);

                GUI.DrawTexture(new Rect(20, 170, 20, 20), score.GetBrickTexture());
                GUI.Label(new Rect(55, 173, 180, 20), "destroyed: " + score.GetTotalBricks(), scoreStyle);
            }

            if (GUI.Button(new Rect(10, 210, 180, 40), "Main Menu")) {
                PlayMainMenuTransition();
            }
            if (GUI.Button(new Rect(10, 260, 180, 40), "Quit")) {
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