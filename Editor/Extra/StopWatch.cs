using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCP.Utils.BenchMark
{
    public class StopWatch
	{
		private static List<CheckPoint> runList = new List<CheckPoint>();
		private List<CheckPoint> checkPoints = new List<CheckPoint>();
		public StopWatch(string name)
		{
			checkPoints.Clear();
			checkPoints.Add(new CheckPoint() { name = name, time = Time.realtimeSinceStartup });
		}

		public StopWatch()
		{
			checkPoints.Clear();
			checkPoints.Add(new CheckPoint() { name = "Start", time = Time.realtimeSinceStartup });
		}

		public void Check(string name) => checkPoints.Add(new CheckPoint() { name = name, time = Time.realtimeSinceStartup });
		public void GetResult()
		{
			if (checkPoints.Count == 0) return;
			else if (checkPoints.Count == 1) Logger.LogError("StopWatch has only one check point");
			string result = "StopWatch Runtime:\n";
			for (int i = 0; i < checkPoints.Count - 1; i++)
			{
				result += $"{checkPoints[i].name} to {checkPoints[i + 1].name} : {checkPoints[i + 1].time - checkPoints[i].time}\n";
			}
			Logger.LogInfo(result);
		}
		public void Finish(string name)
		{
			if (checkPoints.Count < 1) Logger.LogError("StopWatch has only one check point");
			checkPoints.Add(new CheckPoint() { name = name, time = Time.realtimeSinceStartup });
			runList.Add(checkPoints[0]);
			runList.Add(checkPoints[checkPoints.Count - 1]);
			GetResult();
		}
		public void Finish() => Finish("Finish");
		public void Reset() => checkPoints.Clear();
		[MenuItem("Benchmark/Stopwatch AverageRunTime")]
		public static void GetAverageRunTime()
		{
			if (runList.Count == 0) return;
			else if (runList.Count == 1) Logger.LogError("StopWatch not engough run");
			string result = "StopWatch Summary:\n";
			List<float> runTimes = new List<float>();
			for (int i = 0; i < runList.Count; i += 2)
			{
				runTimes.Add(runList[i + 1].time - runList[i].time);
				result += $"{runList[i].name} to {runList[i + 1].name} : {runList[i + 1].time - runList[i].time}\n";
			}
			float average = 0;
			foreach (float v in runTimes)
			{
				average += v;
			}
			average /= runTimes.Count;
			result += $"Average : {average},total : {runTimes.Count}";
			Logger.LogWarning(result);
		}
		[MenuItem("Benchmark/Stopwatch ResetRuns")]
		public static void ResetRuns() => runList.Clear();
	}
	internal class Logger
	{
		internal static void LogInfo(string msg) => Debug.Log("Bechmark:" + msg);
		internal static void LogWarning(string msg) => Debug.LogWarning("Bechmark:" + msg);
		internal static void LogError(string msg) => Debug.LogError("Bechmark:" + msg);
	}
	internal struct CheckPoint
	{
		public string name;
		public float time;
	}
}
