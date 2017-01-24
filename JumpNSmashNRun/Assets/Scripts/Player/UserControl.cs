using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Takes in the User Input, processes it and then forwards the data to the CharacterMovement Behaviour
/// </summary>
[RequireComponent (typeof(IMovement))]
public class UserControl : MonoBehaviour
{

	[SerializeField] public string m_PlayerPrefix = "_P1";
	[SerializeField] float m_UseableCooldown = 0.5f;

	private float m_TimeSinceLastUsedUseable = 0.0f;
    public string player;
	private IMovement m_CharacterMovement;

	private Transform m_MainCameraTransform;
	private bool m_IsJumping;
    private bool m_IsRunning;
    private bool m_isSliding;
    private bool m_isPunching;
    private bool m_isKicking;
	private Vector3 m_MoveDirection;

	void Start ()
	{
       
		m_CharacterMovement = GetComponent<IMovement> ();

		if (null != Camera.main) {
			m_MainCameraTransform = Camera.main.transform;
		} else {
			Debug.LogWarning ("No main camera found. UserControl needs a Camera tagged MainCamera for camera-relative controls");
		}
	}

	void FixedUpdate ()
	{
        player = m_PlayerPrefix;
        float horizontal = ReadHorizontalPlayerInput();
		float vertical = ReadVerticalPlayerInput ();
		// range [-1;1] 
		m_MoveDirection = CalculateMoveDirection (horizontal, vertical);

		m_IsJumping = ReadIsJumping();

		// check, if we are sliding
		m_isSliding = CrossPlatformInputManager.GetButton(m_PlayerPrefix + "Slide");
		//Debug.Log("Wer slidet:" + m_PlayerPrefix);
		//Check, if we are punching
		m_isPunching = CrossPlatformInputManager.GetButton(m_PlayerPrefix + "Punch");
		//Check, if we are kicking
		m_isKicking = CrossPlatformInputManager.GetButton(m_PlayerPrefix + "Kick");
        m_CharacterMovement.HandleMovement (m_MoveDirection, m_IsJumping, m_isSliding, m_isPunching, m_isKicking);

   

		UpdateTimeSinceLastUsedItem ();
		if(ReadIsUsingItem()){
			UseItem ();
		}

        m_isSliding = false;
        m_IsRunning = false;
        m_isPunching = false;
        m_isKicking = false;
	}

	private float ReadHorizontalPlayerInput(){
		return CrossPlatformInputManager.GetAxis (m_PlayerPrefix + "Horizontal");
	}

	private float ReadVerticalPlayerInput(){
		return CrossPlatformInputManager.GetAxis (m_PlayerPrefix + "Vertical");
	}

	private bool ReadIsJumping(){
		return CrossPlatformInputManager.GetButton (m_PlayerPrefix + "Jump");
	}

	private bool ReadIsUsingItem(){
		return CrossPlatformInputManager.GetButton (m_PlayerPrefix + "UseItem");
	}

	private Vector3 CalculateMoveDirection(float horizontal, float vertical){
		Vector3 calculatedMoveDirection;
		if(null != m_MainCameraTransform){
			calculatedMoveDirection = horizontal * m_MainCameraTransform.right + vertical * m_MainCameraTransform.up;
		}else{
			// Else: just use world-relative directions and pray for the best
			calculatedMoveDirection = horizontal * Vector3.right + vertical * Vector3.up;
		}
		return calculatedMoveDirection;
	}

	private void UpdateTimeSinceLastUsedItem(){
		if(m_TimeSinceLastUsedUseable < m_UseableCooldown){
			m_TimeSinceLastUsedUseable += Time.deltaTime;
		}
	}

	private void UseItem(){
		Useable useableItem = gameObject.GetComponent<Useable> ();
		if(null != useableItem && CanUseItem()){
			useableItem.Use ();
			m_TimeSinceLastUsedUseable = 0f;
		}
	}

	private bool CanUseItem(){
		return m_TimeSinceLastUsedUseable > m_UseableCooldown;
	}
}
