using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionHit
{
	public Vector2 EndPos { private set; get; }
	public TransmissionReflecter HitReflecter { private set; get; }

	public TransmissionHit(Vector2 endPos, TransmissionReflecter hitReflecter)
	{
		EndPos = endPos;
		HitReflecter = hitReflecter;
	}
}
