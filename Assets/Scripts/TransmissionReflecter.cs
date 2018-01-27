using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionReflecter : MonoBehaviour
{
    [SerializeField]private float m_distance = 100f;
	[SerializeField]private LayerMask m_transmissionHitMask;
	[SerializeField]private GameObject m_transmissionEdgePrefab;
	[SerializeField]private List<TransmissionEdge> m_edges = new List<TransmissionEdge>();

	public virtual void OnHitByTransmission(Vector2 transmissionOrigin, Vector2 transmissionDirection, Vector2 endPoint)
    {
		foreach (var edge in m_edges)
		{
			edge.gameObject.SetActive(false);
		}

        var direction = transform.up;
        ReflectBeam(direction);
//		CreateTransmissionEdge(transmissionOrigin, transform.position);
    }

    protected void ReflectBeam(Vector2 direction)
    {
		Transmit(transform.position, direction, m_distance);
//        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, m_distance, m_transmissionHitMask);
//        Debug.DrawRay(transform.position, direction * m_distance, Color.red, 1f);
//        if (hit.collider != null)
//        {
//            var reflecter = hit.collider.GetComponent<TransmissionReflecter>();
//            if (reflecter != null)
//            {
//                reflecter.OnHitByTransmission();
//            }
//        }
    }

	protected void Transmit(Vector2 start, Vector2 direction, float distance)
	{
		// Hit next reflecter
//		RaycastHit2D hit = Physics2D.Raycast(start, direction, distance);
//		var endPos = Vector2.zero;
//		var hitDist = Mathf.Min(distance, hit.distance);
//		Debug.DrawRay(start, direction * hitDist, Color.red, 1f);
//		if (hit.collider == null)
//		{
//			endPos = start + direction * hitDist;
//		}
//		else
//		{
//			var reflecter = hit.collider.GetComponent<TransmissionReflecter>();
//			if (reflecter == null)
//			{
//				endPos = hit.point;
//			}
//			else
//			{
//				reflecter.OnHitByTransmission(start, direction, hit.point);
//				endPos = reflecter.transform.position;
//			}
//
//		}
		var hit = TransmissionController.TransmitBeam(start, direction, distance);

		// Create or reuse edge
		TransmissionEdge edgeToUse = null;
		foreach (var edge in m_edges)
		{
			if (!edge.gameObject.activeInHierarchy)
			{
				edgeToUse = edge;
				edgeToUse.gameObject.SetActive(true);
			}
		}
		if (edgeToUse == null)
		{
			var obj = Instantiate(m_transmissionEdgePrefab);
			edgeToUse = obj.GetComponent<TransmissionEdge>();
			m_edges.Add(edgeToUse);
			
		}
		edgeToUse.StartTransmission(start, hit.EndPos);
	}
}
