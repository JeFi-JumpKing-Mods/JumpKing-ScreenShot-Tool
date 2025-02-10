using System;

namespace ScreenShotTool.Utils;

public class Timer
{
	private float timer;

	private float firstTime;
	public float FirstTime
	{
		get
		{
			return firstTime;
		}
		set
		{
			firstTime = Math.Max(0f, value);
		}
	}
	public bool checkFistTime
	{
		get;
		private set;
	}
	
	private float loopTime;
	public float LoopTime
	{
		get
		{
			return loopTime;
		}
		set
		{
			loopTime = Math.Max(0f, value);
		}
	}

	public float Current => timer;

	public Timer(float duration): this(first: duration, loop: duration)
	{
	}

	public Timer(float first, float loop)
	{
		firstTime = first;
		loopTime = loop;
		Reset();
	}

	public void Reset()
	{
		checkFistTime = true;
		timer = 0f;
	}

	private void WrapTimer()
	{
		while (timer >= loopTime)
		{
			timer -= loopTime;
		}
	}

	public bool Update(float delta)
	{
		if (LoopTime == 0f)
		{
			return true;
		}
		timer += delta;
		if (checkFistTime)
		{
			if (timer >= firstTime)
			{
				checkFistTime = false;
				timer -= firstTime;
			}
			else
			{
				return false;
			}
		}
		if (timer >= loopTime)
		{
			WrapTimer();
			return true;
		}
		return false;
	}
}
