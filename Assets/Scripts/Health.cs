using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    public GameObject _gameObject;
    public int health = 100;
    private Rigidbody2D rBody;

    void Start () {
	    if (_gameObject == null) {
            _gameObject = gameObject;
        }

        rBody = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
	    if (health <= 0) {
            Destroy();
        }
	}

    void OnCollisionEnter2D(Collision2D col) {
        string colliderTag = col.gameObject.tag;

        if (colliderTag == "DamageTypePit") {
            DeathByPit(col);
        } else if (colliderTag == "DamageTypeFire") {
            DamageByFire(col);
        } else if (colliderTag == "DamageTypeBite") {
            DamageByBite(col);
        }
    }

    void DeathByPit(Collision2D col) {
        health = 0;
    }

    void DamageByFire(Collision2D col) {
        health -= 50;
    }

    void DamageByBite(Collision2D col) {
        Vector2 position = transform.position;
        Vector2 damagePoint = col.contacts[0].point;
        rBody.velocity -= (position - damagePoint);

        health -= 35;
    }

    void Destroy() {
        Rigidbody2D rBody = GetComponent<Rigidbody2D>();

        if (rBody != null) {
            rBody.isKinematic = true;
        }

        GameObject.Destroy(_gameObject);
    }
}
