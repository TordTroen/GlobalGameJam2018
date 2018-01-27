using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
	[SerializeField]private PlayerInfo[] m_players = new PlayerInfo[2];
	private int m_currentPlayer = 0;

	private void Start()
	{
		UpdatePlayerToolbox();
	}

	public void NextPlayer()
	{
		m_currentPlayer = (m_currentPlayer + 1) % 2;
		UpdatePlayerToolbox();
	}

	public void UpdatePlayerToolbox()
	{
	}
}
