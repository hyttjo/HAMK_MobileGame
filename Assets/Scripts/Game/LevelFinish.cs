using UnityEngine;
using System.Collections;

public class LevelFinish : MonoBehaviour {

    GameManager GM;

    public Transition nextLevelTransition = Transition.HorizontalIn;
    public Transition quitGameTransition = Transition.BoxIn;

    private CameraControl camControl;
    private Score score;

    private Texture2D levelFinish;
    private Texture2D background;

    private GUIStyle scoreStyle;

    void Awake() {
        GM = FindObjectOfType<GameManager>();

        camControl = Camera.main.GetComponent<CameraControl>();
        score = GM.scores[GM.levels[GM.level_index]];

        scoreStyle = new GUIStyle();
        scoreStyle.normal.textColor = Color.white;

        levelFinish = (Texture2D)Resources.Load("Textures/LevelFinished");
        background = (Texture2D)Resources.Load("Textures/Background");
    }

    public void OnGUI() {
        if (levelFinish != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - levelFinish.width / 2, Screen.height / 2 - 225, levelFinish.width, levelFinish.height), levelFinish);
        }

        if (background != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 135, Screen.height / 2 - 75, 270, 320), background);
        }

        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 75, 250, 300));
            if (score != null) {
                scoreStyle.fontSize = 16;

                GUI.Label(new Rect(20, 20, 180, 20), "Score: " + score.GetScore(), scoreStyle);
                
                scoreStyle.fontSize = 8;

                GUI.DrawTexture(new Rect(0, 60, 20, 25), score.GetEnemyTexture());
                GUI.Label(new Rect(30, 65, 180, 20), "Enemies killed: " + score.GetEnemiesKilled(), scoreStyle);

                GUI.DrawTexture(new Rect(0, 90, 20, 20), score.GetCoinTexture());
                GUI.Label(new Rect(30, 93, 180, 20), "Coins collected: " + score.GetCoinsCollected(), scoreStyle);

                GUI.DrawTexture(new Rect(0, 115, 20, 20), score.GetHeartTexture());
                GUI.Label(new Rect(30, 119, 180, 20), "Hearts collected: " + score.GetHeartsCollected(), scoreStyle);

                GUI.DrawTexture(new Rect(0, 138, 20, 25), score.GetPowerUpTexture());
                GUI.Label(new Rect(30, 145, 180, 20), "PowerUps collected: " + score.GetPowerUpsCollected(), scoreStyle);

                GUI.DrawTexture(new Rect(0, 170, 20, 20), score.GetBrickTexture());
                GUI.Label(new Rect(30, 172, 180, 20), "Bricks destroyed: " + score.GetBricksDestroyed(), scoreStyle);
            }

            if (GUI.Button(new Rect(10, 210, 180, 40), "Next Level")) {
                PlayNextLevelTransition();
            }
            if (GUI.Button(new Rect(10, 260, 180, 40), "Quit")) {
                PlayQuitGameTransition();
            }
        GUI.EndGroup();
    }

    private void PlayNextLevelTransition() {
        if (camControl != null) {
            CameraControl.OnTransitionFinish += NextLevel;
            camControl.transition = nextLevelTransition;
        }
    }

    private void NextLevel() {
        CameraControl.OnTransitionFinish -= NextLevel;
        if (GM != null) {
            GM.SetGameState(GameState.NextLevel);
        }
    }

    private void PlayQuitGameTransition() {
        if (camControl != null) {
            CameraControl.OnTransitionFinish += QuitGame;
            camControl.transition = quitGameTransition;
        }
    }

    private void QuitGame() {
        CameraControl.OnTransitionFinish -= QuitGame;
        GM.SetGameState(GameState.QuitGame);
    }
}