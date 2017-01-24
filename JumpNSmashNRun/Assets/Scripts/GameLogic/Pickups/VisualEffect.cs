using UnityEngine;
using System.Collections;

abstract public class VisualEffect : MonoBehaviour {

	[SerializeField] protected float m_EffectDuration;

	protected float m_EffectActiveTime = 0f;

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
	
	void LateUpdate(){
		OnUpdate ();

		UpdateEffectActiveTime ();
	}

	private void UpdateEffectActiveTime(){
		m_EffectActiveTime += Time.deltaTime;
		if(m_EffectActiveTime > m_EffectDuration){
			CleanUp ();
		}
	}

	protected void CleanUp(){
		OnEndEffect ();
		Destroy(this);
	}
}
