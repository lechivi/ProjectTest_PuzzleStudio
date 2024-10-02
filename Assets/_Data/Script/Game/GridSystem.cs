using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Coord
{
	public int Col;
	public int Row;

	public Coord(int col, int row)
	{
		this.Col = col;
		this.Row = row;
	}

	public static bool operator ==(Coord a, Coord b)
	{
		return a.Col == b.Col && a.Row == b.Row;
	}

	public static bool operator !=(Coord a, Coord b)
	{
		return !(a == b);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Col, Row);
	}

	public override string ToString()
	{
		return $"{Col},{Row}";
	}
}

public class GridSystem : MonoBehaviour
{
	public Dictionary<Coord, HexaCell> Grids { get; private set; }

	[SerializeField] private HexaColorSO hexaColorSO;
	[SerializeField] private HexaCell pfCell;
	[SerializeField] private Transform levelHolder;

	private float height;
	[SerializeField] private float size = 1.075f;

	public void LoadLevel(LevelData levelData)
	{
		Grids = new Dictionary<Coord, HexaCell>();
		RenderLevel(levelData);
	}

	public void Clear()
	{
		for (int i = levelHolder.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(levelHolder.GetChild(i).gameObject);
		}
	}

	public HexaCell GetCell(Coord coord)
	{
		if (Grids.TryGetValue(coord, out HexaCell cell))
		{
			return cell;
		}
		return null;
	}

	public void CheckMatchAt(Coord coord)
	{
		HexaCell cell = GetCell(coord);
		if (cell == null) return;
		
		List<PartColorIndexes> partColorIndexes = cell.GetPartColorIndexes();
		foreach (PartColorIndexes partColorIndex in partColorIndexes)
		{
			List<HexaCell> matchedCells = new List<HexaCell>();
			foreach (int index in partColorIndex.indexes)
			{
				Coord neighbor = Extensions.GetHexNeighborWithDirection(coord, index);
				HexaCell neighborCell = GetCell(neighbor);
				if (neighborCell != null)
				{
					if (partColorIndex.hexColor == neighborCell.GetDirectionHexColor(index))
					{
						neighborCell.SetNoneColor(partColorIndex.hexColor);
						matchedCells.Add(neighborCell);
					}
				}
			}
			
			if (matchedCells.Count > 0)
			{
				cell.SetNoneColor(partColorIndex.hexColor);
				ListenerManager.Instance.BroadCast(ListenType.OnMatchColor, partColorIndex.hexColor);
			}
			
			//TODO: Check if we need to check the matched cells	
		}
		
	}
	
	private void RenderLevel(LevelData levelData)
	{
		Clear();

		// instantiate cells
		for (int i = 0; i < levelData.CellDatas.Length; i++)
		{
			var cell = levelData.CellDatas[i];
			//TODO: Replace instantiation with object pooling
			HexaCell cellInstance = Instantiate(pfCell, CoordToWorld(new Coord(cell.Col, cell.Row)), Quaternion.identity, levelHolder);
			cellInstance.Init(cell, hexaColorSO);
			Grids.Add(cellInstance.Coord, cellInstance);
		}
	}

	public void CheckAndRemoveMatchingColors(int row, int col)
	{
		// HexaTile tile = grid[row, col];

		// // Kiểm tra với các ô liền kề (xung quanh miếng hexa)
		// for (int i = -1; i <= 1; i++)
		// {
		// 	for (int j = -1; j <= 1; j++)
		// 	{
		// 		if (i == 0 && j == 0) continue; // Không so sánh với chính nó

		// 		int checkX = row + i;
		// 		int checkY = col + j;

		// 		if (IsValidTile(checkX, checkY))
		// 		{
		// 			HexaTile neighborTile = grid[checkX, checkY];
		// 			foreach (HexColor color in tile.colors)
		// 			{
		// 				if (neighborTile.colors.Contains(color))
		// 				{
		// 					// Cả hai miếng hexa mất đi màu đó
		// 					tile.RemoveColor(color);
		// 					neighborTile.RemoveColor(color);
		// 				}
		// 			}
		// 		}
		// 	}
		// }

		// // Nếu miếng hexa không còn màu, xóa nó đi
		// if (tile.IsEmpty())
		// {
		// 	grid[row, col] = null;
		// }
	}

	private Vector3 CoordToWorld(Coord coord)
	{
		height = Mathf.Sqrt(3) * size;

		float x = coord.Col * size * 1.5f;

		float y = coord.Row * height + (coord.Col % 2 == 0 ? 0 : height / 2f);

		return new Vector3(x, y, 0);
		// return new Vector3(x, 0, y);
	}

	public IEnumerable<(Coord coord, HexaCell cell)> GetNeighbors(HexaCell position)
	{
		var neighborCoords = Extensions.GetNeighborCoordinate(position.Coord);

		foreach (var coord in neighborCoords)
		{
			if (Grids.TryGetValue(coord, out HexaCell cellRs))
			{
				yield return (coord, cellRs);
			}
		}
	}
}
