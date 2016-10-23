using UnityEngine;
using System.Collections;

public class BoostObject : MonoBehaviour {

    private SpriteRenderer sRenderer;
    public Sprite defaultSprite;
    public Sprite boostSprite;
    public float animationDuration = 0.1f;
    public Vector2 boostDirection = Vector2.up;
    public float boostForce = 10f;

	void Start () {
	    sRenderer = GetComponentInChildren<SpriteRenderer>();
	}
	
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            Vector2 hitDirection = Misc.GetHitDirection(col.contacts[0].normal);

            if (hitDirection == boostDirection * -1) {
                Rigidbody2D rBody = col.gameObject.GetComponent<Rigidbody2D>();
               if (rBody != null) {
                    StartCoroutine(Boost(rBody, animationDuration));
                    AudioControl.SpringJump(null); //Toistaa äänen
                }
            }
        }
    }

    IEnumerator Boost(Rigidbody2D rBody, float duration) {
        if (sRenderer != null) {
            if (boostSprite != null) {
                sRenderer.sprite = boostSprite;
            }
            yield return new WaitForSeconds(duration);

            if (defaultSprite != null) {
                sRenderer.sprite = defaultSprite;
            }
            rBody.AddForce(boostDirection * boostForce, ForceMode2D.Impulse);
        }
    }
}
