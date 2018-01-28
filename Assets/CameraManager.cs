using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private Camera cam;
	public float m_fitSize = 7f;

	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	[ContextMenu("Test")]
	void Test()
	{
		FitCameraTo(GameObject.FindGameObjectWithTag(Tags.Floor));
	}

	public void FitCameraTo(GameObject obj)
	{
		if (cam == null)
		{
			cam = GetComponent<Camera>();
		}
		var targetX = m_fitSize;//obj.transform.localScale.x;
		var targetY = m_fitSize;//obj.transform.localScale.y;

		float screenRatio = (float)Screen.width / (float)Screen.height;
		float targetRatio = targetX / targetY;

		if (screenRatio >= targetRatio)
		{
			cam.orthographicSize = targetY / 2;
		}
		else
		{
			float differenceInSize = targetRatio / screenRatio;
			cam.orthographicSize = targetY / 2 * differenceInSize;
		}

		//transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, -1f)
	}
}
