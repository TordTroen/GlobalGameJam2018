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

	[SerializeField]private GameObject m_rightSidePanel;

	[SerializeField]private Text m_winText;

	private void Start()
	{
		InitLevelSelect();
		GoToLevelSelect();
	}

	public void GoToLevelSelect()
	{
		m_levelSelectPanel.SetActive(true);
		m_rightSidePanel.SetActive(false);
	}

	public void GoToLevelScreen()
	{
		m_rightSidePanel.SetActive(true);
	}

	public void WinScreenToLevelSelect()
	{
		m_winPanel.SetActive(false);
		m_levelSelectPanel.SetActive(true);
		m_rightSidePanel.SetActive(false);
	}

	public void GoToWinScreen(PlayerInfo winningPlayer)
	{
		m_winPanel.SetActive(true);
		m_winText.text = string.Format("{0} wins!", winningPlayer.PlayerName);
		m_winText.color = winningPlayer.PlayerColor;
	}

	public void GoToStalemateScreen()
	{
		m_stalematePanel.SetActive(true);
		m_rightSidePanel.SetActive(false);
	}

	public void GoBackFromLevel()
	{
		ReferenceManager.Instance.GameFlowController.UnloadCurrentLevel();
		GoToLevelSelect();
	}

	private void InitLevelSelect()
	{
		foreach (var levelAsset in ReferenceManager.Instance.GameFlowController.AllLevels)
		{
			var obj = Instantiate(m_levelGridItemPrefab);
			obj.GetComponent<Button>().onClick.AddListener(() => { OnLevelClick(levelAsset); });
			obj.transform.SetParent(m_levelGridParent);
			obj.GetComponent<LevelSelectItem>().Init(levelAsset.LevelName);
		}
	}

	private void OnLevelClick(Level levelAsset)
	{
		GoToLevelScreen();
		m_levelSelectPanel.SetActive(false);
		var levelObj = Instantiate(levelAsset);
		var level = levelObj.GetComponent<Level>();
		level.OnStartLevel();
		ReferenceManager.Instance.GameFlowController.CurrentLevel = level;
	}
}
