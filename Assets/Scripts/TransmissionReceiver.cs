using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransmissionReceiver : Tool
{
	public override void OnHitByTransmission(Vector2 transmissionOrigin, Vector2 transmissionDirection, Vector2 endPoint, List<Tool> visitedTools, TransmissionReflecter originReflecter)
	{
		base.OnHitByTransmission(transmissionOrigin, transmissionDirection, endPoint, visitedTools, originReflecter);

		if (ReferenceManager.Instance.ToolManager.CurrentToolState == ToolState.None)
		{
			var winningPlayer = originReflecter.OwnerPlayer;
			// unload level
			// show win screen
			ReferenceManager.Instance.GameFlowController.WinGame(winningPlayer);

		}
	}
}
