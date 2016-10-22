using UnityEngine;
using System.Collections;

public delegate void OnPickUpDelegate(GameObject e);

public class PickupControl : MonoBehaviour {

    /*
     * Tämä skripti käsittelee kaikkien pickup-tyylisten objektien toimintaa. Containereille on oma skriptinsä.
     * Erilaisia pickupeja ovat (Coin, Heart, Fireball, Iceshard) ja ainahan lisää voi tehdä, jos vain haluaa.
     * Pickpit käyttäytyvät eri tavoin, joten oikean "Behaviour" asetuksen valinta Unity Editorissa on tärkeää.
     */ 

    public static event OnPickUpDelegate OnCoinCollected;
    public static event OnPickUpDelegate OnHeartCollected;
    public static event OnPickUpDelegate OnPowerUpCollected;

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

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.transform.parent != null) {
            string colliderTag = col.gameObject.transform.parent.gameObject.tag;

            if (colliderTag == "Player") { // Jos pelaaja osuu pickupiin
                if (behaviour == Pickups.Heart) {
                    OnHeartCollected(null);
                    Destroy(); // Poistetaan tämä pickup pelimaailmasta.
                }

                if (behaviour == Pickups.Coin) {
                    OnCoinCollected(null);
                    AudioControl.PlayerCollectCoin(null);
                    Destroy(); // Poistetaan tämä pickup pelimaailmasta.
                }

                if (behaviour == Pickups.PowerUp) {
                    OnPowerUpCollected(powerUp);
                    Destroy(); // Poistetaan tämä pickup pelimaailmasta.
                }
            }
        }
    }

    private void Decay(){
        decayTimer += Time.deltaTime;
        if (decayTimer >= decayTime && !ignoreDecay){
            Destroy();
        }
    }

    private void SpawnJump() {
        Rigidbody2D rBody = GetComponent<Rigidbody2D>();

        if (rBody != null){
            rBody.AddForce(Vector2.up, ForceMode2D.Impulse);
        }
    }

    private void Destroy() {
        Destroy(gameObject);
    }
}
