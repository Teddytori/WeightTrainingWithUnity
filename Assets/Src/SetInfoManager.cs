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

    public GameObject SubWorkPrefab;
    public GameObject SubWorkObjRoot;
    public RectTransform ScrollContentsRect;

    private List<SubWorkoutInput> subWorkoutInputs = new List<SubWorkoutInput>();
    private float initialScrollHeight = 0;

    private void Awake()
    {
        initialScrollHeight = ScrollContentsRect.rect.height;
    }

    private void OnEnable()
    {
        var userdata = DataManager.Instance.UserData;

        SquatInput.text = userdata.SquatInfo.Weight.ToString();
        BenchPressInput.text = userdata.BenchPressInfo.Weight.ToString();
        DeadLiftInput.text = userdata.DeadLiftInfo.Weight.ToString();
        PendlayRowInput.text = userdata.PendlayRowInfo.Weight.ToString();
        OverHeadPressInput.text = userdata.OverHeadPressInfo.Weight.ToString();

        MainUnitWeightInput.text = userdata.MainUnitWeight.ToString();

        if (userdata.SubWorkOutList != null && userdata.SubWorkOutList.Count != 0)
        {
            foreach(var subData in userdata.SubWorkOutList)
            {
                AddSubWorkInput(subData);
            }
        }
    }

    private void OnDisable()
    {
        if (subWorkoutInputs.Count != 0)
        {
            foreach (var sub in subWorkoutInputs)
            {
                Destroy(sub.gameObject);
            }
            ScrollContentsRect.sizeDelta = new Vector2(0, initialScrollHeight);
            subWorkoutInputs.Clear();
        }
    }

    private SubWorkoutInput AddSubWorkInput(SubWorkoutInfo info)
    {
        var subworkObj = Instantiate(SubWorkPrefab, SubWorkObjRoot.transform);

        var subworkRect = subworkObj.GetComponent<RectTransform>();
        subworkRect.anchoredPosition = new Vector2(0, -(subworkRect.rect.height) * subWorkoutInputs.Count);
        ScrollContentsRect.sizeDelta = new Vector2(0, ScrollContentsRect.rect.height + subworkRect.rect.height);

        var subWorkInput = subworkObj.GetComponent<SubWorkoutInput>();
        info ??= new SubWorkoutInfo("보조 운동", 0, 0);
        subWorkInput.SetInfo(info, this);
        subWorkoutInputs.Add(subWorkInput);

        return subWorkInput;
    }

    public void DeleteSubWorkInput(SubWorkoutInput target)
    {
        subWorkoutInputs.Remove(target);
        Destroy(target.gameObject);
        float subInputHeightSum = 0;
        for(int i = 0; i < subWorkoutInputs.Count; ++i)
        {
            var rect = subWorkoutInputs[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -(rect.rect.height) * i);
            subInputHeightSum += rect.rect.height;
        }
        ScrollContentsRect.sizeDelta = new Vector2(0, initialScrollHeight + subInputHeightSum);
    }

    public void OnClickLoadCurrentWeight()
    {
        var userdata = DataManager.Instance.UserData;
        if (userdata.RecentDay == -1)
            return;

        SquatInput.text = GetCurrentWeight(userdata.SquatInfo, userdata.MainUnitWeight).ToString();
        BenchPressInput.text = GetCurrentWeight(userdata.BenchPressInfo, userdata.MainUnitWeight).ToString();
        DeadLiftInput.text = GetCurrentWeight(userdata.DeadLiftInfo, userdata.MainUnitWeight).ToString();
        PendlayRowInput.text = GetCurrentWeight(userdata.PendlayRowInfo, userdata.MainUnitWeight).ToString();
        OverHeadPressInput.text = GetCurrentWeight(userdata.OverHeadPressInfo, userdata.MainUnitWeight).ToString();

        foreach (var subInput in subWorkoutInputs)
        {
            subInput.LoadCurrentWeight();
        }
    }

    public static float GetCurrentWeight(WorkoutInfo info, float unitWeight)
    {
        int curWeek = Math.Max(info.Week, 4);
        return WorkOutManager.GetMainSetWeight(info.Weight, unitWeight, curWeek - 1);
    }

    public void OnClickAddSubWork()
    {
        AddSubWorkInput(null);
    }

    public void OnClickSave()
    {
        var newData = new WorkOutData();
        try
        {
            newData.SquatInfo.Weight = float.Parse(SquatInput.text);
            newData.BenchPressInfo.Weight = float.Parse(BenchPressInput.text);
            newData.DeadLiftInfo.Weight = float.Parse(DeadLiftInput.text);
            newData.PendlayRowInfo.Weight = float.Parse(PendlayRowInput.text);
            newData.OverHeadPressInfo.Weight = float.Parse(OverHeadPressInput.text);

            newData.MainUnitWeight = float.Parse(MainUnitWeightInput.text);

            if(subWorkoutInputs != null)
            {
                newData.SubWorkOutList = new List<SubWorkoutInfo>();
                foreach(var subInput in subWorkoutInputs)
                {
                    newData.SubWorkOutList.Add(subInput.GetAppliedData());
                }
            }

            DataManager.Instance.UserData = newData;
            DataManager.Instance.SaveData();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Parsing Weight Error :\n{ex}");
        }
    }
}
