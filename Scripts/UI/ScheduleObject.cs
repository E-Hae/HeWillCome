using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleObject : MonoBehaviour
{
    [SerializeField] private ScheduleType _interactableScheduleType;

    private void Start()
    {
        AfterGameManager.Instance.OnNextDay += OnNextDay;
    }

    private void OnNextDay()
    {
        bool _isInteractable = AfterGameManager.Instance.GetTodaysSchedule().ScheduleTypes.Contains(_interactableScheduleType);
        GetComponent<Collider2D>().enabled = _isInteractable;
    }
}