using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementControl))]
public class PlayerControl : MonoBehaviour {

    private MovementControl mControl;
    
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.W;
    public KeyCode shoot = KeyCode.Space;

    void Start () {
        mControl = GetComponent<MovementControl>();
    }

    void FixedUpdate () {
        if (mControl != null) {
    	    if (Input.GetKey(moveLeft)) {
    		    mControl.MoveLeft();
    	    } else if (Input.GetKey(moveRight)) {
    		    mControl.MoveRight();
    	    } else {
    		    mControl.Idle();
    	    }
    	
    	    if (Input.GetKey(jump)) {
    		    mControl.Jump();
    	    }

            if (Input.GetKey(shoot)) {
                mControl.Shoot();
            }

            CheckPlayerState();
        }
    }

    void CheckPlayerState() {
        if (mControl.character == null) {
            gameObject.AddComponent<GameOver>();
        }
    }
}