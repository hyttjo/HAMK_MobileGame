using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject player;
	public GameObject[] background;
	private Material[] backgroundMat;
	public float offsetMultiplierX = 0.01f;
	public float offsetMultiplierY = 0.01f;
	public float offsetClampY = 0.05f;
    public float cameraHeight = 1f;
    public Rect cameraBounds = new Rect(7,3,1017,253);
    private Vector3 cameraPosition;
	
	void Start () {
		if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
		}

        if (background.Length > 0) {
			backgroundMat = new Material[background.Length];
			
			for(int i = 0; i < background.Length; i++) {
				backgroundMat[i] = background[i].GetComponent<MeshRenderer>().material;
			}
		}
	}
	
	void FixedUpdate () {
		if (player != null) {
            UpdateCameraPosition();
			
			if (background.Length > 0) {
                UpdateParallaxBackground();
			}
		}
	}

    void UpdateCameraPosition() {
        Vector3 playerPosition = player.transform.position;

        cameraPosition = new Vector3(playerPosition.x, playerPosition.y + cameraHeight, transform.position.z);

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
        Vector3 playerPosition = player.transform.position;

        for (int i = 0; i < background.Length; i++) {
            if (background[i] != null) {
                Vector3 backgroundPosition = new Vector3(cameraPosition.x, cameraPosition.y, 10 + background.Length - i);
                background[i].transform.position = backgroundPosition;

                float backgroundOffsetY = playerPosition.y * offsetMultiplierY * i;

                if (backgroundOffsetY > offsetClampY) {
                    backgroundOffsetY = offsetClampY;
                } else if (backgroundOffsetY < -offsetClampY) {
                    backgroundOffsetY = -offsetClampY;
                }

                Vector2 backgroundOffset = new Vector2(playerPosition.x * offsetMultiplierX * i, backgroundOffsetY);
                backgroundMat[i].mainTextureOffset = backgroundOffset;
            }
        }
    }
}