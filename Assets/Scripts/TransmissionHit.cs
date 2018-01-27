using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionHit
{
	public Vector2 EndPos { private set; get; }
	public Tool HitTool { private set; get; }

	public TransmissionHit(Vector2 endPos, Tool hitTool)
	{
		EndPos = endPos;
		HitTool = hitTool;
	}
}
