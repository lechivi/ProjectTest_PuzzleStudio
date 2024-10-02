using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "SO/LevelData")]
public class LevelData : ScriptableObject
{
	public GoalColor[] GoalColors;
	public CellData[] CellDatas;
}

[System.Serializable]
public struct GoalColor
{
	public HexColor Color;
	public int Count;
}

[System.Serializable]
public struct CellData
{
	public int Col;
	public int Row;
	public bool StartAtTop;
	public HexColor[] Colors;
}