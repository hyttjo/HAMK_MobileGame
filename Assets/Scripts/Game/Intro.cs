using UnityEngine;

public class Intro : MonoBehaviour {

    GameManager GM;

    public Transition introTransition = Transition.BoxOut;
    private CameraControl camControl;
    public float introTime = 1f;

    private Texture2D logo;
    private Texture2D credits;

    private void Awake() {
        GM = FindObjectOfType<GameManager>();

        camControl = Camera.main.GetComponent<CameraControl>();

        logo = (Texture2D)Resources.Load("Textures/MobileGame");
        credits = (Texture2D)Resources.Load("Textures/MadeBy");

        PlayIntroTransition();
    }

    private void Update() {
        if (Input.anyKey) {
            GM.SetGameState(GameState.MainMenu);
        }
    }

    private void OnGUI() {
        if (logo != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width / 2, Screen.height / 2 - 250, logo.width, logo.height), logo);
        }
        if (credits != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - credits.width / 2, Screen.height / 2 - 200 + logo.height, credits.width, credits.height), credits);
        }
    }

    private void PlayIntroTransition() {
        if (camControl != null) {
            CameraControl.OnTransitionFinish += IntroTransitionFinished;
            camControl.transition = introTransition;
        }
    }

    private void IntroTransitionFinished() {
        CameraControl.OnTransitionFinish -= IntroTransitionFinished;
        Invoke("GoToMainMenu", introTime);
    }

    private void GoToMainMenu() {
        GM.SetGameState(GameState.MainMenu);
    }
}