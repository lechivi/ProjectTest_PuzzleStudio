using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PartColor
{
	public int index;
	public HexColor hexColor;
	public SpriteRenderer spriteRenderer;
}

[System.Serializable]
public class PartColorIndexes
{
	public HexColor hexColor;
	public List<int> indexes;
	
	public PartColorIndexes()
	{
		this.indexes = new List<int>();
	}
}

public class HexaCell : MonoBehaviour
{
	public bool IsRotating { get; private set; }
	public Coord Coord { get; private set; }
	public List<PartColor> PartColors { get => this.partColors; }
	public List<HexColor> HexColors { get => this.hexColors; }

	[SerializeField] private Transform trans;
	[SerializeField] private SortingGroup sortingGroup;
	// [SerializeField] private SpriteRenderer[] partSRs;
	[SerializeField] private List<PartColor> partColors = new List<PartColor>();
	[SerializeField] private TMP_Text coordText;

	[Space]
	[SerializeField] private float rotateDuration = 0.5f;

	private HexaCellRenderer cellRenderer;
	private List<HexColor> hexColors = new List<HexColor>();
	
	private const int DEFAULT_ORDER = 10;
	private const int MOVE_ORDER = 20;

	private void OnMouseDown()
	{
		RotateClockwise();

// #if UNITY_EDITOR
// 		Debug.Log("Clicked on tile: " + Coord, gameObject);
// #endif
	}

	public void Init(CellData cellData, HexaColorSO hexaColorSO)
	{
#if UNITY_EDITOR
		this.gameObject.name = $"{cellData.Col},{cellData.Row}";
#endif

		this.Coord = new Coord(cellData.Col, cellData.Row);
		this.hexColors = cellData.Colors.ToList();
		
		this.cellRenderer = new HexaCellRenderer(this, hexaColorSO, cellData);
		this.SetOrder(false);
		
		this.coordText.text = $"{this.Coord}";
	}

	public int ColorCount()
	{
		return this.hexColors.Count;
	}
	
	public List<PartColorIndexes> GetPartColorIndexes()
	{
		List<PartColorIndexes> partColorIndexes = new List<PartColorIndexes>();
		foreach (PartColor partColor in partColors)
		{
			if (partColor.hexColor == HexColor.None) continue;
			
			PartColorIndexes partColorIndex = partColorIndexes.Find(x => x.hexColor == partColor.hexColor);
			if (partColorIndex == null)
			{
				partColorIndex = new PartColorIndexes();
				partColorIndex.hexColor = partColor.hexColor;
				partColorIndex.indexes = new List<int>();
				partColorIndexes.Add(partColorIndex);
			}

			partColorIndex.indexes.Add(partColor.index);
		}

		return partColorIndexes;
	}
	
	public void SetNoneColor(HexColor hexColor)
	{
		int currentColor = this.ColorCount();
		
		switch (currentColor)
		{
			case 1:
				CompletCell();
				break;
			default:
				this.cellRenderer.UpdateCellColors(hexColor);
				break;
		}
	}
	
	private void CompletCell()
	{
		foreach (PartColor partColor in partColors)
		{
			partColor.hexColor = HexColor.None;
			partColor.spriteRenderer.color = Color.white;
		}
	}
	
	public HexColor GetDirectionHexColor(int index)
	{
		// return partColors[(index + 3) % 6].hexColor;
		return GetPartColor((index + 3) % 6).hexColor;
	}

	public void RotateClockwise()
	{
		// trans.Rotate(0, 0, -60);

		// for (int i = 0; i < partColors.Count; i++)
		// {
		// 	partColors[i].index = (partColors[i].index + 1) % 6;
		// }

		if (this.IsRotating) return;

		RotateClockwise(rotateDuration);
	}

	public Sequence RotateClockwise(float duration)
	{
		this.IsRotating = true;
		this.SetOrder(true);

		Sequence seq = DOTween.Sequence();
		seq.SetId(this);
		seq.Append(trans.DOPunchScale(Vector3.one * 0.3f, duration, 5));
		seq.Join(this.trans.DOLocalRotate(new Vector3(0, 0, this.trans.eulerAngles.z - 60), duration));
		seq.OnComplete(() =>
		{
			this.IsRotating = false;
			this.SetOrder(false);
			this.ShiftPartColorIndex();
			
			GameManager.Instance.GridSystem.CheckMatchAt(this.Coord);
		});

		return seq;
	}

	private void ShiftPartColorIndex()
	{
		for (int i = 0; i < partColors.Count; i++)
		{
			partColors[i].index = (partColors[i].index + 1) % 6;
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

	private void SetOrder(bool isMove)
	{
		sortingGroup.sortingOrder = isMove ? MOVE_ORDER : DEFAULT_ORDER;
	}
}