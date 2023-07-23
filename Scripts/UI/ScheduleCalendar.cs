using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class ScheduleCalendar : MonoBehaviour
{
    private ScheduleContents _essentialScheduleContents;

    private void Start()
    {
        _essentialScheduleContents = GameObject.Find("EssentialSchedule").GetComponentInChildren<ScheduleContents>();

        SetEssentailSchedule();
    }

    public void SetEssentailSchedule()
    {
        int _currentIndex = GetComponent<HorizontalScrollSnap>().CurrentPage;
        _essentialScheduleContents.ContentsScheduleType = AfterGameManager.Instance.Schedules.Find(schedule => schedule.SiblingIndex == _currentIndex).EssentialScheduleType;
    }
}