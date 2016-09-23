﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    public GameObject _gameObject;
    public int health = 100;
    public bool immuneToFire = false;
    public int damageFire = 34;
    public bool immuneToBite = false;
    public int damageBite = 34;
    public bool immuneToCrush = false;
    public int damageCrush = 100;
    public float damageShowDuration = 0.25f;
    public Color32 damageColor = new Color32(255, 128, 128, 255);
    public float pushBackForce = 10f;
    private Rigidbody2D rBody;
    private SpriteRenderer sRenderer;

    void Start () {
	    if (_gameObject == null) {
            _gameObject = gameObject;
        }

        rBody = GetComponent<Rigidbody2D>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
    }
	
	void Update () {
	    if (health <= 0) {
            Destroy();
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        string colliderTag = col.gameObject.tag;

        if (colliderTag == "DamageTypePit") {
            DeathByPit();
        } else if (colliderTag == "DamageTypeFire") {
            if (!immuneToFire) {
                DamageByFire(col);
            }
        } else if (colliderTag == "DamageTypeBite") {
            if (!immuneToBite) {
                DamageByBite(col);
            }
        } else if (colliderTag == "DamageTypeCrush") {
            if (!immuneToCrush) {
                DamageByCrush(col);
            }
        }
    }

    void DeathByPit() {
        health = 0;
    }

    void DamageByFire(Collider2D col) {
        health -= damageFire;
    }

    void DamageByBite(Collider2D col) {
        PushBack(col);
        StartCoroutine(ShowFlashDamage(damageShowDuration));
        health -= damageBite;
    }

    void DamageByCrush(Collider2D col) {
        health -= damageCrush;
    }

    void PushBack(Collider2D col) {
        if (rBody != null) {
            Vector2 colPosition = col.transform.position;
            Vector2 position = transform.position;
            Vector2 direction = -(colPosition - position).normalized;
            rBody.velocity = direction * pushBackForce;
        }
    }

    IEnumerator ShowFlashDamage(float duration) {
        ChangeColor(damageColor);
        yield return new WaitForSeconds(duration);
        ChangeColor(Color.white);
    }

    void ChangeColor(Color32 color) {
        if (sRenderer != null) {
            sRenderer.color = color;
        }
    }

    void Destroy() {
        if (rBody != null) {
            rBody.isKinematic = true;
        }

        GameObject.Destroy(_gameObject);
    }
}
