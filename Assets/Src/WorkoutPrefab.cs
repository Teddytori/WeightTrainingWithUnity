using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutPrefab : MonoBehaviour
{
    public Text WorkoutNameText;
    public Text CurrentWeekText;
    public List<SetObject> SetList;
    
    public WorkoutInfo CurWorkoutInfo { get; set; }

    private float curBaseWeight;
    private int curWeek;
    private float curUnitWeight;
    private bool isAmrep;

    delegate void WeightSettingMethod(bool useAmrep);
    private WeightSettingMethod curMethod;
    private string weekTextFormat;

    private const string WEEK_COUNT_STRING = "{0} 주차";
    private const string TIME_COUNT_STRING = "{0} 회차";

    public void SetWorkoutInfo(string workoutName, WorkoutInfo info, float unitWeight, bool isMain = true)
    {
        CurWorkoutInfo = info;

        WorkoutNameText.text = workoutName;
        curBaseWeight = info.Weight;
        curWeek = info.Week;
        if (isMain)
            weekTextFormat = WEEK_COUNT_STRING;
        else
            weekTextFormat = TIME_COUNT_STRING;
        CurrentWeekText.text = string.Format(weekTextFormat, curWeek);
        curUnitWeight = unitWeight;

        foreach(var set in SetList)
        {
            set.CurWorkoutPrefab = this;
            set.CheckToggle.isOn = false;
        }
    }

    public void SetWeightForDay1(bool useAmrep)
    {
        float mainWeight = WorkOutManager.GetMainSetWeight(curBaseWeight, curUnitWeight, curWeek - 1);
        for(int i = 0; i < 4; ++i)
        {
            float curSetWeight = WorkOutManager.GetSetWeightForMainWork(i + 1, 5, mainWeight, curUnitWeight);
            SetList[i].gameObject.SetActive(true);
            SetList[i].SetText(curSetWeight, 5);
        }
        SetList[4].gameObject.SetActive(true);
        SetList[4].SetText(mainWeight, 5);

        if (useAmrep)
        {
            SetList[5].gameObject.SetActive(true);
            SetList[5].SetText(SetList[1].WeightText.text, 0);
        }
        else
        {
            SetList[5].gameObject.SetActive(false);
        }

        isAmrep = useAmrep;
        curMethod = SetWeightForDay1;
    }

    public void SetWeightForDay3(bool useAmrep = false)
    {
        float day1MainWeight = WorkOutManager.GetMainSetWeight(curBaseWeight, curUnitWeight, curWeek - 1);
        for (int i = 0; i < 4; ++i)
        {
            float curSetWeight = WorkOutManager.GetSetWeightForMainWork(i + 1, 5, day1MainWeight, curUnitWeight);
            SetList[i].gameObject.SetActive(true);
            SetList[i].SetText(curSetWeight, 5);
        }

        float day3MainWeight = WorkOutManager.GetMainSetWeight(curBaseWeight, curUnitWeight, curWeek);
        SetList[4].gameObject.SetActive(true);
        SetList[4].SetText(day3MainWeight, 3);

        SetList[5].gameObject.SetActive(true);
        SetList[5].SetText(SetList[2].WeightText.text, 0);

        curMethod = SetWeightForDay3;
    }

    public void SetWeightForDL(bool useAmrep = false)
    {
        float set3Weight = WorkOutManager.GetMainSetWeight(curBaseWeight, curUnitWeight, curWeek - 1);
        for (int i = 0; i < 2; ++i)
        {
            float curSetWeight = WorkOutManager.GetSetWeightForMainWork(i + 1, 3, set3Weight, curUnitWeight);
            SetList[i].gameObject.SetActive(true);
            SetList[i].SetText(curSetWeight, 5);
        }
        SetList[2].gameObject.SetActive(true);
        SetList[2].SetText(set3Weight, 5);

        float mainWeight = WorkOutManager.GetMainSetWeight(curBaseWeight, curUnitWeight, curWeek);
        SetList[3].gameObject.SetActive(true);
        SetList[3].SetText(mainWeight, 3);

        for(int i = 4; i < SetList.Count; ++i)
        {
            SetList[i].gameObject.SetActive(false);
        }

        curMethod = SetWeightForDL;
    }

    public void SetWeightForOHP(bool useAmrep = false)
    {
        float mainWeight = WorkOutManager.GetMainSetWeight(curBaseWeight, curUnitWeight, curWeek - 1);
        for(int i = 0; i < 3; ++i)
        {
            float curSetWeight = WorkOutManager.GetSetWeightForMainWork(i + 1, 4, mainWeight, curUnitWeight);
            SetList[i].gameObject.SetActive(true);
            SetList[i].SetText(curSetWeight, 5);
        }
        SetList[3].gameObject.SetActive(true);
        SetList[3].SetText(mainWeight, 5);

        SetList[4].gameObject.SetActive(true);
        SetList[4].SetText(SetList[0].WeightText.text, 0);

        SetList[5].gameObject.SetActive(false);

        curMethod = SetWeightForOHP;
    }

    public void SetLightWeight(bool useAmrep = false)
    {
        float mainWeight = WorkOutManager.GetMainSetWeight(curBaseWeight, curUnitWeight, curWeek - 1);
        for (int i = 0; i < 3; ++i)
        {
            float curSetWeight = WorkOutManager.GetSetWeightForMainWork(i + 1, 5, mainWeight, curUnitWeight);
            SetList[i].gameObject.SetActive(true);
            SetList[i].SetText(curSetWeight, 5);
        }
        SetList[3].gameObject.SetActive(true);
        SetList[3].SetText(SetList[1].WeightText.text, 0);
        SetList[4].gameObject.SetActive(true);
        SetList[4].SetText(SetList[0].WeightText.text, 0);

        SetList[5].gameObject.SetActive(false);

        curMethod = SetLightWeight;
    }

    public void SetSubWorkoutWeight(bool useAmrep = false)
    {
        float mainWeight = curBaseWeight + curUnitWeight * (curWeek - 1);
        for(int i = 0; i < 4; ++i)
        {
            SetList[i].gameObject.SetActive(true);
            SetList[i].SetText(mainWeight, i == 3 ? 0 : 12);
        }
        for(int i = 4; i < SetList.Count; ++i)
        {
            SetList[i].gameObject.SetActive(false);
        }

        curMethod = SetSubWorkoutWeight;
    }

    public void OnClickNextWeek()
    {
        if(curWeek < 12)
        {
            curWeek++;
            curMethod(isAmrep);
        }
        RefreshWeekText();
    }

    public void OnClickPrevWeek()
    {
        if(curWeek > 1)
        {
            curWeek--;
            curMethod(isAmrep);
        }
        RefreshWeekText();
    }

    public void RefreshWeekText()
    {
        CurrentWeekText.text = string.Format(weekTextFormat, curWeek);
    }

    public void CheckClearedAndSetWeek()
    {
        if (curMethod == SetLightWeight || curMethod == SetWeightForDay1)
            return;

        bool isComplete = true;
        foreach(var set in SetList)
        {
            isComplete &= !set.gameObject.activeSelf || set.CheckToggle.isOn;
        }

        if (isComplete)
            CurWorkoutInfo.Week = curWeek + 1;
    }
}
