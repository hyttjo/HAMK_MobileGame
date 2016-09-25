using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour {

    /*
     * Tämä scripti-tiedosto käsittelee Pickup-objekteihin liittyviä toimintoja
     * Esimerkiksi mitä tapahtuu kun pelaaja koskee PickupBoxFire-laatikkoon? (syntyy poimittava PickupFire)
     * Entäpä mitä tapahtuu kun PickupFire syntyy? (Se lentää ilmaan)
     * Mitä jos pelaaja nappaa PickupFire-objektin? (Pelaaja saa sen sisältämän "taikavoiman" itselleen)
     */
    private CharacterControl cControl;
    public GameObject Pickup; 

	// Use this for initialization
	void Start () {

        if (gameObject.tag == "PickupFire") //Jos tämä scripti koskee PickupFireä...
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            //rb.AddForce(transform.up * Random.Range(5.75f, 11.25f)); //PickupFire lentää random suuntaan random nopeudella
            rb.AddForce(transform.up * 6, ForceMode2D.Impulse);
        }

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") //Jos pelaaja osuu pickupiin
        {

            cControl = other.gameObject.GetComponent<CharacterControl>(); //Tämä poimii viittauksen pelaajan CharacterControl -scriptiin

            if (gameObject.tag == "PickupBoxFire") //Jos osuttava pickup onkin laatikko
            {
                SpawnFire();
            }
            if (gameObject.tag == "PickupFire") //Jos osuttava pickup onkin laatikko
            {
                GainFire();
            }
        }
    }

    void SpawnFire()
    {
        //Tähän voisi kehitellä animaation alkamisen (laatikko hajoaa)
        Destroy(gameObject); //Laatikko häviää
        Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);
        Instantiate(Pickup, spawnPosition, Quaternion.identity); //Syntyy poimittava power-up
    }

    void GainFire()
    {
        Destroy(gameObject); //Powerup häviää
        cControl.EnableFire(); //Pelaaja saa taikavoiman
    }
}
