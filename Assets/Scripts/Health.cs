using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    public GameObject _gameObject;
    public int health = 100;

	// Use this for initialization
	void Start () {
	    if (_gameObject == null) {
            _gameObject = gameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (health <= 0) {
            Destroy();
        }
	}

    void OnCollisionEnter2D(Collision2D col) {
        string colliderTag = col.gameObject.tag;

        if (colliderTag == "DamageTypePit") {
            DeathByPit();
        } else if (colliderTag == "DamageTypeFire") {
            DamageByFire();
        }
    }

    void DeathByPit() {
        Destroy();
    }

    void DamageByFire() {
        health -= 50;
    }

    void Destroy() {
        Rigidbody2D rBody = GetComponent<Rigidbody2D>();

        if (rBody != null) {
            rBody.isKinematic = true;
        }

        GameObject.Destroy(_gameObject);
    }
}
