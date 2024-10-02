using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGoal : MonoBehaviour
{
	public HexColor HexColor { get; private set; }

	[SerializeField] private HexaColorSO hexaColorSO;

	[Space]
	[SerializeField] private TMP_Text goalText;
	[SerializeField] private Image iconImage;
	[SerializeField] private Transform reactTrans;
	[SerializeField] private GameObject done;

	public void Init(GoalColor goal)
	{
		this.HexColor = goal.Color;
		this.goalText.text = goal.Count.ToString();
		this.iconImage.color = hexaColorSO.GetColor(goal.Color);
	}

	public void UpdateProgress(int remain)
	{
		done.SetActive(remain <= 0);
		goalText.gameObject.SetActive(remain > 0);

		goalText.text = remain.ToString();

		if (remain >= 0)
		{
			ReactHit();
		}
	}

	private void ReactHit()
	{
		DOTween.Kill(this);
		this.reactTrans.localScale = Vector3.one;
		this.reactTrans.DOPunchScale(Vector3.one * 0.3f, 0.3f, 5).SetId(this);
	}
}
