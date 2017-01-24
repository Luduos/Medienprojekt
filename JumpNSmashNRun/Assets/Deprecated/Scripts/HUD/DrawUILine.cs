using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// Draws the Running Phase UI-Line
/// 
/// DOESNT WORK RIGHT NOW; BECAUSE UNITY UI IS DUMB!
/// 
/// </summary>
[ExecuteInEditMode]
public class DrawUILine : Graphic {

	[Tooltip("Is this UI-Line the Left or the right line?")]
	[SerializeField]
	private bool m_bIsLeftLine;

	[Tooltip("Width of the UI-Line")]
	[SerializeField]
	private float m_LineWidth = 0.01f;

	[Tooltip("X Position of the min Anchor")]
	[SerializeField]
	private float m_PositionX;

	//[Tooltip("Speed at which the UILine adjusts its position")]
	//[SerializeField]
	//private float m_AdjustSpeed = 1f;


		
	void Update(){
		
	}

	public bool IsLeftLine
	{
		get {
			return m_bIsLeftLine;
		}
		set{
			m_bIsLeftLine = value;
		}
	}

	public float LineWidth{
		get{
			return m_LineWidth;
		}
		set{
			m_LineWidth = value;
		}
	}

	public float PositionX{
		get{
			return m_PositionX;
		}
		set{
			m_PositionX = value;
		}
	}

}
