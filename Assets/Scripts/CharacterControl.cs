using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {

    private Animator anim;
    private SpriteRenderer sRenderer;
    private Rigidbody2D rBody;
    
    public float speed = 1;
    public float jumpForce = 1;
    public float airSpeed = 1.5f;
    
    private float move = 0;
    private bool jumping = false;


    void Start () {
        anim = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () {    	
    	Move();
    }
    
    public void Move() {
    	rBody.AddForce(new Vector2(move * speed, 0), ForceMode2D.Force);
    	anim.SetFloat("Speed", move);
    }
    
    public void MoveLeft() {
		move = -1;
		sRenderer.flipX = true;
    }
    
    public void MoveRight() {
		move = 1;
		sRenderer.flipX = false;
    }
    
    public void Jump() {
    	Vector2 position = transform.position;
    	
    	RaycastHit2D hit = Physics2D.Raycast(position, position + Vector2.down * 0.1f);

    	if (hit.collider != null){
    		if (!jumping) {
				rBody.AddForce(new Vector2(0, 1 * jumpForce), ForceMode2D.Impulse);
				jumping = true;
				speed = speed / airSpeed;
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