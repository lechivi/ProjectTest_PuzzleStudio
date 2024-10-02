using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	public GridSystem GridSystem { get => this.gridSystem; }

	[SerializeField] private LevelData levelData;
	[SerializeField] private GridSystem gridSystem;

	private void Start()
	{
		this.LoadLevel();
	}

	[EasyButtons.Button]
	private void LoadLevel()
	{
		this.gridSystem.LoadLevel(this.levelData);
	}

	// void Update()
	// {
	// 	if (Input.GetMouseButtonDown(0))
	// 	{
	// 		Debug.Log("Mouse Clicked");
	// 		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	// 		Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

	// 		RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
	// 		Debug.DrawRay(mousePos2D, Vector2.zero, Color.red, 2f);

	// 		if (hit.collider != null)
	// 		{
	// 			HexaCell tile = hit.collider.GetComponent<HexaCell>();
	// 			if (tile != null)
	// 			{
	// 				tile.RotateClockwise();

	// 				// gridSystem.CheckAndRemoveMatchingColors(tile.row, tile.col);
	// 				Debug.LogError("Clicked on tile: " + tile.Coord, tile.gameObject);
	// 				for (int i = 0; i < 6; i++)
	// 				{
	// 					Coord neighbor = Extensions.GetHexNeighborWithDirection(tile.Coord, i);
	// 					HexaCell cell = gridSystem.GetCell(neighbor);
	// 					if (cell != null)
	// 					{
	// 						Debug.LogError("Neighbor: " + i, cell.gameObject);
	// 					}
	// 				}
	// 			}
	// 		}
	// 	}
	// }

}
