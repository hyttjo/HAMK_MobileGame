using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject player;
	public GameObject[] background;
	private Rigidbody2D playerRb;
	private Material[] backgroundMat;
	private float cameraHeight;
	public float cameraHeightMultiplier = 2;
	public float offsetMultiplierX = 0.01f;
	public float offsetMultiplierY = 0.01f;
	public float offsetClampY = 0.05f;
	
	void Start () {
		if (player != null) {
			playerRb = player.GetComponent<Rigidbody2D>();
		}
		
		if (background.Length > 0) {
			backgroundMat = new Material[background.Length];
			
			for(int i = 0; i < background.Length; i++) {
				backgroundMat[i] = background[i].GetComponent<MeshRenderer>().material;
			}
		}
		
		cameraHeight = transform.position.y;
	}
	
	void FixedUpdate () {
		if (player != null) {
			Vector3 playerPosition = player.transform.position;
			float playerVelocity = playerRb.velocity.magnitude * cameraHeightMultiplier;
			
			Vector3 cameraPosition = new Vector3(playerPosition.x, playerPosition.y + cameraHeight, transform.position.z);
			gameObject.transform.position = Vector3.Slerp(transform.position, cameraPosition, 1f);	
			
			Camera.main.orthographicSize = 5 + playerVelocity;
			
			if (background.Length > 0) {
				for(int i = 0; i < background.Length; i++) {
					if (background[i] != null) {
						Vector3 backgroundPosition = new Vector3(cameraPosition.x, cameraPosition.y, 10 + background.Length - i);
						background[i].transform.position = backgroundPosition;
						
						float backgroundOffsetY = playerPosition.y * offsetMultiplierY * i;
						
						if (backgroundOffsetY > offsetClampY) {
							backgroundOffsetY = offsetClampY;
						} else if (backgroundOffsetY < -offsetClampY) {
							backgroundOffsetY = -offsetClampY;
						}//
						
						Vector2 backgroundOffset = new Vector2(playerPosition.x * offsetMultiplierX * i, backgroundOffsetY);
						backgroundMat[i].mainTextureOffset = backgroundOffset;
					}
				}
			}
		}
	}
}