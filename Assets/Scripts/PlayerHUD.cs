using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

    GameManager GM;

    public GameObject player;
    private MovementControl mControl;
    private Health health;
    private Score score;
    public Font font;
    public Sprite livesSprite;
    public Sprite healthSprite;
    public Sprite coinSprite;
    public Sprite fireSprite;
    public Sprite iceSprite;
    private Texture2D livesTexture;
    private Texture2D healthTexture;
    private Texture2D coinTexture;
    private Texture2D fireTexture;
    private Texture2D iceTexture;
    public int numberOfHearts = 3;
    public int hudHeight = 50;
    public Rect padding = new Rect(10, 10, 10, 10);
    public Color hudBackgroundColor = new Color(0, 0, 0, 0.75f);
    public Color color = new Color(1, 1, 1, 1);

    private Texture2D texture;
    private Texture2D background;

    void Start() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        GM = FindObjectOfType<GameManager>();

        if (player != null) {
            mControl = player.GetComponent<MovementControl>();
            health = player.GetComponent<Health>();
            score = player.GetComponent<Score>();

            if (livesSprite != null) {
                livesTexture = Misc.GetTextureFromSprite(livesSprite);
            }
            if (healthSprite != null) {
                healthTexture = Misc.GetTextureFromSprite(healthSprite);
            }
            if (coinSprite != null) {
                coinTexture = Misc.GetTextureFromSprite(coinSprite);
            }
            if (fireSprite != null) {
                fireTexture = Misc.GetTextureFromSprite(fireSprite);
            }
            if (iceSprite != null) {
                iceTexture = Misc.GetTextureFromSprite(iceSprite);
            }
        }
        background = new Texture2D(1, 1);
        background.SetPixel(0, 0, hudBackgroundColor);
        background.Apply();
    }

    void OnGUI() {
        if (font != null) {
            GUI.skin.font = font;
        }

        if (player != null) {
            DisplayBackground();
            DisplayLives();
            DisplayHealth();
            DisplayCoins();
            DisplayPowerUp();
            DisplayScore();
        }
    }

    void DisplayBackground() {
        GUI.skin.box.normal.background = background;
        GUI.Box(new Rect(0, 0, Screen.width, hudHeight), GUIContent.none);
    }

    void DisplayLives() {
        if (GM != null) {
            int playerLives = GM.playerLives;

            if (livesTexture != null) {
                Rect rect = new Rect(padding.x + 10, padding.y + 2, 20, 20);
                GUI.DrawTexture(rect, livesTexture);
            }
            GUI.Label(new Rect(padding.x + 32, padding.y, Screen.width - padding.width, hudHeight - padding.height), "*" + playerLives);
        }
    }

    void DisplayHealth() {
        if (health != null) {
            int healthInt = (int)health.health;

            if (healthInt < 0) {
                healthInt = 0;
            }

            GUI.Label(new Rect(padding.x + 100, padding.y, Screen.width - padding.width, hudHeight - padding.height), "HP:");

            if (healthTexture != null) {
                for (int i = 0; i < health.health / Mathf.CeilToInt(100 / numberOfHearts); i++) {
                    Rect rect = new Rect(padding.x + 139 + 20 * i + 4 * i, padding.y + 2, 20, 20);
                    GUI.DrawTexture(rect, healthTexture);
                }
            }
        }
    }

    void DisplayCoins() {
        if (score != null) {
            int coinInt = score.GetTotalCoins();

            GUI.Label(new Rect(padding.x + Screen.width / 3.3f, padding.y, Screen.width - padding.width, hudHeight - padding.height), "Coins:");

            if (coinTexture != null) {  
                Rect rect = new Rect(padding.x + Screen.width / 3.3f + 78, padding.y + 2, 20, 20);
                GUI.DrawTexture(rect, coinTexture);
            }
            string coinText = "*" + coinInt;
            GUI.Label(new Rect(padding.x + Screen.width / 3.3f + 100, padding.y, Screen.width - padding.width, hudHeight - padding.height), coinText);
        }
    }

    void DisplayPowerUp() {
        if (mControl != null) {
            GUI.Label(new Rect(padding.x + Screen.width / 1.8f, padding.y, Screen.width - padding.width, hudHeight - padding.height), "Power:");

            if (mControl.currentPower != null && mControl.currentPower.gameObject.tag == "DamageTypeFire") {
                Rect rect = new Rect(padding.x + Screen.width / 1.8f + 100, padding.y - 4, 16, 26);
                GUI.DrawTexture(rect, fireTexture);
            }

            if (mControl.currentPower != null && mControl.currentPower.gameObject.tag == "DamageTypeIce") {
                Rect rect = new Rect(padding.x + Screen.width / 1.8f + 100, padding.y - 4, 16, 26);
                GUI.DrawTexture(rect, iceTexture);
            }
        }
    }

    void DisplayScore() {
        if (score != null) {
            string scoreText;

            if (GM != null) {
                scoreText = "Score: " + score.GetTotalScore();
            } else {
                scoreText = "Score: " + score.GetScore();
            }
            GUI.Label(new Rect(padding.x + Screen.width / 1.3f, padding.y, Screen.width - padding.width, hudHeight - padding.height), scoreText);
        }
    }
}