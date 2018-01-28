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
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					var rayDist = 20f;
					Debug.DrawRay(ray.origin, ray.direction * rayDist, Color.red, 10f);
					var hit = Physics2D.Raycast(ray.origin, ray.direction, rayDist, 1, -1f, 2f);
					print(hit.collider);
					if (hit.collider != null && hit.collider.tag == Tags.Floor)
					{
						bool isValidPlacement = true;
						PlaceCurrentTool(isValidPlacement);
					}
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

	public void PickupTool(GameObject toolPrefab)
	{
		var obj = Instantiate(toolPrefab);
		obj.transform.SetParent(ReferenceManager.Instance.GameFlowController.CurrentLevel.transform);
		obj.name = "Reflecter_" + m_toolCount++;
		var tool = obj.GetComponent<Tool>();
		m_currentTool = tool;
		CurrentToolState = ToolState.Held;
		m_currentTool.OnPickUp(ReferenceManager.Instance.GameFlowController.CurrentPlayer);
	}

	public void PlaceCurrentTool(bool isValidPlacement)
	{
		if (isValidPlacement)
		{
			CurrentToolState = ToolState.Placed;
			var pos = m_currentTool.transform.position;
			pos.z = 0f;
			m_currentTool.transform.position = pos;
		}
		else
		{
			Destroy(m_currentTool.gameObject);
			m_currentTool = null;
			CurrentToolState = ToolState.None;
		}
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
