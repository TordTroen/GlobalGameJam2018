using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
	[SerializeField]private Sprite[] m_playerSprites = new Sprite[2];
	public Sprite[] PlayerSprites { get { return m_playerSprites; } }
	private SpriteRenderer m_spriteRenderer;
	private PlayerInfo m_ownerPlayer;
	public PlayerInfo OwnerPlayer { get { return m_ownerPlayer; } }

	protected virtual void Awake()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void OnPickUp(PlayerInfo pickedUpBy)
	{
		m_ownerPlayer = pickedUpBy;
		if (m_spriteRenderer)
		{
			m_spriteRenderer.sprite = PlayerSprites[ReferenceManager.Instance.GameFlowController.CurrentPlayerId];
		}
	}

	public virtual void OnHitByTransmission(
		Vector2 transmissionOrigin, 
		Vector2 transmissionDirection, 
		Vector2 endPoint,
		List<Tool> visitedTools,
		TransmissionReflecter originReflecter = null)
	{
//		print("Visited " + this + " (total " + visitedTools.Count + ")");
		visitedTools.Add(this);
	}
}
