using UnityEngine;
using System.Collections;

public delegate void OnScoreDelegate();

public class Score : MonoBehaviour {
    /*
     * Tämä skripti käsittelee pelaajan pisteitä (Score) ja tätä käyttäen kerätääjn kolikot jne.
     * Vihollisen tappamiselle, kolikon keräämiselle sekä jäljelle jääneille sekunneille määritellään arvo Editorissa.
     * Jäljellä olevat sekunnit lisätään pisteisiin vasta kentän lopussa ja sitä varten on oma funktionsa.
     */

    private int coinsCollected = 0;
    private int heartsCollected = 0;
    private int powerUpsCollected = 0;
    private int bricksDestroyed = 0;
    private int enemiesKilled = 0;

    public int coinValue = 50;
    public int heartValue = 200;
    public int powerUpValue = 150;
    public int brickValue = 25;
    public int enemyValue = 100;

    void Start() {
        PickupControl.OnCoinCollected += CoinCollected;
        PickupControl.OnHeartCollected += HeartCollected;
        PickupControl.OnPowerUpCollected += PowerUpCollected;
        BreakableObject.OnBrickDestroyed += BrickDestroyed;
        Health.OnEnemyKilled += EnemyKilled;
    }

    private void CoinCollected(GameObject e) {
        coinsCollected++;
    }

    private void HeartCollected(GameObject e) {
        heartsCollected++;
    }

    private void PowerUpCollected(GameObject e) {
        powerUpsCollected++;
    }

    private void BrickDestroyed() {
        bricksDestroyed++;
    }

    private void EnemyKilled() {
        enemiesKilled++;
    }

    public int GetCoins() { // Palauttaa kerättyjen kolikoiden määrän. Tätä tarvitaan käyttöliittymää varten.
        return coinsCollected;
    }

    public int GetScore() { // Palauttaa kokonaispisteiden määrän. Tätä tarvitaan käyttöliittymää varten.
        return coinsCollected * coinValue + 
               heartsCollected * heartValue + 
               powerUpsCollected * powerUpValue + 
               bricksDestroyed * brickValue + 
               enemiesKilled * enemyValue;
    }
}
