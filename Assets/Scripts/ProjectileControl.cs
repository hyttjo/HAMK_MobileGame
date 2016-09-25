using UnityEngine;

public class ProjectileControl : MonoBehaviour {

    private CharacterControl cControl;
    private Rigidbody2D rBody;

    public float speedX;
    public float speedY;
    public float decayTime;
    private float aliveTimer;

    void Start() {
        rBody = GetComponent<Rigidbody2D>();
        cControl = GameObject.Find("Player").GetComponent<CharacterControl>();

        if (rBody != null) {
            rBody.AddForce(transform.up * speedY, ForceMode2D.Impulse);
            rBody.AddForce(transform.right * speedX * cControl.GetFacingDir(), ForceMode2D.Impulse);
        }
    }

    void Update() {
        CheckDecay();
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            Destroy(gameObject);
        }
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
