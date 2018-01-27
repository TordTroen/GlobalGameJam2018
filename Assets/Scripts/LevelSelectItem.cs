using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectItem : MonoBehaviour
{
	[SerializeField]private Text m_levelNameText;

	public void Init(string levelName)
	{
		m_levelNameText.text = levelName;
	}
}
