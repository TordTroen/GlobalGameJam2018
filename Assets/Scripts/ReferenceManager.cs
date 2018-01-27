using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
	public static ReferenceManager Instance { private set; get; }

	[SerializeField]private GameObject m_gameFlowObject;
	public GameFlowController GameFlowController { private set; get; }

	private void Awake()
	{
		Instance = this;

		GameFlowController = m_gameFlowObject.GetComponent<GameFlowController>();
	}
}
