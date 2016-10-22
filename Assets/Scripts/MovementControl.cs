using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementControl : MonoBehaviour {

    public GameObject character;
    private Animator anim;
    private SpriteRenderer sRenderer;
    private Rigidbody2D rBody;

    public float maxSpeed = 20;
    public float speed = 20;
    public float jumpForce = 15;
    public float airSpeed = 10;

    private Vector2 move;
    private Vector2 contactNormal;
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

        if (gameObject.tag == "Player") {
            PickupControl.OnPowerUpCollected += GainPowerUp;
        }
    }

    void Update() {
        CheckAndResetShooting();
    }

    void FixedUpdate() {    	
    	Move();
    }

    void OnCollisionEnter2D(Collision2D col) {
        contactNormal = col.contacts[0].normal;

        if (contactNormal.y > 0.5) {
            if (jumping) {
                jumping = false;
                speed = speed * airSpeed;  
            }
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        contactNormal = Vector2.zero;
    }
    
    public void Move() {
        float moveSpeed = speed;

        if (contactNormal.y > 0.5 && !jumping) {
            move.y = contactNormal.y / 5;
        }

        if (character != null && rBody != null) {
            if (rBody.velocity.sqrMagnitude > maxSpeed) {
                rBody.velocity *= 0.99f;
            } else if (rBody.velocity.sqrMagnitude < maxSpeed / 2) {
                moveSpeed *= 5f;
            }

            rBody.AddForce(move * moveSpeed, ForceMode2D.Force);

            if (anim != null) {
                anim.SetFloat("Speed", move.x);
            }
        }
    }

    public void MoveTo(Vector2 vector) {
        move = (vector - (Vector2)transform.position).normalized;
        UpdateFacingDir();
    }
    
    public void MoveLeft() {
        if (character != null) {
            move.x = -1;
            sRenderer.flipX = true;
        }
    }
    
    public void MoveRight() {
        if (character != null) {
            move.x = 1;
            sRenderer.flipX = false;
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
                    AudioControl.onPlayerJump += AudioControl.PlayPlayerJumpSound;
                    AudioControl.PlayerJump(gameObject); //Toistaa äänen kun pelaaja hyppää
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
        contactNormal = Vector2.zero;
    }

    public void GainPowerUp(GameObject e){
        currentPower = e;
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