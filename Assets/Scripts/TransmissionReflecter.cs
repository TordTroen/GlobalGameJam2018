using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionReflecter : Tool
{
    [SerializeField]private float m_distance = 100f;
	[SerializeField]private LayerMask m_transmissionHitMask;
	[SerializeField]private GameObject m_transmissionEdgePrefab;
	[SerializeField]private List<TransmissionEdge> m_edges = new List<TransmissionEdge>();
	private TransmissionReflecter m_prevReflecter;

	public override void OnHitByTransmission(Vector2 transmissionOrigin, Vector2 transmissionDirection, Vector2 endPoint, TransmissionReflecter originReflecter = null)
    {
		base.OnHitByTransmission(transmissionOrigin, transmissionDirection, endPoint, originReflecter);
		if (m_prevReflecter != null && m_prevReflecter != originReflecter)
		{
			print(string.Format("[{0}] Already hit! (prev: {1}, origin: {2})", this, m_prevReflecter, originReflecter));
			return;
		}
		m_prevReflecter = originReflecter;

		foreach (var edge in m_edges)
		{
			edge.gameObject.SetActive(false);
		}

        var direction = transform.up;
        ReflectBeam(direction);
    }

    protected void ReflectBeam(Vector2 direction)
    {
		Transmit(transform.position, direction, m_distance);
    }

	protected void Transmit(Vector2 start, Vector2 direction, float distance)
	{
		var hit = TransmissionController.TransmitBeam(start, direction, distance, this);

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
			obj.transform.SetParent(transform);
			edgeToUse = obj.GetComponent<TransmissionEdge>();
			m_edges.Add(edgeToUse);
			
		}
		edgeToUse.StartTransmission(start, hit.EndPos, OwnerPlayer == null ? Color.green : OwnerPlayer.PlayerColor);
	}
}
