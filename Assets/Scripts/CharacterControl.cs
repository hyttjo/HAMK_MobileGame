using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {

    private Animator anim;
    private SpriteRenderer sRenderer;
    private Rigidbody2D rBody;
    
    public float speed = 1;
    private float move = 0;
    public bool jumping = false;


    void Start () {
        anim = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
    }

    void Update () {
    	
    	if (Input.GetKey(KeyCode.A)) {
    		move = -1;
    		sRenderer.flipX = true;
    	} else if (Input.GetKey(KeyCode.D)) {
    		move = 1;
    		sRenderer.flipX = false;
    	} else if (Input.GetKey(KeyCode.W)) {
    		if (!jumping) {
    			rBody.AddForce(new Vector2(0, 1 * speed), ForceMode2D.Impulse);
    			jumping = true;
    			speed = speed / 2;
    		}
    	} else {
    		move = 0;
    	}
    	
    	Vector3 moveVector = transform.position;
    	
    	rBody.AddForce(new Vector2(move * speed, 0), ForceMode2D.Force);
    	
    	anim.SetFloat("Speed", move);
		/*
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        if(Input.GetKeyDown(KeyCode.Space) && stateInfo.nameHash == runStateHash)
        {
            anim.SetTrigger (jumpHash);
        }
        */
    }
    
    void OnCollisionEnter2D(Collision2D col) {
    	if (jumping) {
    		jumping = false;
    		speed = speed * 2;
    	}
    }
}