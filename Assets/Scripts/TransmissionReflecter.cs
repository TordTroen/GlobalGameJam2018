using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionReflecter : MonoBehaviour
{
    [SerializeField]private float m_distance = 100f;
    [SerializeField]private LayerMask m_transmissionHitMask;

    public virtual void OnHitByTransmission()
    {
        var direction = transform.up;
        ReflectBeam(direction);
    }

    protected void ReflectBeam(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, m_distance, m_transmissionHitMask);
        Debug.DrawRay(transform.position, direction * m_distance, Color.red, 1f);
        if (hit.collider != null)
        {
            var reflecter = hit.collider.GetComponent<TransmissionReflecter>();
            if (reflecter != null)
            {
                reflecter.OnHitByTransmission();
            }
        }
    }
}
