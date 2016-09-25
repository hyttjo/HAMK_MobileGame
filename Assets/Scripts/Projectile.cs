using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject parent;
    private CharacterControl cControl;
    private Rigidbody2D rBody;

    public float speedX = 5;
    public float speedY = -4;
    public float decayTime = 2;
    private float aliveTimer;

    void Start() {
        cControl = parent.GetComponent<CharacterControl>();
        rBody = GetComponent<Rigidbody2D>();

        if (rBody != null && cControl != null) {
            rBody.AddForce(new Vector2(speedX * cControl.facingDir, speedY), ForceMode2D.Impulse);
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
