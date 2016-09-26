﻿using UnityEngine;

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
        if (col.gameObject.tag == "Player") { //Jos pelaaja osuu pickupiin
            GameObject collider = col.gameObject;
            CharacterControl cControl = collider.GetComponent<CharacterControl>(); //Tämä poimii viittauksen pelaajan CharacterControl -scriptiin
            Rigidbody2D cRigidBody = collider.GetComponent<Rigidbody2D>();

            Vector2 hitDirection = GetHitDirection(col.contacts[0].normal);     

            if (hitDirection.y == 0 && cRigidBody != null) {
                cRigidBody.AddForce(-hitDirection * pushForce, ForceMode2D.Force);
            }

            if (hitDirection.y != -1 && cControl != null) {
                if (container) { //Jos osuttava pickup onkin laatikko
                    SpawnPickup();
                    bumpDirection = hitDirection;
                } else {
                    cControl.projectile = pickup;
                    Destroy(gameObject);
                }
            }
        }
    }

    void SpawnPickup() {
        if (pickup != null) {
            Vector2 spawnPosition = (Vector2)transform.position + Vector2.up * 0.5f;
            Instantiate(pickup, spawnPosition, Quaternion.Euler(0, 0, 270)); //Syntyy poimittava power-up
            pickUpSpriteRenderer.enabled = false;
            pickup = null;
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

    Vector2 GetHitDirection(Vector2 collider) {
        float angle = Vector3.Angle(collider, Vector3.up);

        if (Mathf.Approximately(angle, 0)) {
            return Vector2.up;
        } else if (Mathf.Approximately(angle, 180)) {
            return Vector2.down;
        }else if (Mathf.Approximately(angle, 90)) {
            Vector3 cross = Vector3.Cross(Vector3.forward, collider);
            if (cross.y > 0) {
                return Vector2.right;
            } else {
                return Vector2.left;
            }
        }
        return Vector2.zero;
    }
}