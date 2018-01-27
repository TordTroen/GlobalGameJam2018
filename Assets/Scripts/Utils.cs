using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static Quaternion GetTopDownZRotation(Vector2 direction, float offest = 0f)
	{
		var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offest;
		return Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public static Quaternion GetTopDownZRotation(Vector2 start, Vector2 end, float offset = 0f)
	{
		var direction = end - start;
		return GetTopDownZRotation(direction, offset);
	}

	public static Vector2 GetMouseInWorldPos()
	{
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		return mousePos;
	}
}
