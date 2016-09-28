using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterControl))]
public class AIControl : MonoBehaviour {

    private CharacterControl cControl;
    private Level level;

    private Vector2 moveDirection;

    public int lootChance = 33;
    public GameObject loot;

    void Start () {
        cControl = GetComponent<CharacterControl>();
        level = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();

        moveDirection = new Vector2(-1, 0);

        InvokeRepeating("CheckObstacles", 1f, 1f);
    }

    void FixedUpdate () {
        if (moveDirection.x < 0) {
    		cControl.MoveLeft();
    	} else if (moveDirection.x > 0) {
    		cControl.MoveRight();
    	} else {
    		cControl.Idle();
    	}
    	
    	if (moveDirection.y > 0) {
    		cControl.Jump();
    	}
    }

    void CheckObstacles() {
        Vector2 position = cControl.character.transform.position;
        Vector2 forward = position + moveDirection + Vector2.up * 0.5f;
        Vector2 forwardDown = position + moveDirection + Vector2.down * 0.5f;

        GameObject obstacle = level.GetGameObject(forwardDown);
        
        if (obstacle == null) {
            moveDirection *= -1;
        }
        
        obstacle = level.GetGameObject(forward);

        if (obstacle != null) {
            moveDirection *= -1;
        }
    }

    void OnDestroy(){
        if (Random.Range(0,100) <= lootChance) {
            if (loot != null) {
                PickupControl pickUpControl = loot.GetComponent<PickupControl>();

                if (pickUpControl != null) {
                    pickUpControl.SpawnPickup(transform.position);
                }
            }
        }
    }
}