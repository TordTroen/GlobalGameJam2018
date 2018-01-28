using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTool : Tool
{
	public override PlaceState OnPlace(RaycastHit2D hit)
	{
		// TODO Raycast and hit tool. if hit tool make that tools owner the same as this tool
//		var hit = ReferenceManager.Instance.ToolManager.RaycastFromCamToMouse();
//		print("PlaceHit: " + hit.collider);
		if (hit.collider != null)
		{
			var tool = hit.collider.GetComponent<Tool>();
			if (tool != null)
			{
				tool.OwnerPlayer = OwnerPlayer;
				return PlaceState.DestroyAndAction;
			}
		}
		return PlaceState.DestroyNoAction;
	}
}
