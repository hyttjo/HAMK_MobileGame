using UnityEngine;
using System.Collections;

public enum Transition { None, VerticalOut, HorizontalOut, BoxOut, CornerOut,
                         VerticalIn, HorizontalIn, BoxIn, CornerIn } 

public class CameraControl : MonoBehaviour {

	public GameObject target;
	public GameObject[] background;

    public float skySpeed = 50;
	private Material[] backgroundMat;
	public float offsetMultiplierX = 0.01f;
	public float offsetMultiplierY = 0.01f;
	public float offsetClampY = 0.05f;
    public float cameraHeight = 1f;

    public delegate void OnTransitionFinishDelegate();
    public static event OnTransitionFinishDelegate OnTransitionFinish;
    public Transition transition;
    public float transitionSpeed = 30f;
    private Texture2D mask;
    public Color maskColor = new Color(0,0,0,1);
    private float timer;

    public Rect cameraBounds = new Rect(8,8,1016,248);
    private Vector3 cameraPosition;
	
	void Start() {
		if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player");
		}

        GameObject gameObjectLevel = GameObject.FindGameObjectWithTag("Level");
        Level level = null;

        if (gameObjectLevel != null) {
            level = gameObjectLevel.GetComponent<Level>();
        }

        if (level != null) {
            cameraBounds.width = level.width;
            cameraBounds.height = level.height;
        }

        if (background.Length > 0) {
			backgroundMat = new Material[background.Length];
			
			for(int i = 0; i < background.Length; i++) {
				backgroundMat[i] = background[i].GetComponent<MeshRenderer>().material;
			}
		}

