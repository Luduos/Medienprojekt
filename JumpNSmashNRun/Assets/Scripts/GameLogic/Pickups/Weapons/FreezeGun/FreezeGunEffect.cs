using UnityEngine;
using System.Collections;

public class FreezeGunEffect : EffectOnPlayer {

	private Vector3 m_GlideVelocity;
	private Rigidbody m_RigidBody = null;

	[SerializeField] float m_GlideSpeedMultiplicator = 0.5f;

	public override void OnStartEffect ()
	{
		GetRigidBody ();
		GetGlideVelocity ();
		DisablePlayerControl (m_EffectDuration);
	}

	public override void OnUpdate ()
	{
		ApplyGliding ();
	}

	public override void OnEndEffect ()
	{
		
	}

	private void ApplyGliding(){
		m_RigidBody.velocity = m_GlideSpeedMultiplicator * m_GlideVelocity;
	}

	private void GetGlideVelocity(){
		m_GlideVelocity = new Vector3 (m_RigidBody.velocity.x, 0f, 0f);
	}

	private void GetRigidBody(){
		m_RigidBody = gameObject.GetComponent<Rigidbody> ();
		if(null == m_RigidBody){
			Debug.LogWarning ("Could not find a rigidbody. Effect is being destroyed.", this);
			CleanUp ();
		}
	}
}
