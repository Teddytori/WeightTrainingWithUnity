using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum MainWorkOutType
{
    Squat = 0,
    BenchPress,
    DeadLift,
    PendlayRow,
    OverHeadPress
}

[System.Serializable]
public class WorkOutData
{
    public WorkOutData()
    {
        SquatInfo = new WorkoutInfo();
        BenchPressInfo = new WorkoutInfo();
        DeadLiftInfo = new WorkoutInfo();
        PendlayRowInfo = new WorkoutInfo();
        OverHeadPressInfo = new WorkoutInfo();
        MainUnitWeight = 5f;
        RecentDay = -1;
    }

    public WorkoutInfo SquatInfo;
    public WorkoutInfo BenchPressInfo;
    public WorkoutInfo DeadLiftInfo;
    public WorkoutInfo PendlayRowInfo;
    public WorkoutInfo OverHeadPressInfo;

    public float MainUnitWeight;

    public List<SubWorkoutInfo> SubWorkOutList;

    public int RecentDay;
}

[System.Serializable]
public class WorkoutInfo
{
    public WorkoutInfo(float weight = 0f)
    {
        Week = 1;
        Weight = weight;
    }

    public int Week;
    public float Weight;
}

[System.Serializable]
public class SubWorkoutInfo : WorkoutInfo
{
    public SubWorkoutInfo(string name, float weight, float unitWeight)
        : base(weight)
    {
        Name = name;
        UnitWeight = unitWeight;
        IsTargetDay = new bool[3] { true, true, true };
    }

    public string Name;
    public float UnitWeight;
    public bool[] IsTargetDay;
}

public class DataManager
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new();
            }

            return _instance;
        }
    }

    private const string FILE_NAME = "WeightData.json";
    private static string FilePath => $"{Application.persistentDataPath}\\{FILE_NAME}";

    public WorkOutData UserData;

    public void LoadData()
    {
        if(File.Exists(FilePath) == false)
        {
            UserData = new();
            return;
        }

        var jsonStr = File.ReadAllText(FilePath);
        try
        {
            UserData = JsonUtility.FromJson<WorkOutData>(jsonStr);
        }
        catch
        {
            UserData = new();
        }

        UserData ??= new();
    }

    public void SaveData()
    {
        var jsonStr = JsonUtility.ToJson(UserData);
        File.WriteAllText(FilePath, jsonStr);
    }
}

