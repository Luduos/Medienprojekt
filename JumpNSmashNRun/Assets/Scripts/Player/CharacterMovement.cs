using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(CapsuleCollider))]
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(UserControl))]
public class CharacterMovement : MonoBehaviour, IMovement
{
    
    [Tooltip ("Multiplier for moving left and right")]
	[SerializeField]
	float m_MoveSpeedMultiplierHorizontal = 7.0f;

    [Tooltip("Multiplier for slowingDownMovement")]
    [SerializeField]
    private float m_SlowMultiplierX = 0.2f;

    [Tooltip ("Multiplier for moving up and down (for example for speeding up the fall)")]
	[SerializeField]
	float m_MoveSpeedMultiplierVertical = 1.0f;

	[Tooltip ("Jump Cooldown in Seconds. REALLY REALLY shouldn't be zero!!")]
	[SerializeField]
	float m_JumpCooldown = 0.05f;

	[Tooltip ("Velocity in y-Direction at the beginning of the jump")]
	[SerializeField]
	float m_JumpPower = 7.0f;

	[Tooltip ("We assume, that the transform of our collision is at the very bottom. If the raycast starts at the wrong point - change this value to adjust it.")]
	[SerializeField]
	float m_GroundRayCastOffsetY = 0.05f;

	[Tooltip ("Check the layers, on which the player should be able to jump.")]
	[SerializeField]
	private LayerMask m_LayersForJumping = -1;

	[Tooltip ("Maximal Number of Walljumps on the same wall")]
	[SerializeField]
	private int m_MaxWallJumpsOnSameWall = 1;

	[Tooltip ("Length of the Ray-casts, that check, if we're at a wall")]
	[SerializeField]
	private float m_WallCheckRadius = 0.2f;

	[Tooltip ("Offset in y-Direction of the Ray-casts, that checks, if we're at a wall")]
	[SerializeField]
	private float m_WallRaysOffsetY = 0.4f;


	private Rigidbody m_Rigidbody;
	private Animator m_Animator;
	private CapsuleCollider m_Collider;
    private UserControl m_UserControl;


    [SerializeField]
    private float m_PunchDistance = 1.0f;
    [SerializeField]
    private float m_PunchDamage = 5.0f;
    [SerializeField]
    private float m_PunchCooldown = 1.0f;

    private float m_CurrentPunchCooldown = 0.0f;

    [SerializeField]
    private float m_Kickdistance = 2.0f;
    [SerializeField]
    private float m_Kickdamage = 10.0f;
    [SerializeField]
    private float m_KickCooldown = 2.5f;

   

    private float m_CurrentKickCooldown = 0.0f;

    private float m_CombatActionCooldown = 0.5f;

    private PunchPhysics m_PunchPhysics;
    private HudWithAutoConnect playerHud = null;

    private Vector3 m_GroundRayOriginOffsetLeft;
	private Vector3 m_GroundRayOriginOffsetRight;

	private Vector3 m_WallRayOriginOffset;

	private bool m_IsGrounded;
    private bool m_IsSliding;
    private bool m_IsPunching;
    private bool m_IsKicking;

    private float m_ColliderHeightDefault;
    private float m_ColliderHeightSlide;
	private bool m_IsInFrontOfWall;

	private bool m_JumpButtonWasPressedAgain = true;

	private int m_JumpsOnSameWall;

	private float m_GroundRayCastLength;
	private float m_GroundRayCastOffsetX;

	private float m_TimeSinceLastJump;


	// Use this for initialization
	void Start ()
	{
		m_Animator = GetComponent<Animator> ();
		m_Animator.applyRootMotion = false;
		m_Rigidbody = GetComponent<Rigidbody> ();
        m_PunchPhysics = GetComponent<PunchPhysics>();
        m_UserControl = GetComponent<UserControl>();
        m_IsSliding = false;
        m_IsPunching = false;
        m_IsKicking = false;
		// Set constraints on player movement - for now he can't move in z-direction. Maybe we'll change that!
		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
		| RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;

		// we are expecting our player to use a capsule collider for collision
		m_Collider = GetComponent<CapsuleCollider> ();
      

		// Calculate the RayCast-Offset with 0.4 times the Radius of the CapsuleCollider
		// to prevent overlap of the groundbox with walls to the left or right (aka jumping, when cuddling a wall)
		m_GroundRayCastOffsetX = m_Collider.bounds.extents.x * 0.4f;

		// Our GroundRayCast length
		m_GroundRayCastLength = 0.1f;

		m_GroundRayOriginOffsetLeft = new Vector3 (-m_GroundRayCastOffsetX, m_GroundRayCastOffsetY, 0.0f);
		m_GroundRayOriginOffsetRight = new Vector3 (m_GroundRayCastOffsetX, m_GroundRayCastOffsetY, 0.0f);

		// Wall Sphere
		m_WallRayOriginOffset = new Vector3 (0f, m_WallRaysOffsetY, 0f);

        playerHud = this.gameObject.GetComponent<HudWithAutoConnect>();
        if(null == playerHud)
        {
            Debug.LogError("Couldn't find HudWithAutoConnect in CharacterMovement", this);
        }

    }

