using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransmissionReceiver : Tool
{
	public override void OnHitByTransmission(Vector2 transmissionOrigin, Vector2 transmissionDirection, Vector2 endPoint, List<Tool> visitedTools, TransmissionReflecter originReflecter)
	{
		base.OnHitByTransmission(transmissionOrigin, transmissionDirection, endPoint, visitedTools, originReflecter);
		print("Player " + originReflecter.OwnerPlayer + " wins!");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
