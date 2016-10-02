using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour
{

    public GameObject player;
    private CharacterControl cControl;
    private Health health;
    private Score score;
    public Font font;
    public Sprite healthSprite;
    public Sprite coinSprite;
    public Sprite fireSprite;
    private Texture2D healthTexture;
    private Texture2D coinTexture;
    private Texture2D fireTexture;
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
        if (player != null) {
            cControl = player.GetComponent<CharacterControl>();
            health = player.GetComponent<Health>();
            score = player.GetComponent<Score>();

            if (healthSprite != null) {
                healthTexture = Misc.GetTextureFromSprite(healthSprite);
            }
            if (coinSprite != null) {
                coinTexture = Misc.GetTextureFromSprite(coinSprite);
            }
            if (fireSprite != null) {
                fireTexture = Misc.GetTextureFromSprite(fireSprite);
            }
        }
        background = new Texture2D(1, 1);
        background.SetPixel(0, 0, hudBackgroundColor);
        background.Apply();
    }

    void OnGUI() {
        DisplayBackground();
        DisplayHealth();
        DisplayCoins();
        DisplayPowerUp();
        DisplayScore();
    }

    void DisplayBackground() {
        GUI.skin.box.normal.background = background;
        GUI.Box(new Rect(0, 0, Screen.width, hudHeight), GUIContent.none);
    }

    void DisplayHealth() {
        if (font != null) {
            GUI.skin.font = font;
        }

        if (health != null) {
            int healthInt = (int)health.health;

            if (healthInt < 0) {
                healthInt = 0;
            }

            GUI.Label(new Rect(padding.x, padding.y, Screen.width - padding.width, hudHeight - padding.height), "Health:");

            if (healthTexture != null) {
                for (int i = 0; i < health.health / Mathf.CeilToInt(100 / numberOfHearts); i++) {
                    Rect rect = new Rect(padding.x + 110 + 20 * i + 4 * i, padding.y + 2, 20, 20);
                    GUI.DrawTexture(rect, healthTexture);
                }
            }
        }
    }

    void DisplayCoins() {
        if (font != null) {
            GUI.skin.font = font;
        }

        if (health != null) {
            int coinInt = score.GetCoins();

            GUI.Label(new Rect(padding.x + Screen.width / 3.5f, padding.y, Screen.width - padding.width, hudHeight - padding.height), "Coins:");

            if (coinTexture != null) {  
                Rect rect = new Rect(padding.x + Screen.width / 3.5f + 78, padding.y + 2, 20, 20);
                GUI.DrawTexture(rect, coinTexture);
            }
            string coinText = "*" + coinInt;
            GUI.Label(new Rect(padding.x + Screen.width / 3.5f + 100, padding.y, Screen.width - padding.width, hudHeight - padding.height), coinText);
        }
    }

    void DisplayPowerUp() {
        if (font != null) {
            GUI.skin.font = font;
        }

        if (cControl != null) {

            GUI.Label(new Rect(padding.x + Screen.width / 1.8f, padding.y, Screen.width - padding.width, hudHeight - padding.height), "Power:");

            if (cControl.currentPower != null && cControl.currentPower.gameObject.tag == "DamageTypeFire") {
                Rect rect = new Rect(padding.x + Screen.width / 1.8f + 100, padding.y - 4, fireTexture.width, fireTexture.height);
                GUI.DrawTexture(rect, fireTexture);
            }
        }
    }

    void DisplayScore() {
        if (font != null) {
            GUI.skin.font = font;
        }

        if (health != null) {
            string scoreText = "Score: " + score.GetScore();
            GUI.Label(new Rect(padding.x + Screen.width / 1.3f, padding.y, Screen.width - padding.width, hudHeight - padding.height), scoreText);
        }
    }
}