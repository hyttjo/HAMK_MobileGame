using UnityEngine;

public class Projectile : MonoBehaviour {

    private CharacterControl cControl;
    private Rigidbody2D rBody;

    public float speedX = 8;
    public float speedY = -4;
    public float decayTime = 1;
    private float aliveTimer;

    void Start() {
        cControl = GetComponent<CharacterControl>();
        rBody = GetComponent<Rigidbody2D>();

        if (rBody != null && cControl != null) {
            rBody.AddForce(transform.up * speedY, ForceMode2D.Impulse);
            rBody.AddForce(transform.right * speedX * cControl.move, ForceMode2D.Impulse);
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
            Destroy(gameObject);
        }
    }

    void Rotate() {
        if (rBody != null) {
            Vector2 moveDirection = rBody.velocity;

            if (moveDirection != Vector2.zero) {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
}
