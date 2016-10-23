using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour
{

    /*
     * Tämä skripti hoitaa piikin putoamisen, jos pelaaja kulkee alta
     */

    Rigidbody2D rb;
    public GameObject destroyEffect;
    bool falling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        HandleHit(col.gameObject);
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            rb.gravityScale = 1;
            falling = true;
        }
    }

    void HandleHit(GameObject target)
    {
        Destroy(transform.position);
    }

    void Destroy(Vector3 effectPosition)
    {
        if (destroyEffect != null)
        {
            destroyEffect = (GameObject)Instantiate(destroyEffect, effectPosition, Quaternion.identity);
            destroyEffect.transform.localScale *= 0.75f;
            Destroy(destroyEffect, 0.5f);
        }
        Destroy(gameObject);
    }
}