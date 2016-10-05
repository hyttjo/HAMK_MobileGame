using UnityEngine;
using System.Collections;

public delegate void OnScoreDelegate();

public class Score : MonoBehaviour {
    /*
     * Tämä skripti käsittelee pelaajan pisteitä (Score) ja tätä käyttäen kerätääjn kolikot jne.
     * Vihollisen tappamiselle, kolikon keräämiselle sekä jäljelle jääneille sekunneille määritellään arvo Editorissa.
     * Jäljellä olevat sekunnit lisätään pisteisiin vasta kentän lopussa ja sitä varten on oma funktionsa.
     */

    public Sprite coinSprite;
    public Sprite heartSprite;
    public Sprite powerUpSprite;
    public Sprite enemySprite;
    public Sprite brickSprite;

    private Texture2D coinTexture;
    private Texture2D heartTexture;
    private Texture2D powerUpTexture;
    private Texture2D enemyTexture;
    private Texture2D brickTexture;

    private int coinsCollected = 0;
    private int heartsCollected = 0;
    private int powerUpsCollected = 0;
    private int enemiesKilled = 0;
    private int bricksDestroyed = 0;

    public int coinValue = 50;
    public int heartValue = 200;
    public int powerUpValue = 150;
    public int brickValue = 25;
    public int enemyValue = 100;

    void Start() {
        heartTexture = Misc.GetTextureFromSprite(heartSprite);
        coinTexture = Misc.GetTextureFromSprite(coinSprite);
        powerUpTexture = Misc.GetTextureFromSprite(powerUpSprite);
        enemyTexture = Misc.GetTextureFromSprite(enemySprite);
        brickTexture = Misc.GetTextureFromSprite(brickSprite);

        PickupControl.OnCoinCollected += CoinCollected;
        PickupControl.OnHeartCollected += HeartCollected;
        PickupControl.OnPowerUpCollected += PowerUpCollected;
        BreakableObject.OnBrickDestroyed += BrickDestroyed;
        Health.OnEnemyKilled += EnemyKilled;
    }

    public Texture2D GetHeartTexture() {
        return heartTexture;
    }

    public Texture2D GetCoinTexture() {
        return coinTexture;
    }

    public Texture2D GetPowerUpTexture() {
        return powerUpTexture;
    }

    public Texture2D GetEnemyTexture() {
        return enemyTexture;
    }

    public Texture2D GetBrickTexture() {
        return brickTexture;
    }

    private void HeartCollected(GameObject e) {
        heartsCollected++;
    }

    public int GetHeartsCollected() {
        return heartsCollected;
    }

    private void CoinCollected(GameObject e) {
        coinsCollected++;
    }

    public int GetCoinsCollected() {
        return coinsCollected;
    }

    private void PowerUpCollected(GameObject e) {
        powerUpsCollected++;
    }

    public int GetPowerUpsCollected() {
        return powerUpsCollected;
    }

    private void BrickDestroyed() {
        bricksDestroyed++;
    }

    public int GetBricksDestroyed() {
        return bricksDestroyed;
    }

    private void EnemyKilled() {
        enemiesKilled++;
    }

    public int GetEnemiesKilled() {
        return enemiesKilled;
    }

    public int GetScore() { // Palauttaa kokonaispisteiden määrän. Tätä tarvitaan käyttöliittymää varten.
        return coinsCollected * coinValue + 
               heartsCollected * heartValue + 
               powerUpsCollected * powerUpValue + 
               bricksDestroyed * brickValue + 
               enemiesKilled * enemyValue;
    }
}
