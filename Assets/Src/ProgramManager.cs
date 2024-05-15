using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramManager : MonoBehaviour
{
    public GameObject MainPage;
    public GameObject BackButton;
    public GameObject[] SubPages;

    private GameObject currentPage;

    public static ProgramManager Instance;

    void Start()
    {
        Instance = this;
        DataManager.Instance.LoadData();

        BackButton.SetActive(false);
        foreach(var sub in SubPages)
        {
            sub.SetActive(false);
        }
        MainPage.SetActive(true);
    }

    public void OnClickBackButton()
    {
        currentPage.SetActive(false);
        MainPage.SetActive(true);
        BackButton.SetActive(false);
    }

    public void OnClickSubPage(int pageIndex)
    {
        if(pageIndex >= SubPages.Length)
        {
            Debug.LogError("Invalid Page Index");
            return;
        }

        MainPage.SetActive(false);
        BackButton.SetActive(true);
        SubPages[pageIndex].SetActive(true);
        currentPage = SubPages[pageIndex];
    }
}
