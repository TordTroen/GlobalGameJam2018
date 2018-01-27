using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
	[SerializeField]private string m_playerName = "NameHere";
	public string PlayerName { get { return m_playerName; } }
	[SerializeField]private Color m_playerColor = Color.red;
	public Color PlayerColor { get { return m_playerColor; } }
	//[SerializeField]private List<Tool> m_toolbox = new List<Tool>();
}
