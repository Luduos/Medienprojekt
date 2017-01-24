using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SphereCollider))]
public class WeaponPickupOld : MonoBehaviour
{

	[Tooltip ("Area in which the player can get the weapon")]
	[SerializeField]
	private SphereCollider m_SphereCollider;

	[Tooltip ("Put the Weapon Prefab in here, that should be spawned on player")]
	[SerializeField]
	private WeaponOld m_WeaponType = null;

	[Tooltip ("RotationSpeed")]
	[SerializeField]
	private float m_RotationSpeed = 45.0f;

	[Tooltip ("Scale")]
	[SerializeField]
	private Vector3 m_Scale = new Vector3 (3f, 3f, 3f);

	[Tooltip ("Axis to rotate around")]
	[SerializeField]
	private Vector3 m_RotationVector = new Vector3 (0f, 1f, 0f);

	// actual instance of the weapontype
	private WeaponOld m_Weapon;

	// Use this for initialization
	void Start ()
	{
		m_SphereCollider = this.gameObject.GetComponent<SphereCollider> ();
		m_SphereCollider.isTrigger = true;

		if (m_WeaponType) {
			m_Weapon = (WeaponOld)Instantiate (m_WeaponType, transform.position, transform.rotation);
			m_Weapon.transform.localScale = m_Scale;
		} else {
			gameObject.AddComponent<DebugText> ().SetDebugText ("Missing\nWeaponType");
		}
	}

	void Update ()
	{
		if (!m_Weapon) {
			return;
		}
		m_Weapon.transform.RotateAround (m_Weapon.transform.position, m_RotationVector, m_RotationSpeed * Time.deltaTime);

	}

	void OnTriggerEnter (Collider other)
	{
		// if player
		if (other.gameObject.CompareTag ("Player") && m_Weapon) {
			m_Weapon.AttachWeaponTo (other.gameObject);

			// clean up
			Destroy (this.gameObject);
		}
	}

}
