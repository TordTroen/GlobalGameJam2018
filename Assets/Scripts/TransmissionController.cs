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
		TransmitBeam(m_initialTransmission.position, m_initialTransmission.up, 100f, new List<Tool>(), gameObject);
    }

	public static TransmissionHit TransmitBeam(Vector2 start, Vector2 direction, float distance, List<Tool> visitedTools, GameObject origin)
	{
//		print("Transmitting " + direction);
		float hitDist = distance;
		bool hasHit = false;
		var originCollider = origin.GetComponent<Collider2D>();
		RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance, LayerMasks.Everything, -0.5f, 0.5f);
		RaycastHit2D hit = new RaycastHit2D();
		if (hits.Length != 0)
		{
			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].collider != originCollider)
				{
					hit = hits[i];
					hasHit = true;
					break;
				}
			}
		}

		var endPos = Vector2.zero;
//		var hitDist = Mathf.Min(distance, hit.distance);
		Tool hitTool = null;
		if (hasHit)
		{
			hitTool = hit.collider.GetComponent<Tool>();
			if (hitTool == null)
			{
				endPos = hit.point;
			}
			else
			{
				var originReflecter = origin.GetComponent<TransmissionReflecter>();
				hitTool.OnHitByTransmission(start, direction, hit.point, visitedTools, originReflecter);
				endPos = hitTool.transform.position;
			}
		}
		else
		{
			endPos = start + direction * distance;
		}
//		Debug.DrawRay(start, endPos - start, Color.green, 1f);
		return new TransmissionHit(endPos, hitTool);
	}
}
