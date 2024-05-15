using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetObject : MonoBehaviour
{
    public Text WeightText;
    public Text RepsText;
    public Toggle CheckToggle;

    public WorkoutPrefab CurWorkoutPrefab { get; set; }
    public bool IsDone { get; set; }

    public void SetText(float weight, int reps)
    {
        SetText(weight.ToString(), reps);
    }

    public void SetText(string weight, int reps)
    {
        WeightText.text = weight;
        if (reps <= 0)
            RepsText.text = "AMREP";
        else
            RepsText.text = $"{reps} reps";

        CheckToggle.isOn = false;
    }
}
