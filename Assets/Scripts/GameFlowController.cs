using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
	[SerializeField]private PlayerInfo[] m_players = new PlayerInfo[2];
	[SerializeField]private float m_secondsPerTurn = 30f;
	private int m_currentPlayerId = 1;
	public int CurrentPlayerId { get { return m_currentPlayerId; } }
	public PlayerInfo CurrentPlayer { get { return m_players[m_currentPlayerId]; } }
	private float m_turnTimer;
	private ToolManager m_toolManager;
	public Level CurrentLevel { get; set; }
	public Level[] AllLevels { get; private set; }

	[SerializeField]private Image m_timerImage;
	[SerializeField]private Text m_timerText;

	private void Awake()
	{
		m_toolManager = ReferenceManager.Instance.ToolManager;
		LoadAllLevelsFromDisk();
		for (int playerId = 0; playerId < m_players.Length; playerId++)
		{
			m_players[playerId].PlayerId = playerId;
		}
	}

	private void Start()
	{
		SetCurrentPlayer(0 ,false);
		SetPlayerOverlaysActive(false);
	}

	private void Update()
	{
		if (CurrentLevel != null)
		{
			m_turnTimer -= Time.deltaTime;
			UpdateTimerGraphics();
			if (m_turnTimer <= 0f)
			{
				EndCurrentTurn();
			}
		}
	}

	private void LoadAllLevelsFromDisk()
	{
		var levelObjects = Resources.LoadAll("Levels", typeof(Level));
		AllLevels = new Level[levelObjects.Length];
		for (int levelIndex = 0; levelIndex < levelObjects.Length; levelIndex++)
		{
			AllLevels[levelIndex] = levelObjects[levelIndex] as Level;
			AllLevels[levelIndex].LevelName = string.Format("Level {0}", levelIndex + 1);
		}
	}

	private void UpdateTimerGraphics()
	{
		m_timerText.text = string.Format("{0:00}s", m_turnTimer);
		m_timerImage.fillAmount = m_turnTimer / m_secondsPerTurn;
	}

	public void EndCurrentTurn()
	{
		if (m_toolManager.CurrentToolState == ToolState.Held)
		{
			m_toolManager.ReleaseCurrentTool();
		}
		else if (m_toolManager.CurrentToolState == ToolState.Placed)
		{
			m_toolManager.CommitCurrentTool();
		}

//		print(string.Format("Player {0} turn done!", CurrentPlayer.PlayerName));
		NextPlayer();
		// TODO if you have placed box, commit it at current rotation
		// TODO visual feedback
	}

	public void NextPlayer()
	{
		var nextPlayerId = (m_currentPlayerId + 1) % 2;
		SetCurrentPlayer(nextPlayerId, true);
	}

	public void SetCurrentPlayer(int id, bool notify)
	{
		CurrentPlayer.SelectedPlayerOverlay.SetActive(false);
		m_currentPlayerId = id;
		m_turnTimer = m_secondsPerTurn;
		CurrentPlayer.SelectedPlayerOverlay.SetActive(true);
		m_timerImage.color = CurrentPlayer.PlayerColor;
		if (notify)
		{
			ReferenceManager.Instance.IngameMenuManager.NotifyPlayerTurn(CurrentPlayer);
		}
	}

	public void UnloadCurrentLevel()
	{
		ReferenceManager.Instance.TransmissionController.StopTransmission();
		ReferenceManager.Instance.ToolManager.ReleaseCurrentTool();
		if (CurrentLevel != null)
		{
			Destroy(CurrentLevel.gameObject);
			CurrentLevel = null;
		}
		SetPlayerOverlaysActive(false);
	}

	public void SetPlayerOverlaysActive(bool active)
	{
		foreach (var player in m_players)
		{
			player.SelectedPlayerOverlay.SetActive(active);
		}
	}

	public void WinGame(PlayerInfo winningPlayer)
	{
		winningPlayer.Score ++;
		UnloadCurrentLevel();
		ReferenceManager.Instance.IngameMenuManager.GoToWinScreen(winningPlayer);
	}

	public void StartLevel(Level level)
	{
		CurrentLevel = level;
		SetCurrentPlayer(0, true);
	}
}
