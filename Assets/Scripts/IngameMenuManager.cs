using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameMenuManager : MonoBehaviour
{
	[SerializeField]private GameObject m_playerPanel;
	[SerializeField]private GameObject m_levelSelectPanel;
	[SerializeField]private GameObject m_gameHudPanel;
	[SerializeField]private GameObject m_winPanel;
	[SerializeField]private GameObject m_stalematePanel;

	[SerializeField]private Transform m_levelGridParent;
	[SerializeField]private GameObject m_levelGridItemPrefab;

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
			var obj = Instantiate(m_levelGridItemPrefab);
			obj.GetComponent<Button>().onClick.AddListener(() => { OnLevelClick(level); });
			obj.transform.SetParent(m_levelGridParent);
			obj.GetComponent<LevelSelectItem>().Init(level.LevelName);
		}
	}

	private void OnLevelClick(Level level)
	{
		print("Clicked level " + level.LevelName);
		m_levelSelectPanel.SetActive(false);
		var levelObj = Instantiate(level);
		level.OnStartLevel();
	}
}
