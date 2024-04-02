using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class PlayerHealth : MonoBehaviour
{
    #region Public Variables;
    public static PlayerHealth instance;
    public float health;
    public float maxHealth;
    public Image healthImage;
    //public GameObject gameOverImage;
    public float inmunerableTime;
    public float knockBackX;
    public float knockBackY;
    public bool isInmune;
    #endregion

    #region Private Variables;
    private Blink material;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator anim;
    
    #endregion

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //gameOverImage.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        material = GetComponent<Blink>();
        health = maxHealth;
        material.original = sprite.material;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthImage.fillAmount = health / maxHealth;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isInmune)
        {
            health -= collision.GetComponentInParent<Enemy>().damageToGive;
            StartCoroutine(Inmunity());
            
            if (collision.transform.position.x > transform.position.x)
            {
                rb.AddForce(new Vector2(-knockBackX, knockBackY), ForceMode2D.Force);
            }
            else
            {
                rb.AddForce(new Vector2(knockBackX, knockBackY), ForceMode2D.Force);
            }
            
            if (health <= 0)
            {
                //StartCoroutine(Morirse());
                Debug.Log("Muelto");
            }
        }
    }
    IEnumerator Inmunity()
    {
        isInmune = true;
        sprite.material = material.parpadeo;
        //AudioMannager.instance.PlayAudio(AudioMannager.instance.bump);
        yield return new WaitForSeconds(inmunerableTime);
        sprite.material = material.original;
        isInmune = false;
    }

    IEnumerator Morirse()
    {
        anim.SetBool("Muerte", true);
        //AudioMannager.instance.PlayAudio(AudioMannager.instance.risa);
        yield return new WaitForSeconds(1f);

        //AudioMannager.instance.backgroundFight.Stop();
        //AudioMannager.instance.background.Stop();
        Time.timeScale = 0;
        //gameOverImage.SetActive(true);

    }
}
