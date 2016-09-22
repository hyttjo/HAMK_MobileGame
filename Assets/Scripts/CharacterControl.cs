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
    
    private float move = 0;
    private bool jumping = false;


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

    void FixedUpdate () {    	
    	Move();
    }
    
    public void Move() {
        if (character != null) {
            rBody.AddForce(new Vector2(move * speed, 0), ForceMode2D.Force);

            if (rBody.velocity.sqrMagnitude > maxSpeed) {
                rBody.velocity *= 0.99f;
            }

            anim.SetFloat("Speed", move);
        }
    }
    
    public void MoveLeft() {
        if (character != null) {
            move = -1;
            sRenderer.flipX = true;
        }
    }
    
    public void MoveRight() {
        if (character != null) {
            move = 1;
            sRenderer.flipX = false;
        }
    }
    
    public void Jump() {
        if (character != null) {
            Vector2 position = transform.position;

            RaycastHit2D hit = Physics2D.Raycast(position, position + Vector2.down * 0.1f);

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
}