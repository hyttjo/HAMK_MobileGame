using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    GameManager GM;

    private Texture2D logo;
    private Texture2D mainmenu;
    private Texture2D background;

    void Awake() {
        GM = FindObjectOfType<GameManager>();

        logo = (Texture2D)Resources.Load("Textures/MobileGame");
        mainmenu = (Texture2D)Resources.Load("Textures/MainMenu");
        background = (Texture2D)Resources.Load("Textures/Background");
    }

    public void OnGUI() {
        if (logo != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width / 2, Screen.height / 2 - 250, logo.width, logo.height), logo);
        }

        GUI.DrawTexture(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 90, 250, 225), background);

        GUI.BeginGroup (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 800));
            if (mainmenu != null) {
                GUI.Box(new Rect (10, 0, 180, 40), mainmenu);
            }
            if (GUI.Button (new Rect (10, 40, 180, 40), "Start")) {
                StartGame();
            }
            if (GUI.Button (new Rect (10, 120, 180, 40), "Quit")) {
                Quit();
            }
        GUI.EndGroup();
    }

    public void StartGame() {
        GM.SetGameState(GameState.LoadLevel);
    }

    public void Quit() {
        Debug.Log("Application Quit!");
        Application.Quit();
    }
}