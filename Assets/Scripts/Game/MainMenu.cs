using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    GameManager GM;

    private Texture2D logo;
    private Texture2D mainmenu;

    void Awake() {
        GM = FindObjectOfType<GameManager>();

        logo = (Texture2D)Resources.Load("Textures/MobileGame");
        mainmenu = (Texture2D)Resources.Load("Textures/MainMenu");
    }

    public void OnGUI() {
        if (logo != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width / 2, Screen.height / 2 - 250, logo.width, logo.height), logo);
        }

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
        GM.SetGameState(GameState.NextLevel);
    }

    public void Quit() {
        Debug.Log("Apllication Quit!");
        Application.Quit();
    }
}