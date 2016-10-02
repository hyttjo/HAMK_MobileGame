using UnityEngine;
using System.Collections;

public class PickupControl : MonoBehaviour {

    /*
     * Tämä skripti käsittelee kaikkien pickup-tyylisten objektien toimintaa. Containereille on oma skriptinsä.
     * Erilaisia pickupeja ovat (Coin, Heart, Fireball, Iceshard) ja ainahan lisää voi tehdä, jos vain haluaa.
     * Pickpit käyttäytyvät eri tavoin, joten oikean "Behaviour" asetuksen valinta Unity Editorissa on tärkeää.
     */ 

    public enum Pickups { Coin, Heart, PowerUp }
    public Pickups behaviour; // Tämä näkyy vetovalikkona Editorissa. Muista valita oikea valinta pickupeja luodessa!
    public GameObject powerUp;

    public bool ignoreDecay = false; // Onko pickupilla elinaika?
    public float decayTime = 5; // Pickupin elinaika sekunneissa.
    private float decayTimer = 0; // Laskuri elinajan laskemiseen.

    public bool spawnJump = true; // Hypähtääkö pickup ilmaan sen syntyessä?

	void Start () {

        if (spawnJump) {
            SpawnJump();
        }
	}
	
	void Update () {
        if (!ignoreDecay) { // Jos elinaikaa ei ole merkattu ignoreen...
            Decay(); // ...elinaika kuluu jokaisessa Updatessa.
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.transform.parent != null) {
            string colliderTag = col.gameObject.transform.parent.gameObject.tag;

            if (colliderTag == "Player") { // Jos pelaaja osuu pickupiin
                if (behaviour == Pickups.Heart) { 
                    Health health = col.GetComponentInParent<Health>(); // Haetaan pelaajan käyttämä Health-skripti
                    if (health != null){ // Jos Health-skripti on olemassa...
                        health.GainHealthPickup(); // ...käytetään sen sisäistä funktiota pelaajan parantamiseen...
                        Destroy(); // ...ja poistetaan tämä pickup pelimaailmasta.
                    }
                }

                if (behaviour == Pickups.Coin) {
                    Score score = col.GetComponentInParent<Score>(); // Haetaan pelaajan käyttämä Score-skripti
                    if (score != null) { // Jos Score-skripti on olemassa...
                        score.GainCoin(); // ...käytetään sen sisäistä funktiota siihen, että pelaajalle merkataan kerätty kolikko...
                        Destroy(); // ...ja poistetaan tämä pickup pelimaailmasta.
                    }
                }

                if (behaviour == Pickups.PowerUp && colliderTag == "Player") {
                    CharacterControl cControl = col.GetComponentInParent<CharacterControl>(); // Haetaan pelaajan käyttämä CharacterControl-skripti
                    if (cControl != null) { // Jos CharacterControl-skripti on olemassa...
                        cControl.GainPowerUp(powerUp); // ...käytetään sen sisäistä funktiota siihen, että pelaajalle annetaan kyky ampua tulipalloja...
                        Destroy(); // ...ja poistetaan tämä pickup pelimaailmasta.
                    }
                }
            }
        }
    }

    void Decay(){
        decayTimer += Time.deltaTime;
        if (decayTimer >= decayTime && !ignoreDecay){
            Destroy();
        }
    }

    void SpawnJump() {
        Rigidbody2D rBody = GetComponent<Rigidbody2D>();

        if (rBody != null){
            rBody.AddForce(Vector2.up, ForceMode2D.Impulse);
        }
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
