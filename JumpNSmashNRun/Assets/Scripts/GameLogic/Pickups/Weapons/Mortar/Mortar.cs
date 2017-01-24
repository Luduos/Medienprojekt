using UnityEngine;
using System.Collections;

public class Mortar : Useable {

	[SerializeField] private MortarShell m_MortarShellPrefab = null;

	[SerializeField] private float m_ShotForce = 100.0f;
	[SerializeField] private float m_MaxMortarShellLifeTime = 10.0f;

	[SerializeField] private Vector3 m_LocalShotStartOffset = new Vector3 (0.5f, 0.5f, 0f);
	[SerializeField] private Vector3 m_LocalShotdirection = new Vector3(0.1f, 0.5f,0.0f);

	private MortarShell m_MortarShell = null;

	
	protected override void Initialize ()
	{
		if(null == m_MortarShellPrefab){
			Debug.LogError ("Missing MortarShell script in Picked-Up Mortar, destroying component.", this);
			Destroy (this);
		}
		m_LocalShotdirection.Normalize ();
	}

	public override void Use ()
	{
		FireOffMortarShell ();

        PlaySoundEffect();

        Destroy (this);
	}

	private void FireOffMortarShell(){
		Vector3 worldShellStartPosition = GetWorldShellStartPosition ();

		m_MortarShell = Instantiate (m_MortarShellPrefab, worldShellStartPosition, Quaternion.identity) as MortarShell;
		if (null != m_MortarShell)
		{
			ApplyMortarVelocity ();
		}
		Destroy (m_MortarShell.gameObject, m_MaxMortarShellLifeTime);
	}

	private void ApplyMortarVelocity(){
		Rigidbody mortarShellRigidBody = m_MortarShell.gameObject.GetComponent<Rigidbody> ();
		if(null != mortarShellRigidBody){
			Vector3 worldShotForceDirection = GetWorldShotForceDirection ();

			mortarShellRigidBody.AddForce(worldShotForceDirection, ForceMode.Impulse);
		}
	}

	private Vector3 GetWorldShellStartPosition(){
		Vector3 localShellStartPosition = new Vector3(
			m_LocalShotStartOffset.x * this.transform.forward.x,
			m_LocalShotStartOffset.y,
			m_LocalShotStartOffset.z
		);
		Vector3 worldShellStartPosition = this.transform.position + localShellStartPosition;
		return worldShellStartPosition;
	}

	private Vector3 GetWorldShotForceDirection(){
		Vector3 localShotForceDirection = m_ShotForce * m_LocalShotdirection;
		Vector3 worldShotForceDirection = new Vector3 (
			localShotForceDirection.x * this.transform.forward.x,
			localShotForceDirection.y,
			localShotForceDirection.z);
		return worldShotForceDirection;
	}

    private void PlaySoundEffect()
    {
        SoundManager soundManager = GameObject.FindObjectOfType<SoundManager>();
        if (null != soundManager)
        {
            soundManager.PlayMortarSound();
        }
    }
}
