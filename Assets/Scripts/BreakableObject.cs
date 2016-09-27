using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {

    public GameObject debris;
    public Vector2[] breakDirections = new Vector2[] { Vector2.up };
    public float debrisLifetime = 2f;
    public float debrisForce = 1f;

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            Vector2 hitDirection = Misc.GetHitDirection(col.contacts[0].normal);

            foreach (var vector in breakDirections) {

                if (hitDirection == vector) {
                    ActivateDebris();
                    Destroy();
                }
            }
        }
    }

    void ActivateDebris() {
        if (debris != null) {
            debris.SetActive(true);

            Rigidbody2D[] debrisRbs = debris.GetComponentsInChildren<Rigidbody2D>();

            foreach (var rBody in debrisRbs) {
                rBody.AddForce((rBody.position - (Vector2)transform.position) + Vector2.up * debrisForce, ForceMode2D.Impulse);
            }
        }
    }

    void Destroy() {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        
        if (sRenderer != null) {
            sRenderer.enabled = false;
        }
        Invoke("DisableCollider", 0.1f);

        Destroy(gameObject, debrisLifetime);
    }

    void DisableCollider() {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        if (collider != null) {
            collider.enabled = false;
        }
    }
}
