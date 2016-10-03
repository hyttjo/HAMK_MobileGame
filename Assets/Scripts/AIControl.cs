using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterControl))]
public class AIControl : MonoBehaviour {

    private CharacterControl cControl;

    private Vector2 moveDirection;
    public Vector2[] path;

    private int path_index = 0;
    private bool path_increment_up = true;

    public int lootChance = 33;

    void Start () {
        cControl = GetComponent<CharacterControl>();

        moveDirection = new Vector2(-1, 0);

        InvokeRepeating("FollowPath", 0, 0.5f);
    }

    void FixedUpdate () {
        if (path != null && path.Length > 0) {
    		cControl.MoveTo(path[path_index]);
    	} else {
    		cControl.Idle();
    	}
    	
    	if (moveDirection.y > 0) {
    		cControl.Jump();
    	}
    }

    void FollowPath() {
        if (path != null && path.Length > 0) {
            Vector2 waypoint = path[path_index];
            Vector2 position = cControl.character.transform.position;
            float waypointDistance = Vector2.Distance(position, waypoint);

            if (waypointDistance < 1.5f) {
                if (path_index == path.Length - 1) {
                    path_increment_up = false;
                } else if (path_index == 0) {
                    path_increment_up = true;
                }

                if (path_increment_up) {
                    path_index++;
                } else {
                    path_index--;
                }
            }
        }
    }
}