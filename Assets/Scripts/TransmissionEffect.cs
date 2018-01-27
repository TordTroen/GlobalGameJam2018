using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionEffect : MonoBehaviour
{
    private float m_speed;
    private TransmissionEdge m_owner;
    private Vector2 m_endPoint;

    private void Update()
    {
        //transform.Translate(dir * m_speed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, m_endPoint, m_speed);

        if (Vector2.Distance(transform.position, m_endPoint) < 0.1f)
        {
            m_owner.AddEffectBackInPool(this);
        }
    }

    public void Fire(Vector2 start, Vector2 end, float speed, TransmissionEdge owner)
    {
        m_speed = speed;
        m_endPoint = end;
        m_owner = owner;
        transform.position = start;

        var dir = end - start;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
