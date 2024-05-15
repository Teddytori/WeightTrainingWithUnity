using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubWorkoutInput : MonoBehaviour
{
    public InputField NameInput;
    public InputField BaseWeightInput;
    public InputField UnitWeightInput;
    public Toggle[] TargetDayToggles;

    private SubWorkoutInfo info;
    private SetInfoManager manager;

    public void SetInfo(SubWorkoutInfo targetInfo, SetInfoManager mgr)
    {
        manager = mgr;
        info = targetInfo;
        NameInput.text = targetInfo.Name;
        BaseWeightInput.text = targetInfo.Weight.ToString();
        UnitWeightInput.text = targetInfo.UnitWeight.ToString();
        for(int i = 0; i < targetInfo.IsTargetDay.Length; ++i)
        {
            TargetDayToggles[i].isOn = targetInfo.IsTargetDay[i];
        }
    }

    public SubWorkoutInfo GetAppliedData()
    {
        info.Name = NameInput.text;
        info.Weight = float.Parse(BaseWeightInput.text);
        info.UnitWeight = float.Parse(UnitWeightInput.text);
        for (int i = 0; i < info.IsTargetDay.Length; ++i)
        {
            info.IsTargetDay[i] = TargetDayToggles[i].isOn;
        }
        info.Week = 1;

        return info;
    }

    public void LoadCurrentWeight()
    {
        BaseWeightInput.text = (info.Weight + (info.Week - 1) * info.UnitWeight).ToString();
    }

    public void OnClickCancel()
    {
        info = null;
        manager.DeleteSubWorkInput(this);
    }
}
