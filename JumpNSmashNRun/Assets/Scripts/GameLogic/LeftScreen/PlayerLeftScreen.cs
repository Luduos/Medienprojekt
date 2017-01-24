using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LeftScreenRespawn))]
[RequireComponent(typeof(Score))]
[RequireComponent(typeof(PlayerHealthComponent))]
public class PlayerLeftScreen : MonoBehaviour {

    // boolean for arena trigger
    private bool m_InRunPhase = true;

	// for initialization of the LeftScreenRespawnComponent
	[SerializeField] private float RespawnTime = 2.0f;
	[SerializeField] private float BlinkIntervall = 0.1f;
    [SerializeField] private float PointsLostOnDeath = 100.0f;
    [Range(0.1f, 0.9f)][SerializeField] private float RespawnPositionX = 0.5f;
	[Range(0.1f, 0.9f)][SerializeField] private float RespawnPositionY = 0.7f;
	LeftScreenRespawn leftScreenComponent;
    Score score;
    private PlayerHealthComponent m_healthScript;

	private Camera m_MainCamera;
	// Use this for initialization
	void Start () {
		m_MainCamera = Camera.main;
		leftScreenComponent = this.gameObject.AddComponent<LeftScreenRespawn>();
        score = this.gameObject.GetComponent<Score>();
        m_healthScript = this.gameObject.GetComponent<PlayerHealthComponent>();
    }

	void FixedUpdate(){
		// Check if player has left screen, by projecting the world coordinates to viewport coordinates. if <0 or >1 --> left screen
		Vector3 viewPortPosition =  m_MainCamera.WorldToViewportPoint(this.transform.position);
		if (viewPortPosition.x < 0 || viewPortPosition.x > 1 
			|| viewPortPosition.y < 0 || viewPortPosition.y > 1) {
			// if yes, add RespawnComponent to player, to handle respawning, if it doesn't already have a RespawnComponent
			if(!m_InRunPhase)
            {
                leftScreenComponent.enabled = false;
                m_healthScript.SetPlayerIsDead(true);
                return;
            } else if (!leftScreenComponent.enabled) {
				leftScreenComponent.startComponent (m_MainCamera, BlinkIntervall ,RespawnTime, RespawnPositionX, RespawnPositionY);
                score.SubstractFromScore(PointsLostOnDeath);
			}
		}
	}

    // getter and setter for arena trigger
    public void SetInRunPhase(bool d)
    {
        this.m_InRunPhase = d;
    }

    public bool GetInRunPhase()
    {
        return this.m_InRunPhase;
    }
}
