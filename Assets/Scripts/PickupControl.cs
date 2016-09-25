using UnityEngine;

public class PickupControl : MonoBehaviour {
    /*
     * Tämä scripti-tiedosto käsittelee Pickup-objekteihin liittyviä toimintoja
     * Esimerkiksi mitä tapahtuu kun pelaaja koskee PickupBoxFire-laatikkoon? (syntyy poimittava PickupFire)
     * Entäpä mitä tapahtuu kun PickupFire syntyy? (Se lentää ilmaan)
     * Mitä jos pelaaja nappaa PickupFire-objektin? (Pelaaja saa sen sisältämän "taikavoiman" itselleen)
     */
    public bool container = true;
    public GameObject pickup;

    void Start() {
        if (container) {
            SpriteRenderer[] sRenderer = GetComponentsInChildren<SpriteRenderer>();

            if (sRenderer != null && pickup != null) {
                sRenderer[1].sprite = pickup.GetComponent<SpriteRenderer>().sprite;
            }
        }

        Rigidbody2D rBody = GetComponent<Rigidbody2D>();

        if (rBody != null) {
            rBody.AddForce(Vector2.up, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") { //Jos pelaaja osuu pickupiin
            CharacterControl cControl = col.gameObject.GetComponent<CharacterControl>(); //Tämä poimii viittauksen pelaajan CharacterControl -scriptiin

            if (cControl != null) {
                if (container) { //Jos osuttava pickup onkin laatikko
                    SpawnPickup();
                } else {
                    cControl.projectile = pickup;
                    Destroy(gameObject);
                }
            }
        }
    }

    void SpawnPickup() {
        //Tähän voisi kehitellä animaation alkamisen (laatikko hajoaa)
        Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
        Instantiate(pickup, spawnPosition, Quaternion.Euler(0, 0, 270)); //Syntyy poimittava power-up
        Destroy(gameObject);
    }
}