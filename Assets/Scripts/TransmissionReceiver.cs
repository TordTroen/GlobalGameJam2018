using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionReceiver : Tool
{
    public override void OnHitByTransmission(Vector2 transmissionOrigin, Vector2 transmissionDirection, Vector2 endPoint, TransmissionReflecter originReflecter)
	{
		base.OnHitByTransmission(transmissionOrigin, transmissionDirection, endPoint, originReflecter);
		print("Player " + originReflecter.OwnerPlayer + " wins!");
	}
}
