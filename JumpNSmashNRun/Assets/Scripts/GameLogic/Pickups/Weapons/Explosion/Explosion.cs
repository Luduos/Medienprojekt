using UnityEngine;
using System.Collections;

public class Explosion : Useable {

	[SerializeField] private LayerMask m_ExplosionCanHitTheseLayers = -1;
	[SerializeField] private float m_ExplosionRadius = 2.0f;
    

	private Collider[] m_ExplosionColliders;

	override protected void Initialize(){
	}

	override public void Use(){
		bool hasHitSomething;

		Explode (out hasHitSomething);
         

        if (hasHitSomething){
			ReactToHit ();
		}

		PlayExplosionVisibleEffect ();

        PlaySoundEffect();

        Destroy (this);
	}

	private void Explode(out bool hasHitSomething){
		m_ExplosionColliders = Physics.OverlapSphere (this.transform.position, m_ExplosionRadius, m_ExplosionCanHitTheseLayers);
        hasHitSomething = m_ExplosionColliders.Length > 0;
	}

	private void ReactToHit(){
		foreach(Collider explosionCollider in m_ExplosionColliders){
			GameObject hitGameObject = explosionCollider.gameObject;

			ApplyExplosionEffectIfValidTarget (hitGameObject);
		}
	}

	private void ApplyExplosionEffectIfValidTarget(GameObject hitGameObject){
		bool IsPlayer = hitGameObject.CompareTag ("Player");
		bool IsPlayerUsingExplosion = hitGameObject.Equals(this.gameObject);

		if(IsPlayer && !IsPlayerUsingExplosion){
			ExplosionGameplayEffect explosionEffect = ApplyGamelogicEffectToPlayer (hitGameObject) as ExplosionGameplayEffect;
			if(null != explosionEffect){
				explosionEffect.SetExplosionActive (this.transform.position, m_ExplosionRadius);
			}
		}
	}

	private void PlayExplosionVisibleEffect(){
		ExplosionVisualEffect explosionVisualEffect = ApplyVisualEffectTo (this.gameObject) as ExplosionVisualEffect;
		if(null != explosionVisualEffect){
			explosionVisualEffect.ExplosionRadius = m_ExplosionRadius;
		}
	}

    private void PlaySoundEffect()
    {
        SoundManager soundManager = GameObject.FindObjectOfType<SoundManager>();
        if (null != soundManager)
        {
            soundManager.PlayExplosionSound();
        }
    }
}
