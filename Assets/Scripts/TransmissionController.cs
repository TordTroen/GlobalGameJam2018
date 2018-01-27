using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionController : MonoBehaviour
{
    [SerializeField]private GameObject m_transmissionEdgePrefab;
    
    private List<Vector3> m_positions = new List<Vector3>();

    [SerializeField]private LineRenderer m_lineRenderer;

    private Vector2 m_lastPos;
    bool hasFirstPos = false;

    [SerializeField]private LayerMask m_transmissionHitMask;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            var hitPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hitPos.z = -0.01f;
            if (hasFirstPos)
            {
                AddPoint(hitPos, m_lastPos);
                //m_positions.Add(hitPos);
                //UpdatePositions();
            }
            else
            {
                hasFirstPos = true;
            }
            m_lastPos = hitPos;
        }
    }

    private void RaycastInDirection(Vector2 start, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(start, direction, 100f, m_transmissionHitMask);
        if (hit.collider != null)
        {
            
        }
    }

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
