using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

    private CharacterControl cControl;
    private Rigidbody2D rb;

    public float speedX;
    public float speedY;
    public float decayTime;
    private float aliveTimer;

	// Use this for initialization
	void Start () {


        rb = GetComponent<Rigidbody2D>();
        cControl = GameObject.Find("Player").GetComponent<CharacterControl>();

        rb.AddForce(transform.up * speedY, ForceMode2D.Impulse);
        rb.AddForce(transform.right * speedX * cControl.GetFacingDir(), ForceMode2D.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
        CheckDecay();
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    void CheckDecay()
    {
        aliveTimer += Time.deltaTime;
        if (aliveTimer > decayTime)
        {
            Destroy(gameObject);
        }
    }
}
