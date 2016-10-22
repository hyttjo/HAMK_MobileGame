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
    public Color32 freezeColor = new Color32(45, 170, 245, 255);
    public float freezeTime = 1f;
    public float freezeSpeed = 0.25f;
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

    void OnCollisionEnter2D(Collision2D col) {
        HandleDamage(col.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col) {
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
                DamageByIce();
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
        PushBack(col, gameObject);
        StartCoroutine(ShowFlashDamage(damageColor, damageShowDuration));
        health -= damageFire;
    }

    void DamageByIce() {
        StartCoroutine(ChangeSpeed(freezeSpeed, freezeTime));
        StartCoroutine(ShowFlashDamage(freezeColor, freezeTime));
        health -= damageIce;
    }

    void DamageByBite(GameObject col) {
        PushBack(col, gameObject);
        StartCoroutine(ShowFlashDamage(damageColor, damageShowDuration));
        health -= damageBite;
    }

    void DamageByCrush(GameObject col) {
        PushBack(gameObject, col);
        health -= damageCrush;
    }

    IEnumerator ChangeSpeed(float speedMultiplier, float duration) {
        MovementControl mControl = gameObject.GetComponentInChildren<MovementControl>();
        if (mControl != null) {
            mControl.speed *= speedMultiplier;
            yield return new WaitForSeconds(duration);
            mControl.speed /= speedMultiplier;
        }
    } 

    void PushBack(GameObject pusher, GameObject target) {
        Rigidbody2D rBodyTarget = target.GetComponentInParent<Rigidbody2D>();

        if (rBodyTarget != null) {
            Vector2 colPosition = pusher.transform.position;
            Vector2 position = target.transform.position;
            Vector2 direction = -(colPosition - position).normalized;
            rBodyTarget.velocity = direction * pushBackForce;
        }
    }

    IEnumerator ShowFlashDamage(Color32 damageColor, float duration) {
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
            AudioControl.EnemyDeath(null); //Toistaa äänen
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
