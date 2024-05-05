using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SetInfoManager : MonoBehaviour
{
    public InputField SquatInput;
    public InputField BenchPressInput;
    public InputField DeadLiftInput;
    public InputField PendlayRowInput;
    public InputField OverHeadPressInput;

    public InputField MainUnitWeightInput;

    public GameObject SubWorkObjRoot;
    public List<SubWorkOutInfo> SubWorkoutInfos;

    void OnEnable()
    {
        
    }

    public void OnClickSave()
    {
        var userData = DataManager.Instance.UserData;
        try
        {
            userData.SquatWeight = float.Parse(SquatInput.text);
            userData.BenchPressWeight = float.Parse(BenchPressInput.text);
            userData.DeadLiftWeight = float.Parse(DeadLiftInput.text);
            userData.PendlayRowWeight = float.Parse(PendlayRowInput.text);
            userData.OverHeadPressWeight = float.Parse(OverHeadPressInput.text);

            userData.MainUnitWeight = float.Parse(MainUnitWeightInput.text);

            if(SubWorkoutInfos != null)
            {
                foreach(var subInfo in SubWorkoutInfos)
                {
                    userData.SubWorkOutList.Add(subInfo);
                }
            }

            DataManager.Instance.SaveData();
        }
        catch (Exception)
        {
            Debug.LogError("Parsing Weight Error");
        }
    }
}
