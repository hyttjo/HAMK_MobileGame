using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    public GameObject _gameObject;
    public int health = 100;

    void Start () {
	    if (_gameObject == null) {
            _gameObject = gameObject;
        }
	}
	
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
        health = 0;
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
