using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInGame : BaseScreen
{
	[SerializeField] private UIGoal pfGoal;

	[Space]
	[SerializeField] private Transform goalHolder;

	private List<UIGoal> goals = new List<UIGoal>();

	public void InitGoal(GoalColor[] goals)
	{
		this.Clear();
		foreach (var goal in goals)
		{
			var uiGoal = Instantiate(pfGoal, goalHolder);
			uiGoal.Init(goal);
			this.goals.Add(uiGoal);
		}
	}

	public void UpdateProgress(Goal goalTracker)
	{
		if (goalTracker.Type == HexColor.None) return;

		var uiGoal = GetUIGoal(goalTracker.Type);
		if (uiGoal != null)
		{
			uiGoal.UpdateProgress(goalTracker.Remaining);
		}
	}

	public UIGoal GetUIGoal(HexColor type)
	{
		for (int i = 0; i < goals.Count; i++)
		{
			if (goals[i].HexColor == type)
			{
				return goals[i];
			}
		}

		return null;
	}

	private void Clear()
	{
		foreach (var goal in goals)
		{
			Destroy(goal.gameObject);
		}
		goals.Clear();
	}
}
