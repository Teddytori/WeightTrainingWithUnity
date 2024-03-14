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

public enum SubWorkOutType
{

}

[System.Serializable]
public class WorkOutData
{
    public Dictionary<MainWorkOutType, float> MainWorkOutWeightDict = new();
    public Dictionary<MainWorkOutType, int> MainWorkOutWeekDict = new();
    public float MainUnitWeight;

    public Dictionary<SubWorkOutType, float> SubWorkOutWeightDict = new();
    public Dictionary<SubWorkOutType, int> SubWorkOutLevelDict = new();
    public Dictionary<SubWorkOutType, float> SubWorkOutUnitWeightDict = new();
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

    private const string FILE_NAME = "Data.dat";
    
    public WorkOutData UserData;

    public void LoadData()
    {
        string filePath = $"{Application.streamingAssetsPath}\\{FILE_NAME}";
        if(File.Exists(filePath) == false)
        {
            UserData = new();
            return;
        }
        
    }

    public void SaveData()
    {

    }
}

