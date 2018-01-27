using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
	[SerializeField]private PlayerInfo[] m_players = new PlayerInfo[2];
	[SerializeField]private float m_secondsPerTurn = 30f;
	private int m_currentPlayerId = 1;
	public PlayerInfo CurrentPlayer { get { return m_players[m_currentPlayerId]; } }
	private float m_turnTimer;
	private ToolManager m_toolManager;

	private void Awake()
	{
		m_toolManager = GetComponent<ToolManager>();
	}

	private void Start()
	{
		NextPlayer();
	}

	private void Update()
	{
		m_turnTimer -= Time.deltaTime;
		if (m_turnTimer <= 0f)
		{
			if (m_toolManager.CurrentToolState == ToolState.Placed)
			{
				m_toolManager.CommitCurrentTool();
			}

			print(string.Format("Player {0} turn done!", CurrentPlayer.PlayerName));
			NextPlayer();
			// TODO if you have placed box, commit it at current rotation
			// TODO visual feedback
		}
	}

	public void NextPlayer()
	{
		m_currentPlayerId = (m_currentPlayerId + 1) % 2;
		UpdatePlayerToolbox();
		m_turnTimer = m_secondsPerTurn;
		// TODO visual feedback like "Player X turn!"
	}

	public void UpdatePlayerToolbox()
	{
		// TODO change the visuals for available tools or does both players have the same tools?
	}
}
