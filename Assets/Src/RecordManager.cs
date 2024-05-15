using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    public Text SquatWeightText;
    public Text BenchPressWeightText;
    public Text DeadLiftWeightText;
    public Text PendlayRowWeightText;
    public Text OverHeadPressWeightText;
    public Text Big3WeightSumText;

    public ScrollRect Scroll;

    public GameObject[] WeightRankImages;

    private void OnEnable()
    {
        var userData = DataManager.Instance.UserData;

        int big3Sum = 0;
        int weight = Get1RMWeight(SetInfoManager.GetCurrentWeight(userData.SquatInfo, userData.MainUnitWeight));
        big3Sum += weight;
        SquatWeightText.text = weight.ToString();
        weight = Get1RMWeight(SetInfoManager.GetCurrentWeight(userData.BenchPressInfo, userData.MainUnitWeight));
        big3Sum += weight;
        BenchPressWeightText.text = weight.ToString();
        weight = Get1RMWeight(SetInfoManager.GetCurrentWeight(userData.DeadLiftInfo, userData.MainUnitWeight));
        big3Sum += weight;
        DeadLiftWeightText.text = weight.ToString();

        PendlayRowWeightText.text = Get1RMWeight(SetInfoManager.GetCurrentWeight(userData.PendlayRowInfo, userData.MainUnitWeight)).ToString();
        OverHeadPressWeightText.text = Get1RMWeight(SetInfoManager.GetCurrentWeight(userData.OverHeadPressInfo, userData.MainUnitWeight)).ToString();

        Big3WeightSumText.text = big3Sum.ToString();

        Scroll.verticalNormalizedPosition = 1f;
    }

    private int Get1RMWeight(float fiveRepsWeight)
    {
        return Mathf.RoundToInt(fiveRepsWeight / (1.0278f - (0.0278f * 5)));
    }

    public void OnClickRankButton(int type)
    {
        foreach(var img in WeightRankImages)
        {
            img.SetActive(false);
        }
        WeightRankImages[type].SetActive(true);
    }
}
