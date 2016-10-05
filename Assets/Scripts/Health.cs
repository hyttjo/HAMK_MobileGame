using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    public static event OnScoreDelegate OnEnemyKilled;

    public GameObject _gameObject;
    public GameObject deathEffect;
    public int health = 100;
    public int maxHealth = 100;
    public int healFromHeart = 34;
    public bool immuneToFire = false;
    public int damageFire = 34;
    public bool immuneToIce = false;
    public int damageIce = 100;
    public bool immuneToBite = false;
    public int damageBite = 34;
    public bool immuneToCrush = false;
    public int damageCrush = 100;
    public float damageShowDuration = 0.25f;
    public Color32 damageColor = new Color32(255, 128, 128, 255);
    public float pushBackForce = 10f;
    private Rigidbody2D rBody;
    private SpriteRenderer sRenderer;

    void Start() {
	    if (_gameObject == null) {
            _gameObject = gameObject;
        }

        if (gameObject.tag == "Player") {
            PickupControl.OnHeartCollected += GainHealth;
        }

        rBody = GetComponent<Rigidbody2D>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
    }
	
	void Update() {
	    if (health <= 0) {
            Destroy();
        }

        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        HandleDamage(col.gameObject);
    }

    void OnTriggerStay2D(Collider2D col) {
        HandleDamage(col.gameObject);
    }

    void HandleDamage(GameObject col) {
        string colliderTag = col.tag;

        if (colliderTag == "DamageTypePit") {
            DeathByPit();
        } else if (colliderTag == "DamageTypeFire") {
            if (!immuneToFire) {
                DamageByFire(col);
            }
        } else if (colliderTag == "DamageTypeIce") {
            if (!immuneToIce) {
                DamageByIce(col);
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

    void DamageByFire(GameObject col) {
        PushBack(col);
        StartCoroutine(ShowFlashDamage(damageShowDuration));
        health -= damageFire;
    }

    void DamageByIce(GameObject col) {
        Debug.Log("Hit healt");
        StartCoroutine(ShowFlashDamage(damageShowDuration));
        health -= damageIce;
    }

    void DamageByBite(GameObject col) {
        PushBack(col);
        StartCoroutine(ShowFlashDamage(damageShowDuration));
        health -= damageBite;
    }

    void DamageByCrush(GameObject col) {
        health -= damageCrush;
    }

    void PushBack(GameObject col) {
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

    public void GainHealth(GameObject e) {
        health += healFromHeart;
    }

    void Destroy() {
        if (gameObject.tag == "Enemy") {
            OnEnemyKilled();
        }

        if (rBody != null) {
            rBody.isKinematic = true;
        }
        if (deathEffect != null) {
            Vector2 spawnPosition = (Vector2)transform.position + Vector2.up * 0.5f * transform.localScale.y;
            deathEffect = (GameObject)Instantiate(deathEffect, spawnPosition, Quaternion.identity);
            Destroy(deathEffect, 0.5f);
            this.enabled = false;
        }
        AIControl aiControl = gameObject.GetComponent<AIControl>();

        if (aiControl != null) {
            if (Random.Range(0, 100) <= aiControl.lootChance) {
                ContainerControl containerController = GetComponent<ContainerControl>();

                if (containerController != null && containerController.pickup != null) {
                    containerController.SpawnPickup(transform.position);
                }
            }
        }
        Destroy(_gameObject);
    }
}
