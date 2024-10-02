using UnityEngine;
using UnityEngine.UI;

public static class UIExtensions
{
	public static void SetActive(this Button button, bool active)
	{
		button.interactable = active;
	}
	public static void SetActiveCanvasGroup(this CanvasGroup canvasGroup, bool value)
	{
		if (canvasGroup != null)
		{
			canvasGroup.alpha = value ? 1 : 0;
			canvasGroup.blocksRaycasts = value;
		}
	}
	public static void SetActiveImage(this Image img, bool value)
	{
		int a = value ? 1 : 0;
		img.color = new Color(1, 1, 1, a);
	}
}

