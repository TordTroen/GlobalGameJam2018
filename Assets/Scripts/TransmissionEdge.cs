using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionEdge : MonoBehaviour
{
    [SerializeField]private float m_spawnSpeed = 1f;
    [SerializeField]private float m_moveSpeed = 1f;
    private Vector2 m_start;
    private Vector2 m_end;
    [SerializeField]private GameObject m_transmissionEffectPrefab;
    [SerializeField]private List<TransmissionEffect> m_effectPool = new List<TransmissionEffect>();
	private LineRenderer m_lineRenderer;

    private float m_spawnTimer;

	private void Awake()
	{
		m_lineRenderer = GetComponent<LineRenderer>();
	}
//    private void Update()
//    {
//        m_spawnTimer -= Time.deltaTime;
//        if (m_spawnTimer <= 0f)
//        {
//            m_spawnTimer = m_spawnSpeed;
//            SpawnEffect(m_start, m_end);
//        }
//    }

    public void StartTransmission(Vector2 start, Vector2 end)
    {
        m_start = start;
        m_end = end;

		m_lineRenderer.SetPositions(new Vector3[]{start, end});
    }

    private void SpawnEffect(Vector2 start, Vector3 end)
    {
        TransmissionEffect effect = null;
        if (m_effectPool.Count == 0)
        {
            var obj = Instantiate(m_transmissionEffectPrefab);
            obj.transform.SetParent(transform);
            effect = obj.GetComponent<TransmissionEffect>();
        }
        else
        {
            int lastIndex = m_effectPool.Count - 1;
            effect = m_effectPool[lastIndex];
            m_effectPool.RemoveAt(lastIndex);
        }

        effect.Fire(start, end, m_moveSpeed, this);
        effect.gameObject.SetActive(true);
    }

    public void AddEffectBackInPool(TransmissionEffect effect)
    {
        effect.gameObject.SetActive(false);
        m_effectPool.Add(effect);
    }
}
