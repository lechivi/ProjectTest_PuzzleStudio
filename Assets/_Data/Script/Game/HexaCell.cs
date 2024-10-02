using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartColor
{
	public int index;
	public HexColor hexColor;
	public SpriteRenderer spriteRenderer;
}

public class HexaCell : MonoBehaviour
{
	public Coord Coord { get; private set; }
	public List<PartColor> partColors = new List<PartColor>();
	
	[SerializeField] private Transform trans;

	private HexaColorSO hexaColorSO;

	public void Init(CellData cellData, HexaColorSO hexaColorSO)
	{
#if UNITY_EDITOR
		this.gameObject.name = $"{cellData.Col},{cellData.Row}";
#endif

		Coord = new Coord(cellData.Col, cellData.Row);
		this.hexaColorSO = hexaColorSO;

		switch (cellData.Colors.Length)
		{
			case 1:
				SetColors(0, 6, cellData.Colors[0], hexaColorSO);
				break;
			case 2:
				if (cellData.StartAtTop)
				{
					SetColors(0, 3, cellData.Colors[0], hexaColorSO);
					SetColors(3, 6, cellData.Colors[1], hexaColorSO);
				}
				else
				{
					SetColors(0, 1, cellData.Colors[1], hexaColorSO);
					SetColors(1, 4, cellData.Colors[0], hexaColorSO);
					SetColors(4, 6, cellData.Colors[1], hexaColorSO);
				}
				break;
			case 3:
				if (cellData.StartAtTop)
				{
					SetColors(0, 2, cellData.Colors[0], hexaColorSO);
					SetColors(2, 4, cellData.Colors[1], hexaColorSO);
					SetColors(4, 6, cellData.Colors[2], hexaColorSO);
				}
				else
				{
					SetColors(0, 1, cellData.Colors[2], hexaColorSO);
					SetColors(1, 3, cellData.Colors[1], hexaColorSO);
					SetColors(3, 5, cellData.Colors[0], hexaColorSO);
					SetColors(5, 6, cellData.Colors[2], hexaColorSO);
				}
				break;
		}
	}

	private void SetColors(int startIndex, int endIndex, HexColor color, HexaColorSO hexaColorSO)
	{
		for (int i = startIndex; i < endIndex; i++)
		{
			partColors[i].hexColor = color;
			partColors[i].spriteRenderer.color = hexaColorSO.GetColor(color);
		}
	}

	public void RotateClockwise()
	{
		trans.Rotate(0, 0, -60);
		
		for (int i = 0; i < partColors.Count; i++)
		{
			partColors[i].index = (partColors[i].index + 1) % 6;
		}
	}

	public int ColorCount()
	{
		List<HexColor> color = new List<HexColor>();
		foreach (PartColor partColor in partColors)
		{
			if (!color.Contains(partColor.hexColor))
			{
				color.Add(partColor.hexColor);
			}
		}

		return color.Count;
	}

	private void OnMouseDown()
	{
		RotateClockwise();

#if UNITY_EDITOR
		Debug.Log("Clicked on tile: " + Coord, gameObject);
#endif
		CheckMatchColor();
	}

	private void CheckMatchColor()
	{
		for (int i = 0; i < 6; i++)
		{
			Coord neighbor = Extensions.GetHexNeighborWithDirection(Coord, i);
			HexaCell cell = GameManager.Instance.GridSystem.GetCell(neighbor);
			if (cell != null)
			{
#if UNITY_EDITOR
				Debug.Log("Neighbor: " + i, cell.gameObject);
				
				PartColor partColor = GetPartColor(i);
#endif
				if (this.partColors[i].hexColor == cell.GetDirectionHexColor(i))
				{
					Debug.Log("Matched " + cell.GetDirectionHexColor(i), cell.gameObject);
				}
			}
		}
	}
	
	private PartColor GetPartColor(int index)
	{
		for (int i = 0; i < partColors.Count; i++)
		{
			if (partColors[i].index == index)
			{
				return partColors[i];
			}
		}
		
		return null;
	}
	
	public HexColor GetDirectionHexColor(int index)
	{
		// return partColors[(index + 3) % 6].hexColor;
		return GetPartColor((index + 3) % 6).hexColor;
	}

}