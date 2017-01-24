using UnityEngine;
using System.Collections;

/// <summary>
/// Do not add this to a player yourself!! This will automatically be added by the PlayerLeftScreen Script, to start a Respawn and will be destroyed again, after usage
/// </summary>
public class LeftScreenRespawn : MonoBehaviour {

	private float m_PassedTime = 0.0f;
	private bool m_IsVisible = true;
	private float m_PassedTimeBlink = 0.0f;
	private bool m_Running = false;

	// variables set by PlayerLeftScreen
	private float m_RespawnTime;
	private float m_BlinkIntervall;
	private Camera m_MainCamera;
	private Vector2 m_RespawnVectorViewport;

	// derived variables
	private Vector2 m_RespawnVectorWorld;
	private SkinnedMeshRenderer[] m_MeshRenderers;
	private Rigidbody m_RigidBody;

	// Use this for initialization
	void Start () {
		m_RigidBody = this.gameObject.GetComponent<Rigidbody> ();
		m_MeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ();

		this.enabled = false;
	}

	/// <summary>
	/// Initializes the component by setting the needed variables and caluclating + setting the player position on the screen.
	/// </summary>
	/// <param name="cameraRef">Camera we can drop out of.</param>
	/// <param name="RespawnTimeValue">Length of the Respawn Time.</param>
	/// <param name="BlinkIntervalValue">Interval of Blinking during respawn</param>
	/// <param name="RespawnPositionX">Respawn position x in Viewport Coordinates.</param>
	/// <param name="RespawnPositionY">Respawn position y in Viewport Coordinates.</param>
	public void startComponent (Camera cameraRef, float BlinkIntervallValue,  float RespawnTimeValue , float RespawnPositionX, float RespawnPositionY){
		SetCamera (cameraRef);
		SetBlinkIntervall (BlinkIntervallValue);
		SetRespawnTime (RespawnTimeValue);
		SetRespawnVecor (RespawnPositionX, RespawnPositionY);

		m_RigidBody.useGravity = false;
		gameObject.GetComponent<Collider> ().isTrigger = true;

		this.enabled = true;
		m_Running = true;
	}


	void FixedUpdate(){
		if(!m_Running)
		{
			return;
		}

		m_PassedTime += Time.deltaTime;

		if (m_PassedTime > m_RespawnTime) 
		{
			RespawnFinished ();
			return;
		}
		SetPositionToRespawn();
		BlinkAnimation ();

		// UGLY WORKAROUND OF THE JITTERING PROBLEM. Try to fix the manual application of speed in the ThirdPersonCharacter Script
		m_RigidBody.velocity = new Vector3 (0.0f, 0.0f, 0.0f); 
	}
		
	private void SetPositionToRespawn(){
		// the respawn position in View Coordinates. The z position is used to put the player on z=0
		Vector3 respawnPosViewCoord = new Vector3(m_RespawnVectorViewport.x,m_RespawnVectorViewport.y ,- m_MainCamera.transform.position.z);
		// calculate the world position from the viewtoWorld Matrix
		Vector2 respawnPosWorld = m_MainCamera.ViewportToWorldPoint(respawnPosViewCoord);
		this.transform.position = new Vector3(
			respawnPosWorld.x,
			respawnPosWorld.y,
			this.transform.position.z);
	}

	/// <summary>
	/// Enables and disables the MeshRenderers to give a blink feeling and visual feedback during respawn
	/// </summary>
	private void BlinkAnimation(){
		m_PassedTimeBlink += Time.deltaTime;
		if (m_PassedTimeBlink > m_BlinkIntervall) {
			m_IsVisible = !m_IsVisible;
			m_PassedTimeBlink = 0.0f;
		}
		EnableMeshRenderers (m_IsVisible);
	}

	private void EnableMeshRenderers(bool IsEnabled){
		for (int i = 0; i < m_MeshRenderers.Length; i++) {
			m_MeshRenderers [i].enabled = IsEnabled;
		}
	}

	/// <summary>
	/// Called once the Respawn is finished. Disables the component and resets all variables
	/// </summary>
	private void RespawnFinished(){
		this.gameObject.GetComponent<Collider> ().isTrigger = false;
		m_RigidBody.velocity = new Vector3 (0.0f, 5.0f, 0.0f);
		m_RigidBody.useGravity = true;
		gameObject.GetComponent<Collider> ().isTrigger = false;
		EnableMeshRenderers (true);

		m_PassedTime = 0.0f;
		m_PassedTimeBlink = 0.0f;

		m_Running = false;
		this.enabled = false;
	}

	public void SetRespawnTime(float value){
		m_RespawnTime = value;
	}

	public void SetBlinkIntervall(float value){
		m_BlinkIntervall = value;
	}

	public void SetCamera(Camera cam){
		m_MainCamera = cam;
	}

	public void SetRespawnVecor(float x, float y){
		m_RespawnVectorViewport = new Vector2 (x, y);

	}
}
