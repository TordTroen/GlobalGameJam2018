using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionController : MonoBehaviour
{
    [SerializeField]private GameObject m_transmissionEdgePrefab;
	private Transform m_initialTransmission;
    [SerializeField]private float m_transmissionRate = 1f;

    private void Awake()
    {
//		m_initialTransmission = GameObject.FindGameObjectWithTag(Tags.InitialTransmission).transform;
		m_initialTransmission = ReferenceManager.Instance.GameFlowController.CurrentLevel.InitialTransmitterTransform;
    }

    private void Start()
    {
		InvokeRepeating("DoInitialBeam", 0f, m_transmissionRate);
    }

    private void DoInitialBeam()
    {
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
