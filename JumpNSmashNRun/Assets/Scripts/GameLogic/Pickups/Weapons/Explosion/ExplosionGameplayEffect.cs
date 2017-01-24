using UnityEngine;
using System.Collections;

public class ExplosionGameplayEffect : EffectOnPlayer {

	[SerializeField] private float m_ExplosionForce = 10.0f;
	[SerializeField] private float m_ExplosionYOffset = 0.5f;

	private Rigidbody m_RigidBody = null;

	public override void OnStartEffect ()
	{
		GetRigidBody ();
		DisablePlayerControl (m_EffectDuration);
		this.enabled = false;
	}

	public override void OnUpdate ()
	{
		// do nothing
	}

	public override void OnEndEffect ()
	{
		// do nothing
	}

	public void SetExplosionActive(Vector3 explosionPosition, float explosionRadius){
		this.enabled = true;
		m_RigidBody.AddExplosionForce (m_ExplosionForce, explosionPosition, explosionRadius, m_ExplosionYOffset, ForceMode.Impulse);
	}

	private void GetRigidBody(){
		m_RigidBody = gameObject.GetComponent<Rigidbody> ();
		if(null == m_RigidBody){
			Debug.LogWarning ("Could not find a rigidbody. Effect is being destroyed.", this);
			CleanUp ();
		}
	}
}
