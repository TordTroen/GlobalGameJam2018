using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public Transform InitialTransmitterTransform { get; private set; }

	public void OnStartLevel()
	{
		InitialTransmitterTransform = GameObject.FindGameObjectWithTag(Tags.InitialTransmission).transform;
	}
}
