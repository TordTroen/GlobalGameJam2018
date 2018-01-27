﻿using System.Collections;
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
	[SerializeField]private ToolState m_toolState = ToolState.None;
	public ToolState CurrentToolState { private set { m_toolState = value; } get { return m_toolState; } }

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
					var hit = Physics2D.Raycast(ray.origin, ray.direction, rayDist);
					print("Hit: " + hit.collider);
					if (hit.collider != null)
					{
						bool isValidPlacement = true;
						PlaceCurrentTool(isValidPlacement);
					}
				}
				break;
			case ToolState.Placed:
				ToolLookAtMouse(m_currentTool);
				break;
			default:
				break;
		}
	}

	public void PickupTool(GameObject toolPrefab)
	{
		var obj = Instantiate(toolPrefab);
		var tool = obj.GetComponent<Tool>();
		m_currentTool = tool;
		CurrentToolState = ToolState.Held;
	}

	public void PlaceCurrentTool(bool isValidPlacement)
	{
		if (isValidPlacement)
		{
			CurrentToolState = ToolState.Placed;
		}
		else
		{
			m_currentTool = null;
			CurrentToolState = ToolState.None;
		}
	}

	public void CommitCurrentTool()
	{
		m_currentTool = null;
		CurrentToolState = ToolState.None;
	}

	private void ToolFollowMousePos(Tool tool)
	{
		tool.transform.position = Utils.GetMouseInWorldPos();
	}

	private void ToolLookAtMouse(Tool tool)
	{
		tool.transform.rotation = Utils.GetTopDownZRotation(tool.transform.position, Utils.GetMouseInWorldPos(), -90f);
	}
}
