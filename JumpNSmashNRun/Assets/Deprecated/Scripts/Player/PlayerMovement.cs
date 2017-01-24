using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Experimental Class, to try to make the player movement Mario-like.
/// Really doesn't work right now!!
/// </summary>
public class PlayerMovement : MonoBehaviour, IMovement
{
	private Animator m_Animator;
	private Rigidbody m_Rigidbody;

	[SerializeField]
	private float m_MaxSpeedGroundHorizontal = 5.0f;

	public float M_MaxSpeedGroundHorizontal {
		get {
			return m_MaxSpeedGroundHorizontal;
		}
		set {
			m_MaxSpeedGroundHorizontal = value;	
			UpdateHorizontalGroundAcceleration ();
			UpdateHorizontalGroundDeceleration ();
		}
	}

	[SerializeField]
	private float m_TimeToFullSpeedGroundHorizontal = 0.2f;

	public float M_TimeToFullSpeedGroundHorizontal {
		get {
			return m_TimeToFullSpeedGroundHorizontal;
		}
		set {
			m_TimeToFullSpeedGroundHorizontal = value;
			UpdateHorizontalGroundAcceleration ();
		}
	}

	[SerializeField]
	private float m_TimeToStopGroundHorizontal = 0.1f;

	public float M_TimeToStopGroundHorizontal {
		get {
			return m_TimeToStopGroundHorizontal;
		}
		set {
			m_TimeToStopGroundHorizontal = value;
			UpdateHorizontalGroundDeceleration ();
		}
	}

	private float m_HorizontalGroundAcceleration;
	private float m_HorizontalGroundDeceleration;

	private Vector3 m_Acceleration;

	private static float EPSILON = 0.00001f;

	// Use this for initialization
	void Start ()
	{
		m_Animator = GetComponent<Animator> ();
		m_Animator.applyRootMotion = false;

		m_Rigidbody = GetComponent<Rigidbody> ();
		// Set constraints on player movement - for now he can't move in z-direction. Maybe we'll change that!
		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
		| RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;

		// init values
		UpdateHorizontalGroundAcceleration ();
		UpdateHorizontalGroundDeceleration ();

		m_Acceleration = new Vector3 ();

	}

	public void HandleMovement (Vector3 MovementDirection,bool IsJumping, bool IsSliding, bool IsPunching, bool IsKicking)
	{
		m_Acceleration = Vector3.zero;

		HandleHorizontalMovement (MovementDirection, IsJumping);

		// update velocity
		m_Rigidbody.velocity = m_Rigidbody.velocity + m_Acceleration * Time.fixedDeltaTime;
		// clamp to max horizontal speed
		m_Rigidbody.velocity = clampVelocity (MovementDirection, IsJumping);

		//Debug.Log (MovementDirection);
	}

	private Vector3 clampVelocity (Vector3 MovementDirection, bool IsJumping)
	{
		float clampedHorizontalVelocity;
		// if we're under a certain threshhold, just stop
		if (m_Rigidbody.velocity.x < EPSILON && m_Rigidbody.velocity.x > EPSILON) {
			clampedHorizontalVelocity = 0f;
		} else {
			clampedHorizontalVelocity = Mathf.Clamp (
				m_Rigidbody.velocity.x,
				-m_MaxSpeedGroundHorizontal,
				m_MaxSpeedGroundHorizontal
			);
		}

		return new Vector3 (clampedHorizontalVelocity, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
	}

	private void HandleHorizontalMovement (Vector3 MovementDirection, bool IsJumping)
	{
		MovementDirection.Normalize ();

		float xAcceleration = 0f;
		if (MovementDirection.x > EPSILON || MovementDirection.x < -EPSILON) {
			xAcceleration = MovementDirection.x * m_HorizontalGroundAcceleration;
		} else {
			float sign = 0f;
			if (m_Rigidbody.velocity.x > EPSILON) {
				sign = -1f;
			} else if (m_Rigidbody.velocity.x < -EPSILON) {
				sign = 1f;
			}
			xAcceleration = sign * Mathf.Abs (m_Rigidbody.velocity.x / m_MaxSpeedGroundHorizontal) * m_HorizontalGroundDeceleration;
		}

		m_Acceleration = new Vector3 (xAcceleration, m_Acceleration.y, m_Acceleration.z);
	}

	private Vector3 CorrectVector3Alteration (float x, float y, float z)
	{
		return new Vector3 (x, y, z);
	}

	private void UpdateHorizontalGroundAcceleration ()
	{
		if (m_TimeToFullSpeedGroundHorizontal < EPSILON) {
			m_TimeToFullSpeedGroundHorizontal = EPSILON;
		}
		m_HorizontalGroundAcceleration = m_MaxSpeedGroundHorizontal / m_TimeToFullSpeedGroundHorizontal;
	}

	private void UpdateHorizontalGroundDeceleration ()
	{
		if (m_TimeToStopGroundHorizontal < EPSILON) {
			m_TimeToStopGroundHorizontal = EPSILON;
		}
		m_HorizontalGroundDeceleration = m_MaxSpeedGroundHorizontal / m_TimeToStopGroundHorizontal;
	}

    public void HandleMovement(Vector3 MovementDirection, bool IsJumping, bool IsSliding, bool IsPunching, bool IsKicking, bool IsJumpingOver)
    {
        throw new NotImplementedException();
    }
}
