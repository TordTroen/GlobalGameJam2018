using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	[SerializeField]private Animator m_playerTurnNotificationAnimator;
	[SerializeField]private Text m_playerTurnNotificationText;
	[SerializeField]private Image m_playerTurnNotificationImage;

	private void Start()
	{
		InitLevelSelect();
		GoToLevelSelect();
	}

	public void GoToLevelSelect()
	{
		m_levelSelectPanel.SetActive(true);
		m_gameHudPanel.SetActive(true);
		m_rightSidePanel.SetActive(false);
	}

	public void GoToLevelScreen()
	{
		m_gameHudPanel.SetActive(true);
		m_rightSidePanel.SetActive(true);
	}

	public void WinScreenToLevelSelect()
	{
		m_winPanel.SetActive(false);
		GoToLevelSelect();
	}

	public void GoToWinScreen(PlayerInfo winningPlayer)
	{
		m_winPanel.SetActive(true);
		m_gameHudPanel.SetActive(false);
		m_winText.text = string.Format("{0} wins!", winningPlayer.PlayerName);
		m_winText.color = winningPlayer.PlayerColor;
	}

	public void GoToStalemateScreen()
	{
		m_gameHudPanel.SetActive(false);
		m_stalematePanel.SetActive(true);
	}

	public void OnBackPressed()
	{
		if (m_rightSidePanel.activeInHierarchy) // In game
		{
			ReferenceManager.Instance.GameFlowController.UnloadCurrentLevel();
			GoToLevelSelect();
		}
		else if (m_levelSelectPanel.activeInHierarchy)
		{
			SceneManager.LoadScene(SceneNames.MainMenu);
		}
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
		ReferenceManager.Instance.GameFlowController.StartLevel(level);
	}

	public void NotifyPlayerTurn(PlayerInfo player)
	{
		m_playerTurnNotificationAnimator.SetTrigger("PlayNotification");
		m_playerTurnNotificationImage.color = player.PlayerColor;
		m_playerTurnNotificationText.text = string.Format("{0}s turn!", player.PlayerName);
	}
}
