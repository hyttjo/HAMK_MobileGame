using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour {

    /*
     * Tämä skripti käsittelee kaikkien pickup-tyylisten objektien toimintaa. Containereille on oma skriptinsä.
     * Erilaisia pickupeja ovat (Coin, Heart, Fireball, Iceshard) ja ainahan lisää voi tehdä, jos vain haluaa.
     * Pickpit käyttäytyvät eri tavoin, joten oikean "Behaviour" asetuksen valinta Unity Editorissa on tärkeää.
     */ 

    public enum Pickups { Coin, Heart, Fireball, IceShard }
    public Pickups behaviour; // Tämä näkyy vetovalikkona Editorissa. Muista valita oikea valinta pickupeja luodessa!

    public bool ignoreDecay = false; // Onko pickupilla elinaika?
    public float decayTime = 5; // Pickupin elinaika sekunneissa.
    private float decayTimer = 0; // Laskuri elinajan laskemiseen.

	void Start () {
	
	}
	
	void Update () {
        if (!ignoreDecay) { // Jos elinaikaa ei ole merkattu ignoreen...
            Decay(); // ...elinaika kuluu jokaisessa Updatessa.
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (behaviour == Pickups.Heart && col.gameObject.tag == "Player"){ // Jos pelaaja osuu pickupiin JA pickup on Pickups.Heart
            Health health = col.GetComponentInParent<Health>(); // Haetaan pelaajan käyttämä Health-skripti
            if (health != null){ // Jos Health-skripti on olemassa...
                health.GainHealthPickup(); // ...käytetään sen sisäistä funktiota pelaajan parantamiseen...
                Despawn(); // ...ja poistetaan tämä pickup pelimaailmasta.
            }
        }

        if (behaviour == Pickups.Fireball && col.gameObject.tag == "Player")
        { // Jos pelaaja osuu pickupiin JA pickup on Pickups.IceShard
            CharacterControl cControl = col.GetComponentInParent<CharacterControl>(); // Haetaan pelaajan käyttämä CharacterControl-skripti
            if (cControl != null)
            { // Jos CharacterControl-skripti on olemassa...
                cControl.GainFireball(); // ...käytetään sen sisäistä funktiota siihen, että pelaajalle annetaan kyky ampua tulipalloja...
                Despawn(); // ...ja poistetaan tämä pickup pelimaailmasta.
            }
        }

        if (behaviour == Pickups.IceShard && col.gameObject.tag == "Player") { // Jos pelaaja osuu pickupiin JA pickup on Pickups.IceShard
            CharacterControl cControl = col.GetComponentInParent<CharacterControl>(); // Haetaan pelaajan käyttämä CharacterControl-skripti
            if (cControl != null) { // Jos CharacterControl-skripti on olemassa...
                cControl.GainIceShard(); // ...käytetään sen sisäistä funktiota siihen, että pelaajalle annetaan kyky ampua jäätikkuja...
                Despawn(); // ...ja poistetaan tämä pickup pelimaailmasta.
            }
        }
    }

    void Decay(){
        decayTimer += Time.deltaTime;
        if (decayTimer >= decayTime && !ignoreDecay){
            Despawn();
        }
    }

    void Despawn() {
        Destroy(gameObject);
    }
}
