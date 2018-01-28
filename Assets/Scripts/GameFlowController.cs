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
		var levelObjects = Resources.LoadAll("Levels", typeof(Level));
		AllLevels = new Level[levelObjects.Length];
		for (int levelIndex = 0; levelIndex < levelObjects.Length; levelIndex++)
		{
			AllLevels[levelIndex] = levelObjects[levelIndex] as Level;
			AllLevels[levelIndex].LevelName = string.Format("Level {0}", levelIndex);
		}
	}

	private void Start()
	{
		NextPlayer();
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

	private void UpdateTimerGraphics()
	{
		m_timerText.text = string.Format("{0:00}s", m_turnTimer);
		m_timerImage.fillAmount = m_turnTimer / m_secondsPerTurn;
	}

	public void EndCurrentTurn()
	{
		if (m_toolManager.CurrentToolState == ToolState.Held)
		{
			m_toolManager.PlaceCurrentTool(false);
		}
		else if (m_toolManager.CurrentToolState == ToolState.Placed)
		{
			m_toolManager.CommitCurrentTool();
		}

		print(string.Format("Player {0} turn done!", CurrentPlayer.PlayerName));
		NextPlayer();
		// TODO if you have placed box, commit it at current rotation
		// TODO visual feedback
	}

	public void NextPlayer()
	{
		var nextPlayerId = (m_currentPlayerId + 1) % 2;
		SetCurrentPlayer(nextPlayerId);
//		CurrentPlayer.SelectedPlayerOverlay.SetActive(false);
//		m_currentPlayerId = (m_currentPlayerId + 1) % 2;
//		m_turnTimer = m_secondsPerTurn;
//		CurrentPlayer.SelectedPlayerOverlay.SetActive(true);
//		m_timerImage.color = CurrentPlayer.PlayerColor;
//		// TODO visual feedback like "Player X turn!"
	}

	public void SetCurrentPlayer(int id)
	{
		CurrentPlayer.SelectedPlayerOverlay.SetActive(false);
		m_currentPlayerId = id;
		m_turnTimer = m_secondsPerTurn;
		CurrentPlayer.SelectedPlayerOverlay.SetActive(true);
		m_timerImage.color = CurrentPlayer.PlayerColor;
		// TODO visual feedback like "Player X turn!"
	}

	public void UnloadCurrentLevel()
	{
		ReferenceManager.Instance.TransmissionController.StopTransmission();
		ReferenceManager.Instance.ToolManager.PlaceCurrentTool(false);
		if (CurrentLevel != null)
		{
			Destroy(CurrentLevel.gameObject);
			CurrentLevel = null;
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
		SetCurrentPlayer(0);
	}
}
