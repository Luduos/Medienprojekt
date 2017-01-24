using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class DebugText : MonoBehaviour {

	[SerializeField] private TextMesh m_DebugTextMesh;

	private float m_LifeTime = -1f;
	private float m_TimeActive = 0f;

	public TextMesh DebugTextMesh{
		get{
			return m_DebugTextMesh;
		}
		set{
			m_DebugTextMesh = value;
		}
	}

	/// <summary>
	/// Gets or sets the life time. Life time <= 0 equals infinite life time.
	/// </summary>
	/// <value>The life time.</value>
	public float LifeTime{
		get{
			return m_LifeTime;
		}
		set{
			m_LifeTime = value;
		}
	}

	void Start(){
	}

	void LateUpdate(){
		LookAtCamera ();
		UpdateLifeTime ();
	}

	void OnDestroy(){
		Destroy (m_DebugTextMesh);
	}

	public void SetDebugText(string text){
		if(!m_DebugTextMesh){
			InitTextMesh ();
		}
		m_DebugTextMesh.text = text;
	}

	private void UpdateLifeTime(){
		if(m_LifeTime > 0f){
			m_TimeActive += Time.deltaTime;
			if(m_TimeActive > m_LifeTime){
				Destroy (this);
			}
		}
	}

	private void InitTextMesh(){
		m_DebugTextMesh = this.GetComponent<TextMesh> ();
		m_DebugTextMesh.characterSize = 0.1f;
		m_DebugTextMesh.lineSpacing = 1f;
		m_DebugTextMesh.fontSize = 16;
		m_DebugTextMesh.color = Color.red;
		LookAtCamera ();
	}

	private void LookAtCamera(){
		transform.rotation = Quaternion.LookRotation (Camera.main.transform.forward, Camera.main.transform.up);
	}
}
