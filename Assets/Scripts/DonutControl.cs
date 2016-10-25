using UnityEngine;
using System.Collections;

public class DonutControl : MonoBehaviour
{

    /*
     * Tämä skripti ihan vain hoitaa sen että ne "donitsi-palikat" alkaa putoamaan, kun pelaaja kävelee niiden päällä"
     */

    public float timeUntilDrop = 2; // Sekuntien määrä, jonka pelaaja saa seistä palikalla ennen kuin se alkaa putoamaan
    private float timeLeft;
    private bool colHappened = false; // Onko pelaaja osunut palikkaan tämän syklin aikana
    float decayPercent = 0; // Kuinka monta prosenttia donut blokki on kulunut

    private Rigidbody2D rb;
    private SpriteRenderer sRenderer;
    private Sprite donutSprite;
    //private Texture donutTexture;

    Color oldColor;

    void Start()
    {

        timeLeft = timeUntilDrop; // Luetaan editorissa säädetty aika muuttujaan, jotta voidaan vähentää sitä menettämättä tietoa alkuperäisestä arvosta

        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        donutSprite = sRenderer.sprite;
        //donutTexture = donutSprite.texture;

        oldColor = sRenderer.color;
    }

    void Update()
    {

        decayPercent = Mathf.Min(0 + (timeUntilDrop - timeLeft) * 100, 100);


        AnimateColor2();
        AnimateShake();
    }

    void FixedUpdate()
    {
        if (decayPercent == 100)
        {
            rb.isKinematic = false;
            rb.gravityScale = 0.1f;
        }
        Debug.Log(decayPercent);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (true)
        {
            colHappened = true;
        }
    }

    void LateUpdate()
    {
        if (colHappened)
        {
            timeLeft -= Time.deltaTime;
            colHappened = false;
        }

        decayPercent = Mathf.Min(decayPercent, 100);
    }

    void AnimateColor()
    {
        if (decayPercent > 0)
        {
            Color newColor = new Vector4(oldColor.r / 100 * decayPercent, oldColor.g / 100 * decayPercent, oldColor.b / 100 * decayPercent, 1);
            sRenderer.color = newColor;
        }
    }

    void AnimateColor2()
    {
        if (decayPercent < 100)
        {
            Color newColor = new Color();
            newColor.a = 1;
            newColor.r = oldColor.r - (0.5f / 100 * decayPercent);
            newColor.g = oldColor.g - (0.5f / 100 * decayPercent);
            newColor.b = oldColor.b - (0.5f / 100 * decayPercent);
            sRenderer.color = newColor;
        }
    }

    void AnimateShake()
    {
        //
    }
}