using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolState
{
	None,
	Held,
	Placed,
	Committed
}

public enum PlaceState
{
	Place,
	DestroyNoAction,
	DestroyAndAction
}

public class ToolManager : MonoBehaviour
{
	private Tool m_currentTool;
	public Tool CurrentTool { get { return m_currentTool; } }
	[SerializeField]private ToolState m_toolState = ToolState.None;
	public ToolState CurrentToolState { private set { m_toolState = value; } get { return m_toolState; } }
	[SerializeField]private Transform m_floor;
	private int m_toolCount; // Only used for debugging purposes really

	private void Update()
	{
		switch (CurrentToolState)
		{
			case ToolState.Held:
				ToolFollowMousePos(m_currentTool);
				if (Input.GetButtonDown("Fire1"))
				{
					var hit = RaycastFromCamToMouse();
//					if (hit.collider != null && hit.collider.tag == Tags.Floor)
//					{
//						bool isValidPlacement = true;
						PlaceCurrentTool(hit);
//					}
				}
				break;
			case ToolState.Placed:
				ToolLookAtMouse(m_currentTool);
				if (Input.GetButtonDown("Fire1"))
				{
					CommitCurrentTool();
				}
				break;
			default:
				break;
		}
	}

	public RaycastHit2D RaycastFromCamToMouse()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var rayDist = 20f;
		Debug.DrawRay(ray.origin, ray.direction * rayDist, Color.red, 10f);
		return Physics2D.Raycast(ray.origin, ray.direction, rayDist, LayerMasks.Everything, -1f, 2f);
	}

	public void PickupTool(GameObject toolPrefab)
	{
		if (CurrentTool == null)
		{
			var obj = Instantiate(toolPrefab);
			obj.transform.SetParent(ReferenceManager.Instance.GameFlowController.CurrentLevel.transform);
			obj.name = obj.name.Replace("(Clone)", "") + m_toolCount++;
			var tool = obj.GetComponent<Tool>();
			m_currentTool = tool;
			CurrentToolState = ToolState.Held;
			m_currentTool.OnPickUp(ReferenceManager.Instance.GameFlowController.CurrentPlayer);
		}
	}

	public void PlaceCurrentTool(RaycastHit2D hit)
	{
//		if (isValidPlacement)
//		{
			var placeState = m_currentTool.OnPlace(hit);
			if (placeState == PlaceState.Place)
			{
				CurrentToolState = ToolState.Placed;
				var pos = m_currentTool.transform.position;
				pos.z = 0f;
				m_currentTool.transform.position = pos;
			}
			else
			{
				ReleaseCurrentTool();
				if (placeState == PlaceState.DestroyAndAction)
				{
					ReferenceManager.Instance.GameFlowController.EndCurrentTurn();
				}
			}
//		}
//		else
//		{
//			if (m_currentTool != null)
//			{
//				Destroy(m_currentTool.gameObject);
//				m_currentTool = null;
//			}
//			CurrentToolState = ToolState.None;
//		}
	}

	public void ReleaseCurrentTool()
	{
		if (m_currentTool != null)
		{
			Destroy(m_currentTool.gameObject);
			m_currentTool = null;
		}
		CurrentToolState = ToolState.None;
	}

	public void CommitCurrentTool()
	{
		m_currentTool = null;
		CurrentToolState = ToolState.None;

		ReferenceManager.Instance.GameFlowController.EndCurrentTurn();
	}

	private void ToolFollowMousePos(Tool tool)
	{
		Vector3 pos = Utils.GetMouseInWorldPos();
		pos.z = 10f;
		tool.transform.position = pos;
	}

	private void ToolLookAtMouse(Tool tool)
	{
		tool.transform.rotation = Utils.GetTopDownZRotation(tool.transform.position, Utils.GetMouseInWorldPos(), -90f);
	}
}
