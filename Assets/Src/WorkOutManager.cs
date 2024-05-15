using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkOutManager : MonoBehaviour
{
    public GameObject WorkoutPrefab;
    public WorkoutPrefab[] MainWorkOuts;
    public GameObject[] DayButtonSelectedImages;
    public ScrollRect Scroll;
    public RectTransform ScrollContentsRect;
    public GameObject SubWorkPrefabRoot;

    public const float SET_WEIGHT_DIFF = 0.125f;

    private WorkOutData data;
    private int curDay = -1;
    private List<WorkoutPrefab> subWorkPrefabList = new List<WorkoutPrefab>();
    private float initialScrollHeight;

    private void Awake()
    {
        initialScrollHeight = ScrollContentsRect.rect.height;
    }

    private void OnEnable()
    {
        data = DataManager.Instance.UserData;
        OnClickDayButton((data.RecentDay + 1) % 3);
        Scroll.verticalNormalizedPosition = 1f;
    }

    public void OnClickDayButton(int day)
    {
        switch (day)
        {
            case 0:
                MainWorkOuts[0].SetWorkoutInfo("스쿼트", data.SquatInfo, data.MainUnitWeight);
                MainWorkOuts[0].SetWeightForDay1(true);
                MainWorkOuts[1].SetWorkoutInfo("벤치프레스", data.BenchPressInfo, data.MainUnitWeight);
                MainWorkOuts[1].SetWeightForDay1(true);
                MainWorkOuts[2].SetWorkoutInfo("펜들레이 로우", data.PendlayRowInfo, data.MainUnitWeight);
                MainWorkOuts[2].SetWeightForDay1(false);
                break;
            case 1:
                MainWorkOuts[0].SetWorkoutInfo("데드리프트", data.DeadLiftInfo, data.MainUnitWeight);
                MainWorkOuts[0].SetWeightForDL();
                MainWorkOuts[1].SetWorkoutInfo("오버헤드 프레스", data.OverHeadPressInfo, data.MainUnitWeight);
                MainWorkOuts[1].SetWeightForOHP();
                MainWorkOuts[2].SetWorkoutInfo("스쿼트", data.SquatInfo, data.MainUnitWeight);
                MainWorkOuts[2].SetLightWeight();
                break;
            case 2:
                MainWorkOuts[0].SetWorkoutInfo("스쿼트", data.SquatInfo, data.MainUnitWeight);
                MainWorkOuts[0].SetWeightForDay3(true);
                MainWorkOuts[1].SetWorkoutInfo("벤치프레스", data.BenchPressInfo, data.MainUnitWeight);
                MainWorkOuts[1].SetWeightForDay3(true);
                MainWorkOuts[2].SetWorkoutInfo("펜들레이 로우", data.PendlayRowInfo, data.MainUnitWeight);
                MainWorkOuts[2].SetWeightForDay3(true);
                break;
        }
        List<SubWorkoutInfo> subInfoList = new List<SubWorkoutInfo>();
        if(data.SubWorkOutList != null)
        {
            foreach (var sub in data.SubWorkOutList)
            {
                if (sub.IsTargetDay[day])
                {
                    subInfoList.Add(sub);
                }
            }
        }
        
        if(subInfoList.Count <= subWorkPrefabList.Count)
        {
            for(int i = 0; i < subInfoList.Count; ++i)
            {
                subWorkPrefabList[i].gameObject.SetActive(true);
            }
            for(int i = subInfoList.Count; i < subWorkPrefabList.Count; ++i)
            {
                subWorkPrefabList[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for(int i = 0; i < subWorkPrefabList.Count; ++i)
            {
                subWorkPrefabList[i].gameObject.SetActive(true);
            }
            for(int i = subWorkPrefabList.Count; i < subInfoList.Count; ++i)
            {
                var subPrefabObj = Instantiate(WorkoutPrefab, SubWorkPrefabRoot.transform);
                subWorkPrefabList.Add(subPrefabObj.GetComponent<WorkoutPrefab>());
                var subPrefabRect = subPrefabObj.GetComponent<RectTransform>();
                subPrefabRect.anchoredPosition = new Vector2(0, -i * subPrefabRect.sizeDelta.y);
            }
        }
        ScrollContentsRect.sizeDelta = new Vector2(0, initialScrollHeight + subInfoList.Count * WorkoutPrefab.GetComponent<RectTransform>().sizeDelta.y);

        for(int i = 0; i < subInfoList.Count; ++i)
        {
            var info = subInfoList[i];
            subWorkPrefabList[i].SetWorkoutInfo(info.Name, info, info.UnitWeight, false);
            subWorkPrefabList[i].SetSubWorkoutWeight();
        }

        for(int i = 0; i < DayButtonSelectedImages.Length; ++i)
        {
            DayButtonSelectedImages[i].SetActive(false);
        }
        DayButtonSelectedImages[day].SetActive(true);

        curDay = day;
    }

    public void OnClickComplete()
    {
        foreach(var work in MainWorkOuts)
        {
            work.CheckClearedAndSetWeek();
        }
        foreach(var subWork in subWorkPrefabList)
        {
            subWork.CheckClearedAndSetWeek();
        }
        data.RecentDay = curDay;

        DataManager.Instance.SaveData();
        ProgramManager.Instance.OnClickBackButton();
    }

    public static float GetMainSetWeightOriginal(float baseWeight, float unitWeight, int week)
    {
        float startWeight = Mathf.RoundToInt(baseWeight * Mathf.Pow((1 / 1.025f), 3) / unitWeight) * unitWeight;
        int unitCount = Mathf.RoundToInt(startWeight * Mathf.Pow(1.025f, week) / unitWeight);

        return unitCount * unitWeight;
    }

    public static float GetMainSetWeight(float baseWeight, float unitWeight, int week)
    {
        if (baseWeight <= 0 || unitWeight <= 0)
            return 0;

        float normBaseWeight = Mathf.RoundToInt(baseWeight / unitWeight) * unitWeight;

        return normBaseWeight + unitWeight * (week - 3);
    }

    public static float GetSetWeightForMainWork(int currentSetNum, int mainSetNum, float mainSetWeight, float unitWeight)
    {
        if (mainSetWeight <= 0 || unitWeight <= 0)
            return 0;

        int unitCount = Mathf.RoundToInt(mainSetWeight * (1 - SET_WEIGHT_DIFF * (mainSetNum - currentSetNum)) / unitWeight);

        return unitCount * unitWeight;
    }
}
