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

        InvokeRepeating("CheckPlayerState", 1f, 1f);
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
        }
    }

    void CheckPlayerState() {
        if (mControl != null) {
            if (mControl.character == null) {
                GameOver gameOver = gameObject.GetComponent<GameOver>();

                if (gameOver != null) {
                    gameOver.enabled = true;
                } else {
                    gameOver = gameObject.AddComponent<GameOver>();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Finish") {
            LevelFinish levelFinish = gameObject.GetComponent<LevelFinish>();

            if (levelFinish != null) {
                levelFinish.enabled = true;
            } else {
                levelFinish = gameObject.AddComponent<LevelFinish>();
            }
        }
    }
}