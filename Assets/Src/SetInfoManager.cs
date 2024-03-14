using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetInfoManager : MonoBehaviour
{
    public InputField[] WeightInputs;

    void OnEnable()
    {
        for(int i = 0; i < WeightInputs.Length; i++)
        {
            WeightInputs[i].text = PlayerPrefs.GetFloat(((MainWorkOutType)i).ToString(), 0f).ToString();
        }
    }

    public void OnClickSave()
    {
        for(int i = 0; i < WeightInputs.Length; i++)
        {
            PlayerPrefs.SetFloat(((MainWorkOutType)i).ToString(), float.Parse(WeightInputs[i].text));
        }
    }
}
