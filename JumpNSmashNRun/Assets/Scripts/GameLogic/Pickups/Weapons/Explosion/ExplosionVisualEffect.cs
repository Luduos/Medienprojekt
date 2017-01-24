using UnityEngine;
using System.Collections;

public class ExplosionVisualEffect : VisualEffect {

	[SerializeField] private ParticleSystem m_ExplosionParticleEffect = null;

	private float m_ExplosionRadius = 2.0f;
	private float m_ParticleRadiusMultiplikator = 2.0f;

	public float ExplosionRadius{
		get{
			return m_ExplosionRadius;
		}
		set{
			m_ExplosionRadius = value;

		}
	}

	void Start(){
		if(null != m_ExplosionParticleEffect){
			m_ExplosionParticleEffect.startLifetime = m_EffectDuration;
			m_ExplosionParticleEffect.startSize = m_ExplosionRadius * m_ParticleRadiusMultiplikator;
		}else{
			Debug.LogWarning ("Missing Particle Effect in ExplosionVisualEffect", this);
		}
	}

	public override void OnStartEffect ()
	{
		StartExplosionParticleEffect ();
	}

	public override void OnUpdate ()
	{

	}

	public override void OnEndEffect ()
	{
		
	}

	private void StartExplosionParticleEffect(){
		if(null != m_ExplosionParticleEffect){
			m_ExplosionParticleEffect = Instantiate (m_ExplosionParticleEffect);
			ResetParticleTransform ();
			m_ExplosionParticleEffect.Play ();
		}
	}

	private void ResetParticleTransform(){
		m_ExplosionParticleEffect.transform.SetParent (this.transform);
		m_ExplosionParticleEffect.transform.localScale = Vector3.one;
		m_ExplosionParticleEffect.transform.localRotation = Quaternion.identity;
		m_ExplosionParticleEffect.transform.localPosition= Vector3.zero;
	}

	private void UpdateParticleRadius(){
		if(null != m_ExplosionParticleEffect){
			m_ExplosionParticleEffect.startSize = m_ExplosionRadius;
		}
	}
}
