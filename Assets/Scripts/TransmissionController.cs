using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionController : MonoBehaviour
{
    [SerializeField]private GameObject m_transmissionEdgePrefab;
	[SerializeField]private Transform m_initialTransmission;
    [SerializeField]private float m_transmissionRate = 1f;
    
    private List<Vector3> m_positions = new List<Vector3>();

    [SerializeField]private LineRenderer m_lineRenderer;

    private Vector2 m_lastPos;
    bool hasFirstPos = false;

    [SerializeField]private LayerMask m_transmissionHitMask;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
		InvokeRepeating("DoInitialBeam", 0f, m_transmissionRate);
    }

    private void DoInitialBeam()
    {
		TransmitBeam(m_initialTransmission.position, m_initialTransmission.up, 100f, new List<Tool>(), null);
//        RaycastHit2D hit = Physics2D.Raycast(m_initialTransmission.position, m_initialTransmission.up, 100f, m_transmissionHitMask);
//        Debug.DrawRay(m_initialTransmission.position, m_initialTransmission.up * 100f, Color.red, 1f);
//        if (hit.collider != null)
//        {
//            var reflecter = hit.collider.GetComponent<TransmissionReflecter>();
//            if (reflecter != null)
//            {
//                reflecter.OnHitByTransmission( );
//            }
//        }
    }

	public static TransmissionHit TransmitBeam(Vector2 start, Vector2 direction, float distance, List<Tool> visitedTools, TransmissionReflecter originReflecter)
	{
		RaycastHit2D hit = Physics2D.Raycast(start, direction, distance);
		var endPos = Vector2.zero;
		var hitDist = Mathf.Min(distance, hit.distance);
		Tool hitTool = null;
		//Debug.DrawRay(start, direction * hitDist, Color.red, 1f);
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
