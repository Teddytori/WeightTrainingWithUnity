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

public class WorkOutData
{
    public float SquatWeight;
    public float BenchPressWeight;
    public float DeadLiftWeight;
    public float PendlayRowWeight;
    public float OverHeadPressWeight;

    public int SquatWeek;
    public int BenchPressWeek;
    public int DeadLiftWeek;
    public int PendlayRowWeek;
    public int OverHeadPressWeek;

    public float MainUnitWeight;

    public List<SubWorkOutInfo> SubWorkOutList;

    public float GetMainWeight(MainWorkOutType type)
    {
        switch (type)
        {
            case MainWorkOutType.Squat:
                return SquatWeight;
            case MainWorkOutType.BenchPress:
                return BenchPressWeight;
            case MainWorkOutType.DeadLift:
                return DeadLiftWeight;
            case MainWorkOutType.PendlayRow:
                return PendlayRowWeight;
            case MainWorkOutType.OverHeadPress:
                return OverHeadPressWeight;
        }

        Debug.LogError("Unknown WorkOut : " + type);
        return 0;
    }

    public int GetMainWeek(MainWorkOutType type)
    {
        switch (type)
        {
            case MainWorkOutType.Squat:
                return SquatWeek;
            case MainWorkOutType.BenchPress:
                return BenchPressWeek;
            case MainWorkOutType.DeadLift:
                return DeadLiftWeek;
            case MainWorkOutType.PendlayRow:
                return PendlayRowWeek;
            case MainWorkOutType.OverHeadPress:
                return OverHeadPressWeek;
        }

        Debug.LogError("Unknown WorkOut : " + type);
        return 0;
    }
}

public class SubWorkOutInfo
{
    public string Name;
    public int Level;
    public float Weight;
    public float UnitWeight;
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

    private const string FILE_NAME = "Data.json";
    private static string FilePath => $"{Application.streamingAssetsPath}\\{FILE_NAME}";

    public WorkOutData UserData;

    public void LoadData()
    {
        if(File.Exists(FilePath) == false)
        {
            UserData = new();
            return;
        }

        var jsonStr = File.ReadAllText(FilePath);
        UserData = JsonUtility.FromJson<WorkOutData>(jsonStr);
    }

    public void SaveData()
    {
        var jsonStr = JsonUtility.ToJson(UserData);
        File.WriteAllText(FilePath, jsonStr);
    }
}

