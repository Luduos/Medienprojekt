using UnityEngine;
using System.Collections;

public class MortarShell : MonoBehaviour {

	[SerializeField] private Explosion m_MortarExplosion = null;
	[SerializeField] private float m_MortarExplosionLifeTime = 4.0f;
	[SerializeField] private float m_MortarShellAdjustmentRotation = 90f;

	private Rigidbody m_MortarShellRigidBody = null;

	void OnCollisionEnter(Collision collision){
		Explosion mortarExplosion = (Explosion)Instantiate (m_MortarExplosion, this.transform.position, Quaternion.identity);
		mortarExplosion.Use ();

		Destroy (mortarExplosion.gameObject, m_MortarExplosionLifeTime);
		Destroy (this.gameObject);
	}

	void Start(){
		m_MortarShellRigidBody = this.gameObject.GetComponent<Rigidbody> ();
		if(null == m_MortarShellRigidBody){
			Debug.LogError ("Couldn't find Rigidbody in MortarShell. Destroying MortarShell.", this);
			Destroy (this.gameObject);
		}
	}

	void Update(){
		transform.LookAt (transform.position + m_MortarShellRigidBody.velocity);
		float adjustmentRotationSign = GetAdjustmentRotationSign ();
		transform.RotateAround (transform.position, Vector3.forward, adjustmentRotationSign * m_MortarShellAdjustmentRotation);
	}

	private float GetAdjustmentRotationSign(){
		float adjustmentRotationSign = 0f;
		if(m_MortarShellRigidBody.velocity.x < 0){
			adjustmentRotationSign = 1f;
		}else{
			adjustmentRotationSign = -1f;
		}
		return adjustmentRotationSign;
	}
}
