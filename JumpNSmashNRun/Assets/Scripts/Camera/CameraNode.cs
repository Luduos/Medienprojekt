using UnityEngine;
using System.Collections;

public class CameraNode : MonoBehaviour {

	[SerializeField] private CameraNode m_NextNode = null;
	[SerializeField] private float m_MoveToThisNodeWithSpeed = 5.0f;
	[SerializeField] private float m_StopBeforeMovingToThisNode = 0.0f;
	[SerializeField] private bool m_IsFirstNode = false;


	public float StopBeforeMovingToThisNode{
		get{
			return m_StopBeforeMovingToThisNode;
		}
		set{
			this.m_StopBeforeMovingToThisNode = value;
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public CameraNode GetNextNode(){
		return m_NextNode;
	}

	public float GetCameraSpeed(){
		return m_MoveToThisNodeWithSpeed;
	}

	public float GetStopBeforeMovingToThisNode(){
		return m_StopBeforeMovingToThisNode;
	}

	public bool IsFirstNode(){
		return m_IsFirstNode;
	}


}
