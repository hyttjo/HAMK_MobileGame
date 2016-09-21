using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject player;
	public GameObject background;
	private Rigidbody2D playerRb;
	private Material backgroundMat;
	private float cameraHeight;
	public float cameraHeightMultiplier = 2;
	public float offsetMultiplier = 0.01f;
	
	void Start () {
		if (player != null) {
			playerRb = player.GetComponent<Rigidbody2D>();
		}
		
		if (background != null) {
			backgroundMat = background.GetComponent<MeshRenderer>().material;
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
			
			if (background != null) {
				Vector3 backgroundPosition = new Vector3(playerPosition.x, background.transform.position.y, playerPosition.z);
				background.transform.position = backgroundPosition;
				
				Vector2 backgroundOffset = new Vector2(playerPosition.x * offsetMultiplier, playerPosition.z * offsetMultiplier);
				backgroundMat.mainTextureOffset = backgroundOffset;
			}
		}
	}
}