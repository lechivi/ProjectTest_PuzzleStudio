using UnityEngine;
using UnityEngine.UI;

public class PopupLose : BasePopup
{
	[SerializeField] private Button okButton;

	private void OnDestroy()
	{
		this.okButton.onClick.RemoveListener(OnClickOk);
	}

	protected override void RegisterButtons()
	{
		base.RegisterButtons();
		this.okButton.onClick.AddListener(OnClickOk);
	}

	private void OnClickOk()
	{
		this.Hide();
		GameManager.Instance.LoadLevel();
	}
}
