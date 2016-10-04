using UnityEngine;

public class Intro : MonoBehaviour {

    GameManager GM;
    public float introTime = 3f;

    private Texture2D logo;
    private Texture2D credits;

    void Awake() {
        GM = FindObjectOfType<GameManager>();

        logo = (Texture2D)Resources.Load("Textures/MobileGame");
        credits = (Texture2D)Resources.Load("Textures/MadeBy");

        Invoke("LoadMainMenu", introTime);
    }

    public void OnGUI() {
        if (logo != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width / 2, Screen.height / 2 - 250, logo.width, logo.height), logo);
        }
        if (credits != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - credits.width / 2, Screen.height / 2 - 200 + logo.height, credits.width, credits.height), credits);
        }
    }

    public void LoadMainMenu() {
        GM.SetGameState(GameState.MainMenu);
    }
}