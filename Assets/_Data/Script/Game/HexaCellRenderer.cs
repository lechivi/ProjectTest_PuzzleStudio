using System.Collections.Generic;
using UnityEngine;

public class HexaCellRenderer
{
	private HexaCell cell;
	private HexaColorSO hexaColorSO;

	public HexaCellRenderer(HexaCell hexaCell, HexaColorSO hexaColorSO, CellData cellData)
	{
		this.cell = hexaCell;
		this.hexaColorSO = hexaColorSO;

		this.InitColors(cellData.Colors, cellData.StartAtTop);
	}

	public void UpdateCellColors(HexColor removeColor)
	{
		switch (this.cell.HexColors.Count)
		{
			case 2:
				this.cell.HexColors.Remove(removeColor);
				SetColors(0, 6, this.cell.HexColors[0], hexaColorSO);
				break;

			case 3:
				// List<PartColorIndexes> partColorIndexes = this.cell.GetPartColorIndexes();
				// int partColorIndex = partColorIndexes.FindIndex(x => x.hexColor == removeColor);

				// var pair = partColorIndexes[partColorIndex];
				// int leftPairIndex = (partColorIndex + 2) % 3;
				// int rightPairIndex = (partColorIndex + 1) % 3;

				// this.cell.PartColors[pair.indexes[0]].hexColor = partColorIndexes[leftPairIndex].hexColor;
				// this.cell.PartColors[pair.indexes[0]].spriteRenderer.color = hexaColorSO.GetColor(partColorIndexes[leftPairIndex].hexColor);

				// this.cell.PartColors[pair.indexes[1]].hexColor = partColorIndexes[rightPairIndex].hexColor;
				// this.cell.PartColors[pair.indexes[1]].spriteRenderer.color = hexaColorSO.GetColor(partColorIndexes[rightPairIndex].hexColor);

				this.cell.HexColors.Remove(removeColor);
				int randomIndex = Random.Range(0, 2);

				for(int i =0; i < 6; i++)
				{
					if (this.cell.PartColors[i].hexColor == removeColor)
					{
						this.cell.PartColors[i].hexColor = this.cell.HexColors[randomIndex];
						this.cell.PartColors[i].spriteRenderer.color = hexaColorSO.GetColor(this.cell.HexColors[randomIndex]);
					}
				}
				break;
		}
	}

	private void InitColors(HexColor[] hexColors, bool startAtTop)
	{
		switch (hexColors.Length)
		{
			case 1:
				SetColors(0, 6, hexColors[0], hexaColorSO);
				break;
			case 2:
				if (startAtTop)
				{
					SetColors(0, 3, hexColors[0], hexaColorSO);
					SetColors(3, 6, hexColors[1], hexaColorSO);
				}
				else
				{
					SetColors(0, 1, hexColors[1], hexaColorSO);
					SetColors(1, 4, hexColors[0], hexaColorSO);
					SetColors(4, 6, hexColors[1], hexaColorSO);
				}
				break;
			case 3:
				if (startAtTop)
				{
					SetColors(0, 2, hexColors[0], hexaColorSO);
					SetColors(2, 4, hexColors[1], hexaColorSO);
					SetColors(4, 6, hexColors[2], hexaColorSO);
				}
				else
				{
					SetColors(0, 1, hexColors[2], hexaColorSO);
					SetColors(1, 3, hexColors[1], hexaColorSO);
					SetColors(3, 5, hexColors[0], hexaColorSO);
					SetColors(5, 6, hexColors[2], hexaColorSO);
				}
				break;
		}
	}

	private void SetColors(int startIndex, int endIndex, HexColor color, HexaColorSO hexaColorSO)
	{
		for (int i = startIndex; i < endIndex; i++)
		{
			if (this.cell.PartColors[i].hexColor == color) continue;

			this.cell.PartColors[i].hexColor = color;
			this.cell.PartColors[i].spriteRenderer.color = hexaColorSO.GetColor(color);
		}
	}

}
