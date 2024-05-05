using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOutManager : MonoBehaviour
{
    public const float SET_WEIGHT_DIFF = 0.125f;

    private WorkOutData data;

    private void Start()
    {
        data = DataManager.Instance.UserData;
    }

    public float GetMainSetWeight(float baseWeight, float unitWeight, int week)
    {
        int unitCount = Mathf.RoundToInt(baseWeight * Mathf.Pow(1.025f, week) / unitWeight);

        return unitCount * unitWeight;
    }

    public float GetSetWeightForMainWork(int currentSetNum, int mainSetNum, float mainSetWeight)
    {
        float unitWeight = data.MainUnitWeight;
        int unitCount = Mathf.RoundToInt(mainSetWeight * (1 - SET_WEIGHT_DIFF * 2) / unitWeight);

        return unitCount * unitWeight;
    }
}
