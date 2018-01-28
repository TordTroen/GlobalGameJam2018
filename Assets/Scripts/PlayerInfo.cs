using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInfo
{
	[SerializeField]private string m_playerName = "NameHere";
	public string PlayerName { get { return m_playerName; } }
	[SerializeField]private Color m_playerColor = Color.red;
	public Color PlayerColor { get { return m_playerColor; } }
	public int Score { get { return m_score; } set { m_score = value; m_scoreText.text = m_score.ToString(); } }
	private int m_score;
	[SerializeField]private Text m_scoreText;
	[SerializeField]private GameObject m_selectedPlayerOverlay;
	public GameObject SelectedPlayerOverlay { get { return m_selectedPlayerOverlay; } set { m_selectedPlayerOverlay = value; } }
	public int PlayerId { get; set; }
}
