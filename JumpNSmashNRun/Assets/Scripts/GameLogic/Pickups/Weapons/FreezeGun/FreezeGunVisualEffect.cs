using UnityEngine;
using System.Collections;


public class FreezeGunVisualEffect : VisualEffect {

	[SerializeField] private Material m_EffectMaterial = null;

	[SerializeField] private Color m_StartColor = Color.blue;
	[SerializeField] private Color m_EndColor = Color.cyan;

	[SerializeField] private float m_EffectStartWidth = 0.25f;
	[SerializeField] private float m_EffectEndWidth = 0.5f;

	[SerializeField] private Vector3 m_StartOffset = new Vector3(0f, 0.4f, 0f);
	[SerializeField] private Vector3 m_EndOffset = new Vector3(0f, 0.4f, 0f);

	private LineRenderer m_EffectRenderer;
	private Transform m_TargetTransform = null;
	private Vector3 m_EndPosition;

	override public void OnStartEffect(){
		m_EffectRenderer = this.gameObject.GetComponent<LineRenderer> ();
		if(null == m_EffectRenderer){
			m_EffectRenderer = this.gameObject.AddComponent<LineRenderer>();
		}
		m_EffectRenderer.enabled = false;
	}

	override public void OnUpdate(){
		UpdateEffectPositions ();
	}

	override public void OnEndEffect(){
		Destroy (m_EffectRenderer);
	}

	public void InitializeEffect(Transform targetTransform){
		m_TargetTransform = targetTransform;
		StartEffect ();
	}

	public void InitializeEffect(Vector3 maxRayHitPosition){
		m_EndPosition = maxRayHitPosition;
		StartEffect ();
	}

	private void StartEffect(){
		UpdateEffectPositions ();

		m_EffectRenderer.enabled = true;
		m_EffectRenderer.material = m_EffectMaterial;
		m_EffectRenderer.SetColors (m_StartColor, m_EndColor);
		m_EffectRenderer.SetWidth (m_EffectStartWidth, m_EffectEndWidth);

		this.enabled = true;
	}

	private void UpdateEffectPositions(){
		if(null != m_EffectRenderer){
			m_EffectRenderer.SetPosition (0, CalculateStartPosition());
			m_EffectRenderer.SetPosition (1, CalculateEndPosition());
		}
	}

	private Vector3 CalculateStartPosition(){
		return this.transform.position + m_StartOffset;
	}

	private Vector3 CalculateEndPosition(){
		if(null != m_TargetTransform){
			m_EndPosition = m_TargetTransform.position + m_EndOffset;
		}
		return m_EndPosition;
	}
}
