using UnityEngine;
using System.Collections;

/// <summary>
/// Usable effect, applied by a Usable. Weapons can apply negative UsableEffects on other players,
/// Items can apply positive UsableEffects on the carrying player
/// </summary>
abstract public class EffectOnPlayer : MonoBehaviour {

	[SerializeField] protected float m_EffectDuration;

	[ Tooltip ( "Positive Values do damage, negative Values heal")]
	[SerializeField] protected float m_HealthChange;

	protected float m_EffectActiveTime = 0f;
	protected PlayerHealthComponent m_PlayerHealth;

	private float m_DisabledControlDuration = 0f;
	private float m_DisabledControlActiveTime = 0f;



	public float EffectDuration{
		get{
			return m_EffectDuration;
		}
		set{
			m_EffectDuration = value;
		}
	}

	public abstract void OnStartEffect ();

	public abstract void OnUpdate();

	public abstract void OnEndEffect();

	void Start(){
		m_PlayerHealth = this.gameObject.GetComponent<PlayerHealthComponent> ();
		if(null != m_PlayerHealth){
			m_PlayerHealth.AddHealth (m_HealthChange);
		}
	}

	void LateUpdate(){
		OnUpdate ();

		UpdateEffectActiveTime ();
	}

	protected void CleanUp(){
		OnEndEffect ();
		EnablePlayerControl ();
		Destroy(this);
	}

	private void UpdateEffectActiveTime(){
		m_EffectActiveTime += Time.deltaTime;
		if(m_EffectActiveTime > m_EffectDuration){
			CleanUp ();
		}
	}

	protected void DisablePlayerControl(float disabledControlDuration){
		float remainingEffectActiveTime = m_EffectDuration - m_EffectActiveTime;
		m_DisabledControlDuration = Mathf.Min (remainingEffectActiveTime, disabledControlDuration);

		DisablePlayerControl ();
	}

	private void UpdateDisabledPlayerControl(){
		if(m_DisabledControlDuration > 0f){
			m_DisabledControlActiveTime += Time.deltaTime;
			if(m_DisabledControlActiveTime > m_DisabledControlDuration){
				EnablePlayerControl ();
			}
		}
	}

	private void EnablePlayerControl(){
		SetPlayerControl (true);
	}

	private void DisablePlayerControl(){
		SetPlayerControl (false);
	}

	private void SetPlayerControl(bool isPlayerControlEnabled){
		UserControl playerControl = gameObject.GetComponent<UserControl> ();
		if(null != playerControl){
			playerControl.enabled = isPlayerControlEnabled;
		}
	}
}
