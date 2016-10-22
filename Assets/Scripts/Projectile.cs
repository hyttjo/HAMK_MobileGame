using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject parent;
    private MovementControl mControl;
    private Rigidbody2D rBody;
    public GameObject hitEffect;
    public GameObject decayEffect;

    public float speedX = 5;
    public float speedY = -4;
    public float decayTime = 2;
    private float aliveTimer;

    void Start() {
        mControl = parent.GetComponent<MovementControl>();
        rBody = GetComponent<Rigidbody2D>();

        if (parent.tag == "Enemy") {
            gameObject.layer = 12;
        }

        if (rBody != null && mControl != null) {
            rBody.AddForce(new Vector2(speedX * mControl.GetFacingDir(), speedY), ForceMode2D.Impulse);
        }

        AudioControl.PlayerShootFireball(null); //Toistaa äänen
    }

    void OnCollisionEnter2D(Collision2D col) {
        HandleHit(col.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col) {
        HandleHit(col.gameObject);
    }

   void HandleHit(GameObject target) {
        if (target.tag == "Enemy") {
            if (hitEffect != null) {
                Instantiate(hitEffect, target.transform.position, Quaternion.identity);
            }
            Destroy(transform.position);
        } else if (target.tag == "Player") {
            Destroy(transform.position);
        } else if (target.tag == "Iceblock") {
            if (gameObject.tag == "DamageTypeFire") {
                Destroy(target);
                Destroy(target.transform.position);
            }
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
            Destroy(transform.position);
        }
    }

    void Destroy(Vector3 effectPosition) {
        if (decayEffect != null) {
            decayEffect = (GameObject)Instantiate(decayEffect, effectPosition, Quaternion.identity);
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
