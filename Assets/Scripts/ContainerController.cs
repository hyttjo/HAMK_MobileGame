using UnityEngine;
using System.Collections;

public class ContainerController : MonoBehaviour {

    /*
     * Tämä skripti on tarkoitettu erilaisille laatikoille ja vihollisille, joiden halutaan sisältävän pickup-tyypin esineen
     * Arvoja voi säätää Unity Editorissa haluamikseen. Tärkeimmät ovat pickup-esineen ja määrän säätäminen.
     */

    private float pushForce = 3f;
    private float bumpAmount = 0.5f;
    private float bumpTime = 0.3f;
    private float bumpTimer = 0;

    public int pickupCount = 1; // Kuinka monta esinettä tässä säiliössä on
    public GameObject pickup; // Minkälaisen esineen tämä säiliö pitää sisällään (esimerkiksi Pickup_Coin jne.)

    public bool showPickupSprite = false; // Näytetäänkö pickup-esineen sprite. Tämä toimii kuitenkin vain laatikoissa, ei esim. vihollisissa.
    private SpriteRenderer pickUpSpriteRenderer;
    private Vector2 bumpDirection = Vector2.zero;
    private Vector2 startPosition;


    void Start () {
        startPosition = transform.position;
        DisplayPickupSprite();
    }
	
	void Update () {
        if (bumpDirection != Vector2.zero) {
            BumpBox();
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (gameObject.tag == "PickupBox") {
            if (col.gameObject.tag == "Player") {
                GameObject collider = col.gameObject;
                Rigidbody2D cRigidBody = collider.GetComponentInParent<Rigidbody2D>();

                Vector2 hitDirection = Misc.GetHitDirection(col.contacts[0].normal);

                if (hitDirection.y == 0 && cRigidBody != null) {
                    cRigidBody.AddForce(-hitDirection * pushForce, ForceMode2D.Force);
                }

                if (hitDirection.y != -1) {
                    SpawnPickup(transform.position);
                    bumpDirection = hitDirection;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        
    }

    void DisplayPickupSprite() { //Tämä funktio vastaa pickup-esineen spriten näyttämisestä laatikoissa.
        if (showPickupSprite && gameObject.tag == "PickupBox") {
            SpriteRenderer[] sRenderer = GetComponentsInChildren<SpriteRenderer>();

            if (sRenderer != null && pickup != null) {
                pickUpSpriteRenderer = sRenderer[1];
                pickUpSpriteRenderer.sprite = pickup.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void SpawnPickup(Vector2 location) {
        Vector2 spawnPosition = location + Vector2.up * 0.5f;
        if (pickup != null) {
            Instantiate(pickup, spawnPosition, Quaternion.identity);
            pickupCount--;
            if (pickupCount <= 0) {
                pickup = null;
                if (pickUpSpriteRenderer != null) {
                    pickUpSpriteRenderer.enabled = false;
                }
            }
        }
    }

    void BumpBox() {
        Vector2 endPosition = Vector2.zero;

        if (bumpTimer < bumpTime / 2) {
            endPosition = startPosition + bumpDirection * bumpAmount;
            transform.position = Vector3.Slerp(startPosition, endPosition, bumpTimer / bumpTime);
        }
        else if (bumpTimer > bumpTime / 2 && bumpTimer < bumpTime) {
            transform.position = Vector3.Slerp(transform.position, startPosition, bumpTimer / bumpTime);
        }
        else {
            bumpTimer = 0;
            bumpDirection = Vector2.zero;
        }
        bumpTimer += Time.deltaTime;
    }
}
