using UnityEngine;
using UnityStandardAssets;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

[RequireComponent(typeof(MovementControl))]
public class PlayerControl_Mobile : MonoBehaviour
{

    GameManager GM;

    private MovementControl mControl;

    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.W;
    public KeyCode shoot = KeyCode.Space;

    void Start()
    {
        GM = FindObjectOfType<GameManager>();

        mControl = GetComponent<MovementControl>();

        InvokeRepeating("CheckPlayerState", 1f, 1f);
    }

    void FixedUpdate()
    {
        if (mControl != null)
        {
            if (CrossPlatformInputManager.GetAxisRaw("Horizontal") < 0)
            {
                mControl.MoveLeft();
            }
            else if (CrossPlatformInputManager.GetAxisRaw("Horizontal") > 0)
            {
                mControl.MoveRight();
            }
            else
            {
                mControl.Idle();
            }

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                mControl.Jump();
            }

            if (CrossPlatformInputManager.GetButtonDown("Shoot"))
            {
                mControl.Shoot();
            }
        }
    }

    void CheckPlayerState()
    {
        if (mControl != null)
        {
            if (mControl.character == null)
            {
                if (GM != null)
                {
                    if (GM.gameState == GameState.Playing)
                    {
                        GM.SetGameState(GameState.PlayerDied);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Finish")
        {
            if (GM != null)
            {
                if (GM.gameState == GameState.Playing)
                {
                    GM.SetGameState(GameState.LevelFinished);
                }
            }
        }
    }
}