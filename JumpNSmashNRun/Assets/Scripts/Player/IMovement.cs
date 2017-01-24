using System.Collections;
using UnityEngine;

public interface IMovement
{

	void HandleMovement (Vector3 MovementDirection, bool IsJumping, bool IsSliding, bool IsPunching, bool IsKicking);
}
