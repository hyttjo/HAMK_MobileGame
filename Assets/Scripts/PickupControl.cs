using UnityEngine;

public class PickupControl : MonoBehaviour {
    /*
     * Tämä scripti-tiedosto käsittelee Pickup-objekteihin liittyviä toimintoja
     * Esimerkiksi mitä tapahtuu kun pelaaja koskee PickupBoxFire-laatikkoon? (syntyy poimittava PickupFire)
     * Entäpä mitä tapahtuu kun PickupFire syntyy? (Se lentää ilmaan)
     * Mitä jos pelaaja nappaa PickupFire-objektin? (Pelaaja saa sen sisältämän "taikavoiman" itselleen)
     */
    public bool container = true;
    public float pushForce = 3f;
    public float bumpAmount = 0.5f;
    public float bumpTime = 0.3f;
    public GameObject pickup;

    private SpriteRenderer pickUpSpriteRenderer;
    private Vector2 bumpDirection = Vector2.zero;
    private Vector2 startPosition;
    private float bumpTimer = 0;

    void Start() {
        startPosition = transform.position;

        if (container) {
            SpriteRenderer[] sRenderer = GetComponentsInChildren<SpriteRenderer>();

            if (sRenderer != null && pickup != null) {
                pickUpSpriteRenderer = sRenderer[1];
                pickUpSpriteRenderer.sprite = pickup.GetComponent<SpriteRenderer>().sprite;
            }
        }

        Rigidbody2D rBody = GetComponent<Rigidbody2D>();

        if (rBody != null) {
            rBody.AddForce(Vector2.up, ForceMode2D.Impulse);
        }
    }

    void Update() {
        if (bumpDirection != Vector2.zero) {
            BumpBox();
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (container) {
            if (col.gameObject.tag == "Player") { //Jos pelaaja osuu pickupiin
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
        if (!container) {
            CharacterControl cControl = col.GetComponentInParent<CharacterControl>();
            Health health = col.GetComponentInParent<Health>();

            if (cControl != null && gameObject.tag == "PickupFire") {
                cControl.projectile = pickup;
                Destroy(gameObject);
            }

            if (cControl != null && gameObject.tag == "PickupHeart")
            {
                health.GainHealthPickup();
                Destroy(gameObject);
            }
        }
    }

    public void SpawnPickup(Vector2 location) {
        Vector2 spawnPosition = location + Vector2.up * 0.5f;
        if (pickup != null && container) {
            Instantiate(pickup, spawnPosition, Quaternion.Euler(0, 0, 270)); //Syntyy poimittava power-up
            pickUpSpriteRenderer.enabled = false;
            pickup = null;
        } else {
            Instantiate(pickup, spawnPosition, Quaternion.Euler(0, 0, 0)); //Syntyy poimittava power-up
        }
    }

    void BumpBox() {
        Vector2 endPosition = Vector2.zero;

        if (bumpTimer < bumpTime / 2) {
            endPosition = startPosition + bumpDirection * bumpAmount;
            transform.position = Vector3.Slerp(startPosition, endPosition, bumpTimer / bumpTime);  
        } else if (bumpTimer > bumpTime / 2 && bumpTimer < bumpTime) {
            transform.position = Vector3.Slerp(transform.position, startPosition, bumpTimer / bumpTime);
        } else {
            bumpTimer = 0;
            bumpDirection = Vector2.zero;
        }
        bumpTimer += Time.deltaTime;
    }
}