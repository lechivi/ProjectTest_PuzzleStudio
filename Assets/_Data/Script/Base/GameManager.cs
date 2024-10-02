using UnityEngine;
public enum LevelResultType
{
	None,
	Win,
	Lose
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	public GridSystem GridSystem { get => this.gridSystem; }

	[SerializeField] private LevelData levelData;
	[SerializeField] private GridSystem gridSystem;

	private GoalTracker goalTracker;

	private void Start()
	{
		this.Init();

		UIManager.Instance.ShowScreen<ScreenInGame>();
		this.LoadLevel();
	}

	private void Init()
	{
		this.goalTracker = new GoalTracker();
		this.goalTracker.OnGoalCompleted += OnGoalCompleted;
	}

	[EasyButtons.Button]
	public void LoadLevel()
	{
		this.gridSystem.LoadLevel(this.levelData);
		this.goalTracker.SetGoal(this.levelData.GoalColors);

		ScreenInGame screenInGame = UIManager.Instance.GetExistScreen<ScreenInGame>();
		screenInGame.InitGoal(this.levelData.GoalColors);
	}

	#region  RESULT
	public void CheckResult(LevelResultType result)
	{
		if (result == LevelResultType.None) return;

		switch (result)
		{
			case LevelResultType.Win: Win(); break;
			case LevelResultType.Lose: Lose(); break;
		}
	}

	private void OnGoalCompleted()
	{
		CheckResult(LevelResultType.Win);
	}

	private void Win()
	{
		// Debug.Log("You Win!");
		UIManager.Instance.ShowPopup<PopupWin>();
	}

	private void Lose()
	{
		// Debug.Log("You Lose");
		UIManager.Instance.ShowPopup<PopupLose>();
	}
	#endregion
}
