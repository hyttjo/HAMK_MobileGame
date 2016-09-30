using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    /*
     * Tämä skripti käsittelee pelaajan pisteitä (Score) ja tätä käyttäen kerätääjn kolikot jne.
     * Vihollisen tappamiselle, kolikon keräämiselle sekä jäljelle jääneille sekunneille määritellään arvo Editorissa.
     * Jäljellä olevat sekunnit lisätään pisteisiin vasta kentän lopussa ja sitä varten on oma funktionsa.
     */

    private int score = 0;
    private int coinsCollected = 0;
    private int enemiesKilled = 0;
    private int secondsLeft = 0;

    public int coinValue;
    public int enemyValue;
    public int timeValue;

    void Start () {
	
	}
	
	void Update () {
        Debug.Log("Score: " + score);
	}

    public void GainCoin() {
        score += coinValue;
        coinsCollected++;
    }

    public int GetCoins() { // Palauttaa kerättyjen kolikoiden määrän. Tätä tarvitaan käyttöliittymää varten.
        return coinsCollected;
    }
}
