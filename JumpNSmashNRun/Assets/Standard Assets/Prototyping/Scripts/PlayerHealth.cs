using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public Image pointsImage;
    public float flashSpeed = 5f;
    public Color healthFlashColor = new Color(0.867f, 0f, 0f, 0.1f);
    public Color pointsflashColor = new Color(0.945f, 0.784f, 0.051f, 0.1f);

    private Animator anim;
    private bool isDead;
    private bool damaged;
    private bool gotPoints;

	void Start () 
    {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
	}
	
	void Update ()
    {
        // DamageImage
	    if(damaged)
        {
            damageImage.color = healthFlashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        //PointsImage
        if(gotPoints)
        {
            pointsImage.color = pointsflashColor;
        }
        else
        {
            pointsImage.color = Color.Lerp(pointsImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        gotPoints = false;
	}

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;

        anim.SetTrigger("Death");

        // player movement disabled and respawn;
    }
}
