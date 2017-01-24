using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
    //Stores the score for each player
	private float m_Score;
	private HudWithAutoConnect m_HUD;

	private ScreenSection m_CurrentScreenSection;

	[SerializeField] private Color m_LowPointColor = Color.red;
	[SerializeField] private Color m_MidPointColor = Color.yellow;
	[SerializeField] private Color m_HighPointColor = Color.green;

	public ScreenSection CurrentScreenSection{
		get{
			return m_CurrentScreenSection;
		}
		set{
			m_CurrentScreenSection = value;
		}
	}

	public enum ScreenSection{
		LOW,
		MIDDLE,
		HIGH
	};

	void Start () {
        m_Score = 0.0f;
		m_HUD = GetComponentInParent<HudWithAutoConnect> ();
		m_CurrentScreenSection = ScreenSection.MIDDLE;

		if(!m_HUD){
			Debug.LogWarning ("Couldn't find HudWithAutoConnect Script in Score script");
		}
	}

	void Update () {
		switch (m_CurrentScreenSection) {
		case ScreenSection.LOW:
			m_HUD.SetScoreTextColor (m_LowPointColor);
			break;
		case ScreenSection.MIDDLE:
			m_HUD.SetScoreTextColor (m_MidPointColor);
			break;
		case ScreenSection.HIGH:
			m_HUD.SetScoreTextColor (m_HighPointColor);
			break;
		}
	}

    public void AddToScore(float score)
    {
		this.m_Score += score;
		if(m_HUD){
			m_HUD.AddToScore (score);
		}
    }

    public void SubstractFromScore(float score)
    {
		this.m_Score -= score;
		if(m_HUD){
			m_HUD.SubstractFromScore (score);
		}
    }

    public float GetScore()
    {
        return m_Score;
    }

	public void SetScore(float score){
		this.m_Score = score;
		if(m_HUD){
			m_HUD.SetScore (score);
		}
	}
}
