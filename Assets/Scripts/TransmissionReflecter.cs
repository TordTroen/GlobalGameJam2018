using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionReflecter : Tool
{
    [SerializeField]private float m_distance = 100f;
	[SerializeField]private LayerMask m_transmissionHitMask;
	[SerializeField]private GameObject m_transmissionEdgePrefab;
	[SerializeField]private Transform m_transmissionOrigin;
	[SerializeField]private List<TransmissionEdge> m_edges = new List<TransmissionEdge>();
	private TransmissionReflecter m_prevReflecter;
	private TransmissionReflecter m_nextReflecter;

	protected override void Awake()
	{
		base.Awake();
		if (m_transmissionOrigin == null)
		{
			m_transmissionOrigin = transform;
		}
	}

	public override void OnHitByTransmission(Vector2 transmissionOrigin, Vector2 transmissionDirection, Vector2 endPoint, List<Tool> visitedTools, TransmissionReflecter originReflecter = null)
    {
		if (visitedTools.Contains(this as Tool))
		{
			print("Already visited this tool");
			return;
		}
		base.OnHitByTransmission(transmissionOrigin, transmissionDirection, endPoint, visitedTools, originReflecter);
		m_prevReflecter = originReflecter;

		DisableOutgoingEdges();

        var direction = transform.up;

		Transmit(m_transmissionOrigin.position, direction, m_distance, visitedTools, transform.position);
    }
		
	protected void Transmit(Vector2 transmissionStart, Vector2 direction, float distance, List<Tool> visitedTools, Vector2 visualStart)
	{
		var hit = TransmissionController.TransmitBeam(transmissionStart, direction, distance, visitedTools, gameObject);
		TransmissionReflecter reflecterHit = null;
		if (hit.HitTool is TransmissionReflecter)
		{
			reflecterHit = hit.HitTool as TransmissionReflecter;
		}
		if (reflecterHit != null && reflecterHit == m_prevReflecter)
		{
			print("Danger high voltage");
			return;
		}
//		if (visitedTools.Contains(hit.HitTool))
//		{
//			print("Looping chain");
//			return;
//		}
		if (m_nextReflecter != null && m_nextReflecter != reflecterHit)
		{
			print("Chain broken!");
//			BreakTheChain();
			foreach (var next in GetChain())
			{
				(next as TransmissionReflecter).DisableOutgoingEdges();
			}
		}
		m_nextReflecter = reflecterHit;

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
		edgeToUse.StartTransmission(visualStart, hit.EndPos, OwnerPlayer == null ? Color.gray : OwnerPlayer.PlayerColor);
	}

	public void DisableOutgoingEdges()
	{
		foreach (var edge in m_edges)
		{
			edge.gameObject.SetActive(false);
		}
	}

	public List<Tool> GetChain()
	{
		var chain = new List<Tool>();
		var next = m_nextReflecter;
		while (next != null)
		{
			if (chain.Contains(next))
			{
				print("Already have this in chain");
				break;
			}
			chain.Add(next);
			next = next.m_nextReflecter;
		}
		return chain;
	}
}
