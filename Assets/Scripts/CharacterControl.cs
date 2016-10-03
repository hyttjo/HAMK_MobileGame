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
    public float jumpForce = 15;
    public float airSpeed = 10;

    private Vector2 move;
    private bool jumping = false;

    public float shotsPerSecond = 1.5f;
    private float lastShotTimer = 0;
    private float timer = 0;
    private bool canShoot = false;

    public GameObject currentPower;

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

    void OnCollisionEnter2D(Collision2D col) {
        Vector2 hitDirection = Misc.GetHitDirection(col.contacts[0].normal);

        if (hitDirection == Vector2.up) {
            if (jumping) {
                jumping = false;
                speed = speed * airSpeed;
            }
        }
    }
    
    public void Move() {
        float moveSpeed = speed;
        
        if (move.y > 0.1) {
            moveSpeed = speed * 3f;
        } else if (move.y < -0.1) {
            moveSpeed = speed * 1.2f;
        }

        if (character != null && rBody != null) {
            rBody.AddForce(move * moveSpeed, ForceMode2D.Force);

            if (rBody.velocity.sqrMagnitude > maxSpeed) {
                rBody.velocity *= 0.99f;
            }

            anim.SetFloat("Speed", move.x);
        }

        UpdateFacingDir();
    }

    public void MoveTo(Vector2 vector) {
        move = (vector - (Vector2)transform.position).normalized;
    }
    
    public void MoveLeft() {
        if (character != null) {
            move.x = -1;
        }
    }
    
    public void MoveRight() {
        if (character != null) {
            move.x = 1;
        }
    }
    
    public void Jump() {
        if (character != null && rBody != null) {
            Vector2 position = (Vector2)transform.position + Vector2.up;      
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 1.5f);

            if (hit.collider != null) {
                if (!jumping) {
                    rBody.AddForce(new Vector2(0, 1 * jumpForce), ForceMode2D.Impulse);
                    jumping = true;
                    speed = speed / airSpeed;
                }
            }
        }
    }

    private void UpdateFacingDir() {
        if (sRenderer != null) {
            if (move.x < 0) {
                sRenderer.flipX = true;
            } else {
                sRenderer.flipX = false;
            }
        }
    }

    public int GetFacingDir() {
        if (sRenderer.flipX) {
            return -1;
        } else {
            return 1;
        }
    }
    
    public void Idle() {
        move = Vector2.zero;
    }

    public void GainPowerUp(GameObject powerUp){
        currentPower = powerUp;
    }

    public void Shoot() {
        if (currentPower != null && canShoot) {
            Vector2 spawnPosition = new Vector2((transform.position.x + (0.75f * move.x)), (transform.position.y + 0.75f));
            GameObject projectile_go = (GameObject)Instantiate(currentPower, spawnPosition, Quaternion.identity);
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