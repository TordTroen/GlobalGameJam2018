using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionController : MonoBehaviour
{
	private Transform m_initialTransmission;
    [SerializeField]private float m_transmissionRate = 1f;
	private float m_transmissionTimer;
	private bool m_transmitInitial = false;

    private void Awake()
    {
//		m_initialTransmission = GameObject.FindGameObjectWithTag(Tags.InitialTransmission).transform;
    }

	private void Update()
	{
		if (m_transmitInitial)
		{
			m_transmissionTimer -= Time.deltaTime;
			if (m_transmissionTimer <= 0f)
			{
				TransmitInitialBeam();
			}
		}
	}

	public void StartInitialTransmission(Transform initialTransmissionTransform)
    {
		m_initialTransmission = initialTransmissionTransform;
//		InvokeRepeating("DoInitialBeam", 0f, m_transmissionRate);
		m_transmitInitial = true;
    }

	public void StopTransmission()
	{
		m_transmitInitial = false;
	}

    private void TransmitInitialBeam()
    {
		m_transmissionTimer = m_transmissionRate;
		TransmitBeam(m_initialTransmission.position, m_initialTransmission.up, 100f, new List<Tool>(), null);
    }

	public static TransmissionHit TransmitBeam(Vector2 start, Vector2 direction, float distance, List<Tool> visitedTools, TransmissionReflecter originReflecter)
	{
		RaycastHit2D hit = Physics2D.Raycast(start, direction, distance);
		var endPos = Vector2.zero;
		var hitDist = Mathf.Min(distance, hit.distance);
		Tool hitTool = null;
		if (hit.collider == null)
		{
			endPos = start + direction * hitDist;
		}
		else
		{
			hitTool = hit.collider.GetComponent<Tool>();
			if (hitTool == null)
			{
				endPos = hit.point;
			}
			else
			{
				hitTool.OnHitByTransmission(start, direction, hit.point, visitedTools, originReflecter);
				endPos = hitTool.transform.position;
			}
		}
		return new TransmissionHit(endPos, hitTool);
	}
}
