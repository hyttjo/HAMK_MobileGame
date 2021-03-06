﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementControl))]
public class AIControl : MonoBehaviour {

    private GameObject player;
    private MovementControl mControl;
    private Rigidbody2D rBody;

    private Vector2 moveDirection;
    public Vector2[] path;

    public float shootingDistance = 10f;

    private int path_index = 0;
    private bool path_increment_up = true;

    public int lootChance = 33;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        mControl = GetComponent<MovementControl>();
        rBody = GetComponentInChildren<Rigidbody2D>();

        moveDirection = new Vector2(-1, 0);

        InvokeRepeating("FollowPath", 0, 0.5f);

        if (player != null && mControl != null) {
            InvokeRepeating("CheckShooting", 1f, 1 / mControl.shotsPerSecond);
        }
    }

    void FixedUpdate () {
        if (path != null && path.Length > 0) {
    		mControl.MoveTo(path[path_index]);
    	} else {
    		mControl.Idle();
    	}
    	
    	if (moveDirection.y > 0) {
    		mControl.Jump();
    	}
    }

    private void CheckShooting() {
        if (mControl.currentPower != null) {
            if (Vector2.Distance(player.transform.position, transform.position) < shootingDistance) {
                int facingDirToPlayer = (int)player.transform.position.x - (int)transform.position.x;
                if (facingDirToPlayer < 0) {
                    facingDirToPlayer = -1;
                } else {
                    facingDirToPlayer = 1;
                }

                if (mControl.GetFacingDir() == facingDirToPlayer) {
                    mControl.Shoot();
                }
            }
        }
    }

    private void FollowPath() {
        if (path != null && path.Length > 0) {
            Vector2 waypoint = path[path_index];
            Vector2 position = mControl.character.transform.position;

            if ((rBody.constraints & RigidbodyConstraints2D.FreezePositionX) != RigidbodyConstraints2D.None) {
                waypoint.x = position.x;
            }
            if ((rBody.constraints & RigidbodyConstraints2D.FreezePositionY) != RigidbodyConstraints2D.None) {
                waypoint.y = position.y;
            }

            float waypointDistance = Vector2.Distance(position, waypoint);
            
            if (waypointDistance < 1f) {
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