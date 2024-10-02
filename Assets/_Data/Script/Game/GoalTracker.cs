using System;
using System.Collections.Generic;
public class GoalTracker : IDisposable
{
	public event Action OnGoalCompleted;
	private List<Goal> goals = new List<Goal>();

	public GoalTracker()
	{
		ListenerManager.Instance.Register(ListenType.OnMatchColor, Track);
	}

	public void Dispose()
	{
		ListenerManager.Instance.Unregister(ListenType.OnMatchColor, Track);
		OnGoalCompleted = null;
	}

	public void SetGoal(GoalColor[] goalColors)
	{
		goals.Clear();

		for (int i = 0; i < goalColors.Length; i++)
		{
			if (goalColors[i].Color == HexColor.None) continue;

			Goal goal = new Goal(goalColors[i]);
			goals.Add(goal);
		}
	}

	private void Track(object data)
	{
		HexColor hexColor = (HexColor)data;

		for (int i = 0; i < goals.Count; i++)
		{
			goals[i].Track(hexColor);
		}
		
		Goal goal = goals.Find(x => x.Type == hexColor);
		UIManager.Instance.GetExistScreen<ScreenInGame>().UpdateProgress(goal);
		
		CheckIfCompleted();
	}

	private void CheckIfCompleted()
	{
		bool isCompleted = true;

		for (int i = 0; i < goals.Count; i++)
		{
			if (!goals[i].IsCompleted)
			{
				isCompleted = false;
				break;
			}
		}

		if (isCompleted)
		{
			OnGoalCompleted?.Invoke();
		}
	}
}

public class Goal
{
	public HexColor Type { get; private set; }
	public int Target { get; private set; }

	public int Current { get; private set; }

	public bool IsCompleted => Current >= Target;

	public float Percentage => (float)Current / Target;

	public int Remaining => Target - Current;

	public Goal(GoalColor goalColor)
	{
		this.Type = goalColor.Color;
		this.Target = goalColor.Count;
		this.Current = 0;
	}

	public void Track(HexColor hexColor)
	{
		if (hexColor != Type) return;

		Current++;
	}
}
