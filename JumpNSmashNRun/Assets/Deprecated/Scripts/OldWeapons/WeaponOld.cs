using UnityEngine;
using System.Collections;

public class WeaponOld : MonoBehaviour
{

	[Tooltip ("Bone, at which the weapon should be attached")]
	[SerializeField]
	private string m_SocketName = "WeaponSocket";

	[Tooltip ("Distance from Weapon Bone in local coordinates")]
	[SerializeField]
	private Vector3 m_LocalPosition = new Vector3 (0.1f, -0.03f, -0.26f);

	[Tooltip ("Rotation from Weapon Bone in local coordinates")]
	[SerializeField]
	private Vector3 m_LocalRotation = new Vector3 (25.0f, 153.0f, 168.0f);

	[Tooltip ("Uniform scale of the weapon at the weapon bone")]
	[SerializeField]
	private float m_LocalScale = 3.0f;

	public void AttachWeaponTo (GameObject other)
	{
		Transform weaponBone = RecursiveFindWeaponBone (other.transform);
		if (!weaponBone) {
			Debug.LogWarning ("Weapon.cs: Missing bone to attach weapon to.");
			return;
		}
		// attach weapon to player
		this.gameObject.transform.parent = weaponBone;
		this.gameObject.transform.localPosition = m_LocalPosition;
		this.gameObject.transform.localRotation = Quaternion.Euler (m_LocalRotation);
		this.gameObject.transform.localScale = new Vector3 (m_LocalScale, m_LocalScale, m_LocalScale);
	}

	/// <summary>
	/// Recursive digging for weapon bone.
	/// </summary>
	/// <returns>The for weapon bone.</returns>
	/// <param name="other">Other.</param>
	private Transform RecursiveFindWeaponBone (Transform other)
	{
		if (other.name.Equals (m_SocketName)) {
			return other;
		}
		for (int i = 0; i < other.childCount; i++) {
			Transform temp = RecursiveFindWeaponBone (other.GetChild (i));
			if (temp) {
				return temp;
			}
		}
		return null;
	}
}
