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
    public float hudHeightPercent = 0.08f;
    private int hudScale;
    private Rect padding;
    public Color hudBackgroundColor = new Color(0, 0, 0, 0.75f);
    public Color color = new Color(1, 1, 1, 1);

    private GUIStyle hudStyle;
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
        hudStyle = new GUIStyle();
        hudScale = (int)(Screen.height * hudHeightPercent);
        padding = new Rect(hudScale / 5, hudScale / 5, hudScale / 5, hudScale / 5);

        if (font != null) {
            hudStyle.font = font;
        }
        hudStyle.fontSize = (int)(hudScale / 3.125f);
        hudStyle.normal.textColor = Color.white;
    }

    void OnGUI() {
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
        GUI.Box(new Rect(0, 0, Screen.width, hudScale), GUIContent.none);
    }

    void DisplayLives() {
        if (GM != null) {
            int playerLives = GM.playerLives;

            if (livesTexture != null) {
                Rect rect = new Rect(padding.x + hudScale / 5, padding.y + hudScale / 25, hudScale / 2.5f, hudScale / 2.5f);
                GUI.DrawTexture(rect, livesTexture);
            }
            GUI.Label(new Rect(padding.x + hudScale / 1.56f, padding.y + hudScale / 25, Screen.width - padding.width, hudScale - padding.height), "*" + playerLives, hudStyle);
        }
    }

    void DisplayHealth() {
        if (health != null) {
            int healthInt = (int)health.health;

            if (healthInt < 0) {
                healthInt = 0;
            }

            GUI.Label(new Rect(padding.x + hudScale / 0.55f, padding.y + hudScale / 25, Screen.width - padding.width, hudScale - padding.height), "HP:", hudStyle);

            if (healthTexture != null) {
                for (int i = 0; i < health.health / Mathf.CeilToInt(100 / numberOfHearts); i++) {
                    Rect rect = new Rect(padding.x + hudScale / 0.38f + hudScale / 2.5f * i + hudScale / 12.5f * i, padding.y + hudScale / 25, hudScale / 2.5f, hudScale / 2.5f);
                    GUI.DrawTexture(rect, healthTexture);
                }
            }
        }
    }

    void DisplayCoins() {
        if (score != null) {
            int coinInt = score.GetTotalCoins();

            GUI.Label(new Rect(padding.x + Screen.width / 3.3f, padding.y + hudScale / 25, Screen.width - padding.width, hudScale - padding.height), "Coins:", hudStyle);

            if (coinTexture != null) {  
                Rect rect = new Rect(padding.x + Screen.width / 3.3f + hudScale / 0.65f, padding.y + hudScale / 25, hudScale / 2.5f, hudScale / 2.5f);
                GUI.DrawTexture(rect, coinTexture);
            }
            string coinText = "*" + coinInt;
            GUI.Label(new Rect(padding.x + Screen.width / 3.3f + hudScale / 0.5f, padding.y + hudScale / 25, Screen.width - padding.width, hudScale - padding.height), coinText, hudStyle);
        }
    }

    void DisplayPowerUp() {
        if (mControl != null) {
            GUI.Label(new Rect(padding.x + Screen.width / 1.8f, padding.y + hudScale / 25, Screen.width - padding.width, hudScale - padding.height), "Power:", hudStyle);

            if (mControl.currentPower != null && mControl.currentPower.gameObject.tag == "DamageTypeFire") {
                Rect rect = new Rect(padding.x + Screen.width / 1.8f + hudScale / 0.5f, padding.y - hudScale / 12.5f, hudScale / 3.125f, hudScale / 1.923f);
                GUI.DrawTexture(rect, fireTexture);
            }

            if (mControl.currentPower != null && mControl.currentPower.gameObject.tag == "DamageTypeIce") {
                Rect rect = new Rect(padding.x + Screen.width / 1.8f + hudScale / 0.5f, padding.y - hudScale / 12.5f, hudScale / 3.125f, hudScale / 1.923f);
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
            GUI.Label(new Rect(padding.x + Screen.width / 1.3f, padding.y + hudScale / 25, Screen.width - padding.width, hudScale - padding.height), scoreText, hudStyle);
        }
    }
}