using UnityEngine;
using System.Collections;

public class PlayerHUD : MonoBehaviour {

    public GameObject player;
    private Health health;
    private Score score;
    public Font font;
    public Sprite healthSprite;
    public Sprite coinSprite;
    private Texture2D healthTexture;
    private Texture2D coinTexture;
    public int numberOfHearts = 3;
    public int hudHeight = 50;
    public Rect padding = new Rect(10, 10, 10, 10);
    public Color hudBackgroundColor = new Color(0, 0, 0, 0.75f);
    public Color color = new Color(1, 1, 1, 1);

    private Texture2D texture;
    private Texture2D background;

    void Start() {
        if (player != null) {
            player = GameObject.FindGameObjectWithTag("Player");

            health = player.GetComponent<Health>();
            score = player.GetComponent<Score>();

            if (healthSprite != null) {
                healthTexture = Misc.GetTextureFromSprite(healthSprite);
            }
            if (coinSprite != null)
            {
                coinTexture = Misc.GetTextureFromSprite(coinSprite);
            }
        }

        background = new Texture2D(1, 1);
        background.SetPixel(0, 0, hudBackgroundColor);
        background.Apply();
    }

    void OnGUI() {
        GUI.skin.box.normal.background = background;

        GUI.Box(new Rect(0, 0, Screen.width, hudHeight), GUIContent.none);

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
                    Rect rect = new Rect(padding.x + 110 + 20 * i + 4 * i, padding.y + 4, 20, 20);
                    GUI.DrawTexture(rect, healthTexture);
                }
            }
        }

        if (score != null) {
            int coinsInt = (int)score.GetCoins();

            if (coinsInt < 0) {
                coinsInt = 0;
            }

            GUI.Label(new Rect(padding.x + 200, padding.y, Screen.width - padding.width, hudHeight - padding.height), "Coins:");

            Rect rect = new Rect(padding.x + 310 + 20 + 4, padding.y + 4, 20, 20);
            GUI.DrawTexture(rect, coinTexture);

            string coinCount = "*" + coinsInt;
            GUI.Label(new Rect(padding.x + 350, padding.y, Screen.width - padding.width, hudHeight - padding.height), coinCount);
        }
    }
}