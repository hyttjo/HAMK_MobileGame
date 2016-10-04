using UnityEngine;

public class Intro : MonoBehaviour {

    GameManager GM;
    public float introTime = 3f;

    private Texture2D logo;
    private Texture2D credits;

    private Texture2D skyboxSky;
    private Texture2D skyboxOcean;
    private Texture2D skyboxGrass;
    private Texture2D skyboxClouds;

    void Awake () {
        GM = FindObjectOfType<GameManager>();

        logo = (Texture2D)Resources.Load("Textures/MobileGame");
        credits = (Texture2D)Resources.Load("Textures/MadeBy");

        skyboxSky = (Texture2D)Resources.Load("Textures/Skybox_sky");
        skyboxOcean = (Texture2D)Resources.Load("Textures/Skybox_ocean");
        skyboxGrass = (Texture2D)Resources.Load("Textures/Skybox_grass");
        skyboxClouds = (Texture2D)Resources.Load("Textures/Skybox_clouds");

        Invoke("LoadMainMenu", introTime);
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
        if (credits != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - credits.width / 2, Screen.height / 2 - 200 + logo.height, credits.width, credits.height), credits);
        }
    }

    public void LoadMainMenu() {
        GM.SetGameState(GameState.MainMenu);
    }
}