using UnityEngine;
using System.Collections;

public class PlayerHealthComponent : MonoBehaviour {

    //Represents the HP of each player.
    //Has a Boolean which indicates the status of death or alive. (updates every frame)

    private float m_MaxHealth;
    private float m_CurrentHealth;
    private bool m_playerIsDead;

    //this Bool ensures that Damage is only taken when the ArenaPhase is triggered.
    private bool m_IsArenaPhase;

    //Scripts for communicating with the health sliders and the arena phase
    private HudWithAutoConnect m_healthSlider;

	// Use this for initialization
	void Start () {
        m_playerIsDead = false;
        m_IsArenaPhase = false;
        m_healthSlider = GetComponent<HudWithAutoConnect>();
        m_MaxHealth = m_healthSlider.GetStartingHealth();
        m_MaxHealth = m_healthSlider.GetStartingHealth();
        m_CurrentHealth = m_MaxHealth;
	}

    //Subtracts Health if Damage is taken. Uses the HUD script.
    public void SubtractHealth(float amount)
    {
        if (m_IsArenaPhase)
        {
            m_CurrentHealth -= amount;
            m_healthSlider.TakeDamage(amount);
            CheckDeath();
        }
        
    }

    public void AddHealth(float amount)
    {
        if (m_IsArenaPhase)
        {
            m_CurrentHealth += amount;
            m_healthSlider.GainHealth(amount);
            CheckDeath();
        }
    }

    public bool GetPlayerIsDead()
    {
        return m_playerIsDead;
    }

    public void SetPlayerIsDead(bool isDead)
    {
        m_playerIsDead = isDead;
        if (m_playerIsDead)
        {
            m_CurrentHealth = -0.1f;
        }      
    }

    public float GetCurrentHealth()
    {
        return m_CurrentHealth;
    }

    public void SetCurrentHealth(float health)
    {
        AddHealth(health - m_CurrentHealth);
    }

    public void SetIsArenaPhase(bool isArenaPhase)
    {
        m_IsArenaPhase = isArenaPhase;
        if (isArenaPhase)
        {
            SetHealthToMax();
        }
    }

    public bool GetIsArenaPhase()
    {
        return m_IsArenaPhase;
    }

    public void SetHealthToMax()
    {
        m_CurrentHealth = m_MaxHealth;
        m_healthSlider.SetHealth(m_MaxHealth);
        CheckDeath();
    }

    private void CheckDeath()
    {
        SetPlayerIsDead(m_CurrentHealth < 0.01f);
    }
}
