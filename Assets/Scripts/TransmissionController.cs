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

//    private void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            var hitPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            hitPos.z = -0.01f;
//            if (hasFirstPos)
//            {
//                AddPoint(hitPos, m_lastPos);
//                //m_positions.Add(hitPos);
//                //UpdatePositions();
//            }
//            else
//            {
//                hasFirstPos = true;
//            }
//            m_lastPos = hitPos;
//        }
//    }

    private void DoInitialBeam()
    {
		TransmitBeam(m_initialTransmission.position, m_initialTransmission.up, 100f);
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

	public static TransmissionHit TransmitBeam(Vector2 start, Vector2 direction, float distance)
	{
		RaycastHit2D hit = Physics2D.Raycast(start, direction, distance);
		var endPos = Vector2.zero;
		var hitDist = Mathf.Min(distance, hit.distance);
		TransmissionReflecter hitReflecter = null;
		//Debug.DrawRay(start, direction * hitDist, Color.red, 1f);
		if (hit.collider == null)
		{
			endPos = start + direction * hitDist;
		}
		else
		{
			hitReflecter = hit.collider.GetComponent<TransmissionReflecter>();
			if (hitReflecter == null)
			{
				endPos = hit.point;
			}
			else
			{
				hitReflecter.OnHitByTransmission(start, direction, hit.point);
				endPos = hitReflecter.transform.position;
			}
		}
		return new TransmissionHit(endPos, hitReflecter);
	}

//	public static void Transmit(Vector2 start, Vector2 direction, float distance, GameObject transmissionEdgePrefab)
//    {
//		RaycastHit2D hit = Physics2D.Raycast(start, direction, distance);
//        Debug.DrawRay(start, direction * distance, Color.red, 1f);
//        if (hit.collider != null)
//        {
//			var endPos = Vector2.zero;
//            var reflecter = hit.collider.GetComponent<TransmissionReflecter>();
//            if (reflecter == null)
//			{
//				var hitDist = Mathf.Min(distance, hit.distance);
//				endPos = start + direction * hitDist;
//			}
//			else
//            {
//				reflecter.OnHitByTransmission(start, direction, hit.point);
//				endPos = reflecter.transform.position;
//            }
//
//			var obj = Instantiate(transmissionEdgePrefab);
//			var edge = obj.GetComponent<TransmissionEdge>();
//			edge.StartTransmission(start, endPos);
//        }
//    }


    private void UpdatePositions()
    {
        print(string.Format("Poslen: {0}", m_positions.Count));
        m_lineRenderer.positionCount = m_positions.Count;
        m_lineRenderer.SetPositions(m_positions.ToArray());
    }

    public void AddPoint(Vector2 position, Vector2 prevPosition)
    {
        //m_positions.Add(position);
        // spawn new edge
        var obj = Instantiate(m_transmissionEdgePrefab);
        var edge = obj.GetComponent<TransmissionEdge>();
        edge.StartTransmission(prevPosition, position);
    }
}
