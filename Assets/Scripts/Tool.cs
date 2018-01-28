using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
	[SerializeField]private Sprite[] m_playerSprites = new Sprite[2];
	public Sprite[] PlayerSprites { get { return m_playerSprites; } }
	private SpriteRenderer m_spriteRenderer;
	private PlayerInfo m_ownerPlayer;
	public PlayerInfo OwnerPlayer { get { return m_ownerPlayer; } set { m_ownerPlayer = value; UpdateGraphics(); } }

	protected virtual void Awake()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public virtual void OnPickUp(PlayerInfo pickedUpBy)
	{
		OwnerPlayer = pickedUpBy;
	}

	public virtual PlaceState OnPlace(RaycastHit2D hit)
	{
		return PlaceState.Place;
	}

	public void UpdateGraphics()
	{
		if (m_spriteRenderer)
		{
			m_spriteRenderer.sprite = PlayerSprites[OwnerPlayer.PlayerId];
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
