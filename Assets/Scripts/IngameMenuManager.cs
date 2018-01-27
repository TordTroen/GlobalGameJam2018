using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenuManager : MonoBehaviour
{
	[SerializeField]private GameObject m_playerPanel;
	[SerializeField]private GameObject m_levelSelectPanel;
	[SerializeField]private GameObject m_gameHudPanel;
	[SerializeField]private GameObject m_winPanel;
	[SerializeField]private GameObject m_stalematePanel;

	[SerializeField]private GameObject m_levelGridParent;

	private void Start()
	{
		InitLevelSelect();
	}

	public void GoToLevelSelect()
	{
		
	}

	private void InitLevelSelect()
	{
		foreach (var level in ReferenceManager.Instance.GameFlowController.AllLevels)
		{
			
		}
	}
}