	public void HandleMovement (Vector3 MoveDirection, bool IsJumping, bool IsSliding, bool IsPunching, bool IsKicking)
	{
		m_IsPunching = IsPunching;
		m_IsKicking = IsKicking;
        m_IsSliding = IsSliding;
        // normalize MoveDirection, if needed
        if (MoveDirection.sqrMagnitude > 1.0f) {
			MoveDirection.Normalize ();
		}

		// Check, if we are on the ground
		m_IsGrounded = IsGrounded ();
		// check if we are at a wall
		m_IsInFrontOfWall = IsInFrontOfWall ();
		if (m_IsGrounded || !m_IsInFrontOfWall) {
			// reset jump-counter
			ResetJumpOnSameWallCounter ();
		}

		if(null == m_Rigidbody)
        {
            return;
        }

        float moveSpeedSignX = 1.0f;
        if(m_Rigidbody.velocity.x < 0.0f)
        {
            moveSpeedSignX = -1.0f;
        }


        float slowFactorX = ( m_Rigidbody.velocity.x) * (m_Rigidbody.velocity.x) * m_SlowMultiplierX + 0.05f;
        slowFactorX = Mathf.Min(slowFactorX, m_MoveSpeedMultiplierHorizontal);
        slowFactorX *= -1.0f * moveSpeedSignX;

        float moveSpeedX = m_MoveSpeedMultiplierHorizontal * MoveDirection.x + m_Rigidbody.velocity.x + slowFactorX;
        moveSpeedSignX = 1.0f;
        if(moveSpeedX < 0.0f)
        {
            moveSpeedSignX = -1.0f;
        }
        moveSpeedX = moveSpeedSignX * Mathf.Floor(moveSpeedSignX * moveSpeedX);


        m_Rigidbody.velocity = new Vector3 (
            moveSpeedX, 
			m_Rigidbody.velocity.y, 
			m_Rigidbody.velocity.z);
		
		// turn to MoveDirection
		if (MoveDirection.x > 0f) {
			m_Rigidbody.rotation = Quaternion.LookRotation (Vector3.right);
            //PlaySoundEffect();
		} else if (MoveDirection.x < 0f) {
			m_Rigidbody.rotation = Quaternion.LookRotation (Vector3.left);
            //PlaySoundEffect();
        }

		// Handle Jumping
		m_TimeSinceLastJump += Time.fixedDeltaTime;
		if (IsJumping && m_TimeSinceLastJump > m_JumpCooldown) {
			HandleJump ();
		}

        if(this.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Sliding"))
        {
            m_Collider.height = 0.8f;
            m_Collider.center = new Vector3(0.0f, 0.4f, 0.0f);
        }
        else
        {
            m_Collider.center = new Vector3(0.0f, 0.8f, 0.0f);
            m_Collider.height = 1.6f;
        }

		if (IsJumping && m_JumpButtonWasPressedAgain) {
			m_JumpButtonWasPressedAgain = false;
		} else if (!IsJumping) {
			m_JumpButtonWasPressedAgain = true;
		}

        

       

        UpdateAnimator ();
	}

    void FixedUpdate()
    {
        HandleCombat();
    }

	private void UpdateAnimator ()
	{
		float VelocityX = m_Rigidbody.velocity.x;
		if (VelocityX < 0f){
			VelocityX *= -1f;
		}
		m_Animator.SetFloat ("Forward", VelocityX, 0.1f, Time.deltaTime);
		m_Animator.SetBool ("OnGround", m_IsGrounded);
        m_Animator.SetBool("Slide", m_IsSliding);
        m_Animator.SetBool("Punch", m_IsPunching);
       // Debug.Log("Punching:" + m_IsPunching);
        m_Animator.SetBool("Kick", m_IsKicking);
        if (!m_IsGrounded) {
			m_Animator.SetFloat ("Jump", m_Rigidbody.velocity.y);
		} 

	}

	/// <summary>
	/// Determines whether Player is grounded, using two Raycasts
	/// </summary>
	/// <returns><c>true</c> if this instance is grounded; otherwise, <c>false</c>.</returns>
	private bool IsGrounded ()
	{
		Vector3 GroundRayOriginLeft = transform.position + m_GroundRayOriginOffsetLeft;
		Vector3 GroundRayOriginRight = transform.position + m_GroundRayOriginOffsetRight;

		#if UNITY_EDITOR
		// visualise the ground check rays in the scene view
		Debug.DrawLine (GroundRayOriginLeft, GroundRayOriginLeft + Vector3.down * m_GroundRayCastLength);
		Debug.DrawLine (GroundRayOriginRight, GroundRayOriginRight + Vector3.down * m_GroundRayCastLength);
		#endif

		return Physics.Raycast (GroundRayOriginLeft, Vector3.down, m_GroundRayCastLength, m_LayersForJumping)
		|| Physics.Raycast (GroundRayOriginRight, Vector3.down, m_GroundRayCastLength, m_LayersForJumping);
	}

	private bool IsInFrontOfWall ()
	{
		#if UNITY_EDITOR
		// visualise the wall check sphere in the scene view
		Vector3 WallRayOrigin = transform.position + m_WallRayOriginOffset;
		Debug.DrawLine (WallRayOrigin, WallRayOrigin + transform.forward * m_WallCheckRadius);
		#endif

		return Physics.Raycast (transform.position + m_WallRayOriginOffset, transform.forward, m_WallCheckRadius, m_LayersForJumping);
	}

	private void ResetJumpOnSameWallCounter ()
	{
		m_JumpsOnSameWall = 0;
	}

	/// <summary>
	/// Apply Jump, if all requirements are met
	/// </summary>
	private void HandleJump ()
	{
        PlaySoundEffectJump();
		if (m_IsGrounded) {
			m_Rigidbody.velocity = new Vector3 (m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
		} else if (m_IsInFrontOfWall && m_JumpsOnSameWall < m_MaxWallJumpsOnSameWall && m_JumpButtonWasPressedAgain) {
			m_JumpsOnSameWall++;
			m_Rigidbody.velocity = new Vector3 (m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
		} else {
			return;
		}
		// only reset, if we managed to make a jump
		m_TimeSinceLastJump = 0f;
	}

    private void HandleCombat()
    {
       
        if (m_IsPunching && (m_CurrentPunchCooldown < 0.0f))
        {
            m_CurrentPunchCooldown = m_PunchCooldown;
            m_PunchPhysics.Punch(m_PunchDistance, transform.forward, m_PunchDamage);

            //m_CurrentKickCooldown += m_CombatActionCooldown;
        }else
        {
            playerHud.SetPunchCD(m_CurrentPunchCooldown / m_PunchCooldown);
            m_CurrentPunchCooldown -= Time.deltaTime;
            m_IsPunching = false;

            playerHud.SetPunchCD(1.0f - (m_CurrentPunchCooldown / m_PunchCooldown));
        }

        if (m_IsKicking && (m_CurrentKickCooldown < 0.0f))
        {
            m_CurrentKickCooldown = m_KickCooldown;
            m_PunchPhysics.Punch(m_Kickdistance, transform.forward, m_Kickdamage);

           // m_CurrentPunchCooldown += m_CombatActionCooldown;
        }
        else 
        {
            m_CurrentKickCooldown -= Time.deltaTime;
            m_IsKicking = false;
            playerHud.SetKickCD(1.0f - (m_CurrentKickCooldown / m_KickCooldown));
        }
       
    }
    private void PlaySoundEffect()
    {
        SoundManager soundManager = GameObject.FindObjectOfType<SoundManager>();
        if (null != soundManager)
        {
            soundManager.FootstepsSound();
            
        }
    }
    private void PlaySoundEffectJump()
    {
        
        
         SoundManager soundManager = GameObject.FindObjectOfType<SoundManager>();
        if (null != soundManager)
        {
          
              if (m_UserControl.player.Equals("P1_"))
                  {
                 
                soundManager.JumpMale1();
                  }
              if (m_UserControl.player.Equals("P2_"))
              {
                soundManager.JumpMale2();
              }
            if (m_UserControl.player.Equals("P3_"))
            {
                soundManager.JumpMale3();
            }
            if (m_UserControl.player.Equals("P4_"))
            {
                soundManager.JumpMale4();
            }


        }

    }

}
