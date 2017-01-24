using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private CameraNode m_MoveToNode;
	private float m_StoppedForSeconds;

	[SerializeField] float m_AcceptableRadiusForReachingNode = 5.0f;
	private float m_AcceptableRadiusForReachingNodeSquared;

	private Vector2 m_VectorToNextNodeNormalized = new Vector2();

    // boolean for camera path switching #arenamode
    private bool m_stop = false;

    // current  Camera node for Arena teleportation #arenamode
    private CameraNode m_currentNode;

    private float m_ZPosition;

	/**
	 * Get the first Node
	 * */
	void Start () {
        m_ZPosition = this.transform.position.z;

        FindFirstNode ();
		if (null == m_MoveToNode) {
			Debug.LogError ("Couldn't find the first node. Please check, if you have checked the \"First Node\" Parameter.");
			return;
		}

		// Set Camera xy Position to the first Node xy position
		SetXYPosition(m_MoveToNode.transform.position.x, m_MoveToNode.transform.position.y);

		//Optimizing the distance check
		m_AcceptableRadiusForReachingNodeSquared = m_AcceptableRadiusForReachingNode * m_AcceptableRadiusForReachingNode;


		SetNormalizedVectorToNextNode ();
	}

	void FindFirstNode(){
		CameraNode[] pathNodes = Object.FindObjectsOfType<CameraNode> ();

		for (int i = 0; i < pathNodes.Length; i++) {
			m_currentNode = pathNodes[i];
			if (pathNodes [i].IsFirstNode ()) {
				m_MoveToNode = pathNodes [i];
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		// is there a next node?
		if (null == m_MoveToNode) {
			
			return;
		}
		// Have we reached the next node?
		Vector2 camToNodeVector = 
			new Vector2(
				this.transform.position.x - m_MoveToNode.transform.position.x, 
				this.transform.position.y - m_MoveToNode.transform.position.y
			);
		if (camToNodeVector.sqrMagnitude < m_AcceptableRadiusForReachingNodeSquared) {
			ReachedNextNode ();
		}
		// is there a next node?
		if (null == m_MoveToNode) {
			return;
		}
		// do we have to wait at the current node?
        // #arenamode
		if (m_StoppedForSeconds < m_MoveToNode.GetStopBeforeMovingToThisNode ()) {
			m_StoppedForSeconds += Time.deltaTime;
        } else if (m_stop)
        {
            return;
        }
        else
        {
            // if not, let's move to the next node;
            MoveToNextNodeDelta();
        }
	}

	/**
	 * Moves Camera to next Node according to current camera speed and delta time
	 * */
	void MoveToNextNodeDelta(){
		Vector2 deltaMoveVector = m_VectorToNextNodeNormalized * m_MoveToNode.GetCameraSpeed () * Time.deltaTime;
		this.transform.Translate (deltaMoveVector);
	}

	void SetNormalizedVectorToNextNode(){
		if (null != m_MoveToNode) {
			m_VectorToNextNodeNormalized.x = m_MoveToNode.transform.position.x - this.transform.position.x;
			m_VectorToNextNodeNormalized.y = m_MoveToNode.transform.position.y - this.transform.position.y;
			m_VectorToNextNodeNormalized.Normalize ();
		}
	}

	/**
	 * Logic behind what happens, when reaching the next point
	 * ŕeturns true, if there is a next Node
	 * */
	void ReachedNextNode(){
        setMoveToNode(m_MoveToNode.GetNextNode());
	}


	/**
	 * Set Position of Camera in the XY Plane
	 * */
	public void SetXYPosition(float x, float y){
		this.transform.position =  (new Vector3(x, y, m_ZPosition));
	}
    // getter and setter for #arenamode
    public void setStop(bool s)
    {
        this.m_stop = s;
    }

    public bool getStop()
    {
        return this.m_stop;
    }

    public CameraNode getCurrentNode()
    {
        return this.m_currentNode;
    }

    public void setMoveToNode(CameraNode cNode)
    {
        this.m_MoveToNode = cNode;
        m_StoppedForSeconds = 0.0f;
        SetNormalizedVectorToNextNode();
    }

    public void setJumpToNode(CameraNode cNode)
    {
        //Debug.Log("Camera Jump");
        this.m_MoveToNode = cNode;
        m_StoppedForSeconds = m_MoveToNode.GetStopBeforeMovingToThisNode();
        SetNormalizedVectorToNextNode();
    }

    public void JumpToThisNode(CameraNode jumpToThisNode)
    {
        setMoveToNode(jumpToThisNode);
        Vector3 newCameraPosition = jumpToThisNode.transform.position;
        SetXYPosition(newCameraPosition.x, newCameraPosition.y);
    }
}
