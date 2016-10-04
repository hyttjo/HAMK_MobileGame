using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject target;
	public GameObject[] background;
    public float skySpeed = 50;
	private Material[] backgroundMat;
	public float offsetMultiplierX = 0.01f;
	public float offsetMultiplierY = 0.01f;
	public float offsetClampY = 0.05f;
    public float cameraHeight = 1f;
    public Rect cameraBounds = new Rect(8,8,1016,248);
    private Vector3 cameraPosition;
	
	void Start () {
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
	}
	
	void FixedUpdate () {
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
}