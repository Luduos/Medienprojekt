using System.Collections;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
[RequireComponent (typeof(Renderer))]
public class Pickup : MonoBehaviour
{
	[Tooltip ("Area in which the player can get the pickup")]
	[SerializeField] private SphereCollider m_PickupArea;

	[Tooltip ("Put a Prefab with a Usable-Component in here, that should be given to the player")]
	[SerializeField] private Useable m_PickupType = null;

	[ Tooltip ( "Value less or equal 0 means no respawn. Else Respawn Time in Seconds.")]
	[SerializeField] private float m_RespawnTime = 0.0f;

	private float m_TimeSinceLastRespawn = 0.0f;

	private bool m_IsCurrentlyRespawning = false;

	private Collider m_Collider = null;

	private Renderer m_Renderer = null;


	public Pickup() {

	}

	void Start(){
		InitPickupArea ();
		CheckPickupType ();

		m_Collider = gameObject.GetComponent<Collider> ();
		m_Renderer = gameObject.GetComponent<Renderer> ();
	}

	void Update(){
		if(m_IsCurrentlyRespawning){
			UpdateRespawnTimer ();
		}
	}

	private void UpdateRespawnTimer(){
		m_TimeSinceLastRespawn += Time.deltaTime;
		if(m_TimeSinceLastRespawn > m_RespawnTime){
			m_IsCurrentlyRespawning = false;
			m_TimeSinceLastRespawn = 0.0f;
			Activate ();
		}
	}

	void OnTriggerEnter(Collider collidingWith){
		TryPickUp (collidingWith);
	}

	void OnTriggerStay(Collider collidingWith) {
		TryPickUp (collidingWith);
	}

	private void InitPickupArea(){
		m_PickupArea = this.gameObject.GetComponent<SphereCollider> ();
		m_PickupArea.isTrigger = true;
	}

	private void CheckPickupType(){
		if(null == m_PickupType){
			gameObject.AddComponent<DebugText> ().SetDebugText ("Missing\nPickup Type");
		}
	}

	private void TryPickUp(Collider collidingWith){
		if(IsCollidingWithPlayer(collidingWith) && ! AlreadyHasUseable(collidingWith)){
			Useable addedUsable = (Useable) Utils.CopyComponentFromTo(m_PickupType, collidingWith.gameObject);
			addedUsable.OnPickup ();
			FinishPickup ();
		}
	}

	private bool IsCollidingWithPlayer(Collider otherCollider){
		return otherCollider.gameObject.CompareTag ("Player");
	}

	private bool AlreadyHasUseable(Collider otherCollider){
		Useable potentialUseable = otherCollider.gameObject.GetComponent<Useable> ();
		bool alreadyHasUseable = (null != potentialUseable);
		return alreadyHasUseable;
	}

	private void FinishPickup(){
		if(m_RespawnTime > 0.0f){
			Deactivate ();
			m_IsCurrentlyRespawning = true;
		}else{
			Dispose ();
		}
	}

	private void Activate(){
		m_Renderer.enabled = true;
		m_Collider.enabled = true;
	}

	private void Deactivate(){
		m_Renderer.enabled = false;
		m_Collider.enabled = false;
	}

	private void Dispose(){
		Destroy (gameObject);
	}
}
