using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject parent;
    private MovementControl mControl;
    private Rigidbody2D rBody;
    public GameObject decayEffect;

    public float speedX = 5;
    public float speedY = -4;
    public float decayTime = 2;
    private float aliveTimer;

    void Start() {
        mControl = parent.GetComponent<MovementControl>();
        rBody = GetComponent<Rigidbody2D>();

        if (rBody != null && mControl != null) {
            rBody.AddForce(new Vector2(speedX * mControl.GetFacingDir(), speedY), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            Destroy();
        }
        if (col.gameObject.tag == "Iceblock") {
            // Tämä koodi tässä on sama kuin alla Destroy() funktio, mutta decayEffect tapahtuukin siinä col-objektin kohdalla, eli jääkuutio pössähtää
            if (decayEffect != null) {
                decayEffect = (GameObject)Instantiate(decayEffect, col.transform.position, Quaternion.identity);
                decayEffect.transform.localScale *= 0.75f;
                Destroy(decayEffect, 0.5f);
            }
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }

    void Update() {
        CheckDecay();
    }

    void LateUpdate() {
        Rotate();
    }

    void CheckDecay() {
        aliveTimer += Time.deltaTime;

        if (aliveTimer > decayTime) {
            Destroy();
        }
    }

    void Destroy() {
        if (decayEffect != null) {
            decayEffect = (GameObject)Instantiate(decayEffect, transform.position, Quaternion.identity);
            decayEffect.transform.localScale *= 0.75f;
            Destroy(decayEffect, 0.5f);
        }
        Destroy(gameObject);
    }

    void Rotate() {
        if (rBody != null) {
            Vector2 moveDirection = rBody.velocity;

            if (moveDirection != Vector2.zero) {
                transform.rotation = Quaternion.FromToRotation(Vector3.down, moveDirection);
            }
        }
    }
}
