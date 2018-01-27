using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
	public static ReferenceManager Instance { private set; get; }

	[SerializeField]private GameObject m_gameFlowObject;
	[SerializeField]private GameObject m_transmissionControllerObject;
	[SerializeField]private GameObject m_menuObject;
	public GameFlowController GameFlowController { private set; get; }
	public ToolManager ToolManager { private set; get; }
	public TransmissionController TransmissionController { private set; get; }
	public IngameMenuManager IngameMenuManager { private set; get; }

	private void Awake()
	{
		Instance = this;

		GameFlowController = m_gameFlowObject.GetComponent<GameFlowController>();
		ToolManager = m_gameFlowObject.GetComponent<ToolManager>();
		TransmissionController = m_transmissionControllerObject.GetComponent<TransmissionController>();
		IngameMenuManager = m_menuObject.GetComponent<IngameMenuManager>();
	}
}
