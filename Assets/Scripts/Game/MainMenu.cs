using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    GameManager GM;

    private Texture2D logo;
    private Texture2D mainmenu;

    private Texture2D skyboxSky;
    private Texture2D skyboxOcean;
    private Texture2D skyboxGrass;
    private Texture2D skyboxClouds;

    void Awake () {
        GM = FindObjectOfType<GameManager>();

        logo = (Texture2D)Resources.Load("Textures/MobileGame");
        mainmenu = (Texture2D)Resources.Load("Textures/MainMenu");

        skyboxSky = (Texture2D)Resources.Load("Textures/Skybox_sky");
        skyboxOcean = (Texture2D)Resources.Load("Textures/Skybox_ocean");
        skyboxGrass = (Texture2D)Resources.Load("Textures/Skybox_grass");
        skyboxClouds = (Texture2D)Resources.Load("Textures/Skybox_clouds");
    }

    public void OnGUI(){
        if (skyboxSky != null) {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), skyboxSky);
        }
        if (skyboxOcean != null) {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), skyboxOcean);
        }
        if (skyboxGrass != null) {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), skyboxGrass);
        }
        if (skyboxClouds != null) {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), skyboxClouds);
        }

        if (logo != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width / 2, Screen.height / 2 - 250, logo.width, logo.height), logo);
        }

        GUI.BeginGroup (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 800));
            if (mainmenu != null) {
                GUI.Box(new Rect (10, 0, 180, 32), mainmenu);
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