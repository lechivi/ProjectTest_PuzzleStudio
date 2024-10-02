using UnityEngine;

[CreateAssetMenu(fileName = "HexaColorSO", menuName = "SO/HexaColorSO")]
public class HexaColorSO : ScriptableObject
{
	public HexaColor[] hexaColors;
	
	public Color GetColor(HexColor hexColor)
	{
		foreach (HexaColor hexaColor in hexaColors)
		{
			if (hexaColor.hexColor == hexColor)
			{
				return hexaColor.color;
			}
		}

		return Color.white;
	}
}

[System.Serializable]
public struct HexaColor
{
	public HexColor hexColor;
	public Color color;
}
