using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Updates the player points dependent on their location on the screen
/// </summary>
public class PlayerPointUpdate : MonoBehaviour {

	[SerializeField] private Camera m_MainCamera;
	[SerializeField] private string m_PlayerTag = "Player";

	[Tooltip("X position of the border between low-point and mid-point area")]
	[SerializeField] private float m_LowToMidXPosition = 0.33f;
	[Tooltip("X position of the border between mid-point and high-point area")]
	[SerializeField] private float m_MidToHighXPosition = 0.67f;

	[Tooltip("Time between increments")]
	[SerializeField] private float m_TimeBetweenIncrements = 1.0f;

	[Tooltip("Number of points to increment, without multiplicators")]
	[SerializeField] private float m_IncrementValue = 10.0f;
	[Tooltip("Low-point area multiplicator")]
	[SerializeField] private float m_LowPointMultiplicator = 0.5f;
	[Tooltip("Mid point area multiplicator")]
	[SerializeField] private float m_MidPointMultiplicator = 1.0f;
	[Tooltip("Hight point area multiplicator")]
	[SerializeField] private float m_HighPointMultiplicator = 1.5f;
	[Tooltip("Provide links to the correct HUD-Lines, to avoid searching for them")]

    private bool m_IsArenaPhase = false;

	private List<GameObject> m_Players;
	private float m_TimeSinceLastIncrement = 0.0f;

	void Start () {
		EnforceCameraExistence ();
		InitPlayerList ();
	}

	void Update () {
		
		m_TimeSinceLastIncrement += Time.deltaTime;
		if(m_TimeSinceLastIncrement > m_TimeBetweenIncrements){
			m_TimeSinceLastIncrement = 0.0f;
			UpdateAllPlayerScores ();
		}
	}

	private void UpdateAllPlayerScores(){
		if(!m_IsArenaPhase)
		{
			foreach (GameObject player in m_Players) {
				UpdatePlayerScore (player);
			}
		}
	}

	private void UpdatePlayerScore(GameObject player){
		Score playerScore = player.GetComponent<Score> ();
		if(null != playerScore){
			UpdateScoreIncrement (player.transform.position, playerScore);
		}
	}

	private void UpdateScoreIncrement(Vector3 playerPosition, Score playerScore){
		// transform player position to viewport --> in range [0,1]
		Vector3 viewPortPosition =  m_MainCamera.WorldToViewportPoint(playerPosition);
		float calculatedScoreIncrement = 0.0f;
		if(viewPortPosition.x < m_LowToMidXPosition){
			calculatedScoreIncrement = m_IncrementValue * m_LowPointMultiplicator;
			playerScore.CurrentScreenSection = Score.ScreenSection.LOW;
		} 
		else if(viewPortPosition.x < m_MidToHighXPosition){
			calculatedScoreIncrement = m_IncrementValue * m_MidPointMultiplicator;
			playerScore.CurrentScreenSection = Score.ScreenSection.MIDDLE;
		}
		else{
			calculatedScoreIncrement = m_IncrementValue * m_HighPointMultiplicator;
			playerScore.CurrentScreenSection = Score.ScreenSection.HIGH;
		}
		playerScore.AddToScore (calculatedScoreIncrement);
	}
		


	private void EnforceCameraExistence(){
		if (null == m_MainCamera)
		{
			m_MainCamera = Camera.main;
			if(null == m_MainCamera){
				Debug.LogWarning(
					"PlayerPointUpdate in GameManager: no main camera found. Component is disabled.");	
				this.enabled = false;
			}
		}
	}

	private void InitPlayerList(){
		m_Players = new List<GameObject> ();
		foreach(GameObject player in GameObject.FindGameObjectsWithTag (m_PlayerTag))
		{
			m_Players.Add (player);
			if(!player.GetComponent<Score> ()){
				Debug.LogWarning ("Couldn't find score-script in player-object ", player);
			}
		}
		if(!(m_Players.Count > 0))
		{
			Debug.LogWarning(
				"No Player-GameObjects with tag " + m_PlayerTag + " found.");		
		}
	}

    public void SetArenaPhase(bool isArena)
    {
        m_IsArenaPhase = isArena;
    }

}
