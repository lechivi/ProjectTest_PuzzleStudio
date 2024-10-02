using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
	Unknow = 0,
	Screen = 1,
	Popup = 2,
	Notify = 3,
	ScrollScreen = 4,
}

public class BaseUIElement : MonoBehaviour
{
	protected CanvasGroup canvasGroup;
	protected GraphicRaycaster graphicRaycaster;
	protected bool isHide;
	protected UIType uiType = UIType.Unknow;

	private bool isInited;

	public bool IsHide { get => isHide; }
	public CanvasGroup CanvasGroup { get => canvasGroup; }
	public GraphicRaycaster GraphicRaycaster { get => graphicRaycaster; }
	public RectTransform RectTransform { get; private set; }
	public bool IsInited { get => isInited; }
	public UIType UIType { get => uiType; }

	public virtual void Init()
	{
		this.isInited = true;
		this.InitCanvasGroup();
		this.InitGraphicRaycaster();
		this.gameObject.SetActive(true);
		this.RectTransform = this.gameObject.GetComponent<RectTransform>();

		RegisterButtons();
		Hide_Internal();
	}

	public virtual void Show(object data)
	{
		this.gameObject.SetActive(true);

		if (this.graphicRaycaster != null)
		{
			this.graphicRaycaster.enabled = true;
		}

		this.isHide = false;
		SetActiveGroupCanvas(true);
	}

	public virtual void Hide()
	{
		this.isHide = true;

		if (this.graphicRaycaster != null)
		{
			this.graphicRaycaster.enabled = false;
		}

		SetActiveGroupCanvas(false);

		// BaseUIManager.Instance.HideElementUI(this);
	}

	private void SetActiveGroupCanvas(bool isAct)
	{
		if (CanvasGroup != null)
		{
			CanvasGroup.SetActiveCanvasGroup(isAct);
		}
	}

	private void Hide_Internal()
	{
		this.isHide = true;

		if (this.graphicRaycaster != null)
		{
			this.graphicRaycaster.enabled = false;
		}

		SetActiveGroupCanvas(false);

		// BaseUIManager.Instance.HideElementUI(this);
	}

	protected virtual void RegisterButtons()
	{
		// Register button here
		// this function will be called in the Init method 
	}

	private void InitCanvasGroup()
	{
		if (!this.gameObject.GetComponent<CanvasGroup>())
		{
			this.gameObject.AddComponent<CanvasGroup>();
		}
		this.canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
	}

	private void InitGraphicRaycaster()
	{
		this.graphicRaycaster = this.gameObject.GetComponent<GraphicRaycaster>();
	}
}

