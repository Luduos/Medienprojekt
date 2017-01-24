using UnityEngine;
using System.Collections;

public class DebugGamelogicEffect : EffectOnPlayer {

	public DebugGamelogicEffect(){
		this.m_EffectDuration = 5.0f;
	}

	public override void OnStartEffect ()
	{
		gameObject.AddComponent<DebugText> ().SetDebugText ("No Effect added.");
		Debug.LogError ("No UseableGamelogicEffect was added to the recently used Usable.", this);
	}

	public override void OnUpdate ()
	{
		// do nothing
	}

	public override void OnEndEffect ()
	{
		// do nothing
	}
	
}
