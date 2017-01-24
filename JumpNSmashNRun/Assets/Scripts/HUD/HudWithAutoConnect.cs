using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudWithAutoConnect : MonoBehaviour
{

	public float startingHealth = 100.0f;
	public float currentHealth;
	[SerializeField] public Slider healthSlider;
	[SerializeField] public Image damageImage;
	[SerializeField] public Image pointsImage;
	[SerializeField] public Image itemImage;
	[SerializeField] public Text pointsText;

    [SerializeField]
    public Image m_PunchCD;
    [SerializeField]
    public Image m_KickCD;

	public float flashSpeed = 5f;
	public Color healthFlashColor = new Color (0.867f, 0f, 0f, 0.1f);
	public Color pointsflashColor = new Color (0.945f, 0.784f, 0.051f, 0.1f);

	private bool isDead;
	private bool damaged;
	private bool changedScore;
	private float score;

	// Use this for initialization
	void Start ()
	{
		if (this.name == "Player1")
		{
			this.healthSlider = GameObject.Find ("HealthUI/P1/HealthSliderP1").GetComponent<Slider> ();
			this.damageImage = GameObject.Find ("HealthUI/P1/DmgImageP1").GetComponent<Image> ();
			this.pointsImage = GameObject.Find ("PointsItemUI/P1/PointsImageP1").GetComponent<Image> ();
			this.itemImage = GameObject.Find ("PointsItemUI/P1/ItemImageP1").GetComponent<Image> ();
			this.pointsText = GameObject.Find ("PointsItemUI/P1/TextPointsP1").GetComponent<Text> ();

            m_PunchCD = GameObject.Find("PointsItemUI/P1/PunchIcon").GetComponent<Image>();
            m_KickCD = GameObject.Find("PointsItemUI/P1/KickIcon").GetComponent<Image>();

        } else if (this.name == "Player2")
		{
			this.healthSlider = GameObject.Find ("HealthUI/P2/HealthSliderP2").GetComponent<Slider> ();
			this.damageImage = GameObject.Find ("HealthUI/P2/DmgImageP2").GetComponent<Image> ();
			this.pointsImage = GameObject.Find ("PointsItemUI/P2/PointsImageP2").GetComponent<Image> ();
			this.itemImage = GameObject.Find ("PointsItemUI/P2/ItemImageP2").GetComponent<Image> ();
            this.pointsText = GameObject.Find("PointsItemUI/P2/TextPointsP2").GetComponent<Text>();

            m_PunchCD = GameObject.Find("PointsItemUI/P2/PunchIcon").GetComponent<Image>();
            m_KickCD = GameObject.Find("PointsItemUI/P2/KickIcon").GetComponent<Image>();
        } else if (this.name == "Player3")
		{
			this.healthSlider = GameObject.Find ("HealthUI/P3/HealthSliderP3").GetComponent<Slider> ();
			this.damageImage = GameObject.Find ("HealthUI/P3/DmgImageP3").GetComponent<Image> ();
			this.pointsImage = GameObject.Find ("PointsItemUI/P3/PointsImageP3").GetComponent<Image> ();
			this.itemImage = GameObject.Find ("PointsItemUI/P3/ItemImageP3").GetComponent<Image> ();
            this.pointsText = GameObject.Find("PointsItemUI/P3/TextPointsP3").GetComponent<Text>();

            m_PunchCD = GameObject.Find("PointsItemUI/P3/PunchIcon").GetComponent<Image>();
            m_KickCD = GameObject.Find("PointsItemUI/P3/KickIcon").GetComponent<Image>();
        } else if (this.name == "Player4")
		{
			this.healthSlider = GameObject.Find ("HealthUI/P4/HealthSliderP4").GetComponent<Slider> ();
			this.damageImage = GameObject.Find ("HealthUI/P4/DmgImageP4").GetComponent<Image> ();
			this.pointsImage = GameObject.Find ("PointsItemUI/P4/PointsImageP4").GetComponent<Image> ();
			this.itemImage = GameObject.Find ("PointsItemUI/P4/ItemImageP4").GetComponent<Image> ();
            this.pointsText = GameObject.Find("PointsItemUI/P4/TextPointsP4").GetComponent<Text>();

            m_PunchCD = GameObject.Find("PointsItemUI/P4/PunchIcon").GetComponent<Image>();
            m_KickCD = GameObject.Find("PointsItemUI/P4/KickIcon").GetComponent<Image>();
        } else
		{
			gameObject.AddComponent<DebugText> ().SetDebugText ("Invalid player name.");
			Debug.LogError ("Invalid player name. Player name must be have the form: Player1 / Player 2 / Player 3 / Player 4.", this);
		}



        SetTextAndImageStatus(true);
        

        SetHealthUIStatus(false);
        SetGeneralUIStatus(true);

        //Player health
        currentHealth = startingHealth;
		score = 0;
		SetScoreText (score);
	}
	
	// Update is called once per frame
	void Update ()
	{
        

        if (null != damageImage)
		{
			// DamageImage
			if (damaged)
			{
				damageImage.color = healthFlashColor;
			} else
			{
				damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
			}
			damaged = false;
		} 
		if (null != pointsImage)
		{
			//PointsImage
			if (changedScore)
			{
				SetScoreText (score);
				pointsImage.color = pointsflashColor;
			} else
			{
				pointsImage.color = Color.Lerp (pointsImage.color, Color.clear, flashSpeed * Time.deltaTime);
			}
			changedScore = false;
		}
	}

	// UI health handling
	public void TakeDamage (float amount)
	{
		damaged = true;
		currentHealth -= amount;

		if (healthSlider){
			healthSlider.value = currentHealth;
		}
	}

    public void GainHealth(float amount)
    {
		if(amount < 0f){
			damaged = true;
		}
        currentHealth += amount;
        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }
    }

    public void SetHealth(float amount)
    {
        currentHealth = amount;
        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }
    }

    public float GetStartingHealth()
    {
        return startingHealth;
    }

	public void SetScoreText (float s)
	{
		if (pointsText){
			pointsText.text = "" + s;
		}
	}

	public float GetScore ()
	{
		return this.score;
	}

	public void SetScore(float newScore){
		this.score = newScore;
		changedScore = true;
	}

	public void SetScoreTextColor(Color color){
		this.pointsText.color = color;
	}

	public void AddToScore(float value){
		this.score += value;
		changedScore = true;
	}

	public void SubstractFromScore(float value){
		this.score -= value;
		changedScore = true;
	}

	public Sprite ItemSprite{
		get{
			return this.itemImage.sprite;
		}
		set{
			if(null != this.itemImage){
				this.itemImage.sprite = value;
			}
		}
	}

    public void SetHealthUIStatus(bool active)
    {
        healthSlider.gameObject.SetActive(active);
        damageImage.gameObject.SetActive(active);
    }

    public void SetGeneralUIStatus(bool active)
    {
        this.pointsImage.gameObject.SetActive(active);
        this.itemImage.gameObject.SetActive(active);
        this.pointsText.gameObject.SetActive(active);

        this.m_KickCD.gameObject.SetActive(active);
        this.m_PunchCD.gameObject.SetActive(active);
    }

    public void SetTextAndImageStatus(bool active)
    {
        GameObject hud = GameObject.Find("HUD");
        if (null != hud)
        {
            Transform playerTextTransform = hud.transform.Find(this.name + "Text");
            if (null != playerTextTransform)
                playerTextTransform.gameObject.SetActive(active);
        }
    }

    public void SetKickCD(float zeroToOne)
    {
        if(null != m_KickCD)
        {
            m_KickCD.fillAmount = zeroToOne;
        } 
    }

    public void SetPunchCD(float zeroToOne)
    {
        if(null != m_PunchCD)
        {
            m_PunchCD.fillAmount = zeroToOne;
        }
    }
}

