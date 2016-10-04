using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    GameManager GM;

    private Texture2D gameOver;
    private Texture2D background;

    void Awake() {
        GM = FindObjectOfType<GameManager>();

        gameOver = (Texture2D)Resources.Load("Textures/GameOver");
        background = (Texture2D)Resources.Load("Textures/Background");
    }

    public void OnGUI() {
        if (gameOver != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - gameOver.width / 2, Screen.height / 2 - 225, gameOver.width, gameOver.height), gameOver);
        }

        GUI.DrawTexture(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 75, 250, 225), background);

        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 800));
            if (GUI.Button(new Rect(10, 40, 180, 40), "Main Menu")) {
                GoToMainMenu();
            }
            if (GUI.Button(new Rect(10, 120, 180, 40), "Quit")) {
                Quit();
            }
        GUI.EndGroup();
    }

    public void GoToMainMenu() {
        GM.SetGameState(GameState.MainMenu);
    }

    public void Quit() {
        Debug.Log("Apllication Quit!");
        Application.Quit();
    }
}