        mask = new Texture2D(1, 1);
        mask.SetPixel(0, 0, maskColor);
        mask.Apply();
	}
	
	void FixedUpdate() {
        UpdateCameraPosition();
			
		if (background.Length > 0) {
            UpdateParallaxBackground();
		}
	}

    void UpdateCameraPosition() {
        Vector3 targetPosition = Vector3.zero;

        if (target != null) {
            targetPosition = target.transform.position;
        }

        cameraPosition = new Vector3(targetPosition.x, targetPosition.y + cameraHeight, transform.position.z);

        if (cameraPosition.x < cameraBounds.x) {
            cameraPosition.x = cameraBounds.x;
        }
        if (cameraPosition.x > cameraBounds.width) {
            cameraPosition.x = cameraBounds.width;
        }
        if (cameraPosition.y < cameraBounds.y) {
            cameraPosition.y = cameraBounds.y;
        }
        if (cameraPosition.y > cameraBounds.height) {
            cameraPosition.y = cameraBounds.height;
        }

        gameObject.transform.position = Vector3.Slerp(transform.position, cameraPosition, 1f);
    }

    void UpdateParallaxBackground() {
        Vector3 targetPosition = Vector3.zero;

        if (target != null) {
            targetPosition = target.transform.position;
        }

        for (int i = 0; i < background.Length; i++) {
            if (background[i] != null) {
                Vector3 backgroundPosition = new Vector3(cameraPosition.x, cameraPosition.y, 10 + background.Length - i);
                background[i].transform.position = backgroundPosition;

                float backgroundOffsetY = targetPosition.y * offsetMultiplierY * i;

                if (backgroundOffsetY > offsetClampY) {
                    backgroundOffsetY = offsetClampY;
                } else if (backgroundOffsetY < -offsetClampY) {
                    backgroundOffsetY = -offsetClampY;
                }
                Vector2 backgroundOffset = new Vector2(targetPosition.x * offsetMultiplierX * i, backgroundOffsetY);

                if (i == background.Length - 1) {
                    backgroundOffset += new Vector2(Time.time / skySpeed, 0);
                }
                backgroundMat[i].mainTextureOffset = backgroundOffset;
            }
        }
    }

    void OnGUI() {
        if (transition != Transition.None) {
            timer += Time.deltaTime;
            float deltaTransition = timer * transitionSpeed * 10;

            float Upper_Left_x = 0;
            float Upper_Right_x = 0;
            float Upper_Left_y = 0;
            float Upper_Right_y = 0;
            float Lower_Left_x = 0;
            float Lower_Right_x = 0;
            float Lower_Left_y = 0;
            float Lower_Right_y = 0;

            switch (transition) {
                case Transition.HorizontalOut:
                    Upper_Left_x = Screen.width / 2 - deltaTransition;
                    Upper_Left_y = Screen.height;
                    Lower_Right_x = Screen.width / 2 + deltaTransition;
                    if (Upper_Left_x < 0) { transition = Transition.None; TransitionFinish(); }
                    break;

                case Transition.VerticalOut:
                    Upper_Left_x = Screen.width;
                    Upper_Left_y = Screen.height / 2 - deltaTransition;
                    Lower_Right_y = Screen.height / 2 + deltaTransition;
                    if (Upper_Left_y < 0) { transition = Transition.None; TransitionFinish(); }
                    break;

                case Transition.BoxOut:
                    Upper_Left_x = Screen.width / 2 - deltaTransition;
                    Lower_Left_x = Screen.width;
                    Lower_Right_x = Screen.width / 2 + deltaTransition;
                    Upper_Left_y = Screen.height;
                    Upper_Right_y = Screen.height / 2 - deltaTransition;
                    Lower_Left_y = Screen.height / 2 + deltaTransition;
                    if (Upper_Left_x < 0) { transition = Transition.None; TransitionFinish(); }
                    break;

                case Transition.CornerOut:
                    Upper_Left_x = Screen.width / 2 - deltaTransition;
                    Upper_Right_x = Screen.width / 2 + deltaTransition;
                    Lower_Left_x = Screen.width / 2 - deltaTransition;
                    Lower_Right_x = Screen.width / 2 + deltaTransition;
                    Upper_Left_y = Screen.height / 2 - deltaTransition;
                    Upper_Right_y = Screen.height / 2 - deltaTransition;
                    Lower_Left_y = Screen.height / 2 + deltaTransition;
                    Lower_Right_y = Screen.height / 2 + deltaTransition;
                    if (Upper_Left_x < 0) { transition = Transition.None; TransitionFinish(); }
                    break;

                case Transition.HorizontalIn:
                    Upper_Left_x += deltaTransition;
                    Lower_Right_x = Screen.width - deltaTransition;
                    Upper_Left_y = Screen.height;
                    if (Upper_Left_x > Screen.width / 2) { TransitionFinish(); }
                    break;

                case Transition.VerticalIn:
                    Upper_Left_x = Screen.width;
                    Upper_Left_y += deltaTransition;
                    Lower_Right_y = Screen.height - deltaTransition;
                    if (Upper_Left_y > Screen.height / 2) { TransitionFinish(); }
                    break;

                case Transition.BoxIn:
                    Upper_Left_x = Screen.width;
                    Upper_Right_x = Screen.width - deltaTransition;
                    Lower_Left_x += deltaTransition;
                    Upper_Left_y += deltaTransition;
                    Upper_Right_y = Screen.height;
                    Lower_Right_y = Screen.height - deltaTransition;
                    if (Upper_Left_y > Screen.height / 2) { TransitionFinish(); }
                    break;

                case Transition.CornerIn:
                    Upper_Left_x += deltaTransition;
                    Upper_Right_x = Screen.width - deltaTransition;
                    Lower_Left_x += deltaTransition;
                    Lower_Right_x = Screen.width - deltaTransition;
                    Upper_Left_y += deltaTransition;
                    Upper_Right_y += deltaTransition;
                    Lower_Left_y = Screen.height - deltaTransition;
                    Lower_Right_y = Screen.height - deltaTransition;
                    if (Upper_Left_x > Screen.width / 2) { TransitionFinish(); }
                    break;
            }

            GUI.DrawTexture(new Rect(0, 0, Upper_Left_x, Upper_Left_y), mask);
            GUI.DrawTexture(new Rect(Upper_Right_x, 0, Screen.width, Upper_Right_y), mask);
            GUI.DrawTexture(new Rect(Lower_Right_x, Lower_Right_y, Screen.width, Screen.height), mask);
            GUI.DrawTexture(new Rect(0, Lower_Left_y, Lower_Left_x, Screen.height), mask);
        } else {
            timer = 0;
        }
    }

    private void TransitionFinish() {
        if (OnTransitionFinish != null) { 
            OnTransitionFinish();
        }
    }
}