using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterControl))]
public class PlayerControl : MonoBehaviour {

    private CharacterControl cControl;
    
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.W;
    public KeyCode jumpAlt = KeyCode.Space;

    void Start () {
        cControl = GetComponent<CharacterControl>();
    }

    void FixedUpdate () {
    	if (Input.GetKey(moveLeft)) {
    		cControl.MoveLeft();
    	} else if (Input.GetKey(moveRight)) {
    		cControl.MoveRight();
    	} else {
    		cControl.Idle();
    	}
    	
    	if (Input.GetKey(jump) || Input.GetKey(jumpAlt)) {
    		cControl.Jump();
    	}
    }
}