using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterControl : MonoBehaviour {

    public GameObject character;
    private Animator anim;
    private SpriteRenderer sRenderer;
    private Rigidbody2D rBody;

    public float maxSpeed = 20;
    public float speed = 20;
    public float jumpForce = 12;
    public float airSpeed = 10;

    public int facingDir = 1;
    private float move = 1;
    private bool jumping = false;

    public float shotsPerSecond = 1.5f;
    private float lastShotTimer = 0;
    private float timer = 0;
    private bool canShoot = false;

    public GameObject projectile;

    void Start () {
        if (character != null) {
            anim = character.GetComponent<Animator>();
            sRenderer = character.GetComponent<SpriteRenderer>();
            rBody = GetComponent<Rigidbody2D>();
        } else {
            anim = GetComponentInChildren<Animator>();
            sRenderer = GetComponentInChildren<SpriteRenderer>();
            rBody = GetComponentInChildren<Rigidbody2D>();
        }
    }

    void Update() {
        CheckAndResetShooting();
    }

    void FixedUpdate() {    	
    	Move();
    }
    
    public void Move() {
        if (character != null && rBody != null) {
            rBody.AddForce(new Vector2(move * facingDir * speed, 0), ForceMode2D.Force);

            if (rBody.velocity.sqrMagnitude > maxSpeed) {
                rBody.velocity *= 0.99f;
            }

            anim.SetFloat("Speed", move);

            move = 1;
        }
    }
    
    public void MoveLeft() {
        if (character != null) {
            facingDir = -1;
            sRenderer.flipX = true;
        }
    }
    
    public void MoveRight() {
        if (character != null) {
            facingDir = 1;
            sRenderer.flipX = false;
        }
    }
    
    public void Jump() {
        if (character != null && rBody != null) {
            Vector2 position = transform.position;

            RaycastHit2D hit = Physics2D.Raycast(position, position + Vector2.down * 0.5f);

            Debug.DrawLine(position, position + Vector2.down * 0.5f, Color.red, 1f);

            if (hit.collider != null) {
                if (!jumping) {
                    rBody.AddForce(new Vector2(0, 1 * jumpForce), ForceMode2D.Impulse);
                    jumping = true;
                    speed = speed / airSpeed;
                }
            }
        }
    }
    
    public void Idle() {
        move = 0;
    }
    
    void OnCollisionEnter2D(Collision2D col) {
    	if (jumping) {
    		jumping = false;
    		speed = speed * airSpeed;
    	}
    }

    public void Shoot() {
        if (projectile != null && canShoot) {
            Vector2 spawnPosition = new Vector2((transform.position.x + (0.75f * move)), (transform.position.y + 1));
            GameObject projectile_go = (GameObject)Instantiate(projectile, spawnPosition, Quaternion.identity);
            Projectile projectile_co = projectile_go.GetComponent<Projectile>();
            projectile_co.parent = gameObject;      
            lastShotTimer = timer;
            canShoot = false;
        }
    }

    void CheckAndResetShooting() {
        timer += Time.deltaTime;

        if (!canShoot) {
            if (lastShotTimer + (1 / shotsPerSecond) < timer) {
                canShoot = true;
            }
        }
    }
}