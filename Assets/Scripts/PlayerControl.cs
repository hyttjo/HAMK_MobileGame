using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(MovementControl))]
public class PlayerControl : MonoBehaviour {

    GameManager GM;

    private MovementControl mControl;
    
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.W;
    public KeyCode shoot = KeyCode.Space;

    void Start () {
        GM = FindObjectOfType<GameManager>();

        mControl = GetComponent<MovementControl>();

        InvokeRepeating("CheckPlayerState", 1f, 1f);
    }

    void FixedUpdate () {
        if (mControl != null) {
            if (CrossPlatformInputManager.GetButton("Left") || Input.GetKey(moveLeft)) {
                mControl.MoveLeft();
            } else if (CrossPlatformInputManager.GetButton("Right") || Input.GetKey(moveRight)) {
                mControl.MoveRight();
            } else {
                mControl.Idle();
            }

            if (CrossPlatformInputManager.GetButton("Jump") || Input.GetKey(jump)) {
                mControl.Jump();
            }

            if (CrossPlatformInputManager.GetButton("Shoot") || Input.GetKey(shoot)) {
                mControl.Shoot();
            }
        }
    }

    void CheckPlayerState() {
        if (mControl != null) {
            if (mControl.character == null) {
                if (GM != null) {
                    if (GM.gameState == GameState.Playing) {
                        GM.SetGameState(GameState.PlayerDied);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Finish") {
            if (GM != null) {
                if (GM.gameState == GameState.Playing) {
                    GM.SetGameState(GameState.LevelFinished);
                }
            }
        }
    }
}