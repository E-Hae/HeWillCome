using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public enum ScheduleType
{
    None, Work, Shower, Playing, Rest, Shopping
}

public class Schedule : MonoBehaviour
{
    public List<ScheduleType> ScheduleTypes { get; private set; } = new List<ScheduleType>();

    public int SiblingIndex
    {
        get
        {
            return transform.GetSiblingIndex();
        }
    }

    private int _scheduleDate = 0;

    //현재 일정의 날짜
    public int ScheduleDate
    {
        get
        {
            if (_scheduleDate == 0)
            {
                _scheduleDate = AfterGameManager.Instance.Days + SiblingIndex + 1;
            }
            return _scheduleDate;
        }
    }

    private GameObject _scheduleBox;

    //짜여진 일정이 보여지는 박스
    public GameObject ScheduleBox
    {
        get
        {
            if (_scheduleBox == null)
            {
                _scheduleBox = transform.Find("ScheduleBox").gameObject;
            }

            return _scheduleBox;
        }
    }

    private Image _scheduleBoxInfoImage;

    //일정이 짜여져 있는지 볼수있는 이미지
    public Image ScheduleBoxInfoImage
    {
        get
        {
            if (_scheduleBoxInfoImage == null)
            {
                _scheduleBoxInfoImage = GameObject.Find("ScheduleBoxInfo").GetComponentsInChildren<Image>()[SiblingIndex + 1];
            }

            return _scheduleBoxInfoImage;
        }
    }

    private ScheduleType _essentialScheduleType = 0;

    //필수 일정
    public ScheduleType EssentialScheduleType
    {
        get
        {
            if (_essentialScheduleType == 0)
            {
                List<ScheduleType> _scheduleTypes = new List<ScheduleType>() { ScheduleType.Work, ScheduleType.Playing, ScheduleType.Rest, ScheduleType.Shopping, ScheduleType.Shower };
                int _randomIndex = Random.Range(0, 100) % 5;
                _essentialScheduleType = _scheduleTypes[_randomIndex];
            }

            return _essentialScheduleType;
        }
    }

    private Dictionary<GameObject, ScheduleType> _scheduleDictionary = new Dictionary<GameObject, ScheduleType>();

    private void Start()
    {
        //스케줄 제거 이벤트 트리거 생성
        foreach (var _eventTrigger in GetComponentsInChildren<EventTrigger>())
        {
            GameObject _scheduleObject = _eventTrigger.transform.parent.gameObject;
            _scheduleDictionary.Add(_scheduleObject, ScheduleType.Work);

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { RemoveSchedule(_scheduleObject); });
            _eventTrigger.triggers.Add(entry);

            _scheduleObject.SetActive(false);
        }

        //날짜 및 요일 표시
        transform.Find("Day").GetComponent<Text>().text = ScheduleDate.ToString();
        List<string> _dayNames = new List<string> { "일", "월", "화", "수", "목", "금", "토" };
        transform.Find("DayOfTheWeek").GetComponent<Text>().text = _dayNames[ScheduleDate % 7];
    }

    //일정 초기화
    private void InitializeSchedule()
    {
        ScheduleTypes = new List<ScheduleType>();

        foreach (var _eventTrigger in GetComponentsInChildren<EventTrigger>())
        {
            _eventTrigger.transform.parent.gameObject.SetActive(false);
        }
    }

    //일정 추가
    public void AddSchedulue(ScheduleType _scheduleType)
    {
        ScheduleContents _scheduleContents;

        if (ScheduleTypes.Contains(_scheduleType))
        {
            List<ScheduleContents> scheduleContentsList = new List<ScheduleContents>(ScheduleBox.GetComponentsInChildren<ScheduleContents>());
            _scheduleContents = scheduleContentsList.Find(elem => elem.ContentsScheduleType == _scheduleType);
            RemoveSchedule(_scheduleContents.gameObject);
            return;
        }

        if (ScheduleTypes.Count == 2)
        {
            return;
        }

        _scheduleContents = ScheduleBox.transform.GetChild(ScheduleTypes.Count).GetComponent<ScheduleContents>();

        _scheduleContents.ContentsScheduleType = _scheduleType;
        _scheduleContents.gameObject.SetActive(true);

        _scheduleDictionary[_scheduleContents.gameObject] = _scheduleType;

        ScheduleTypes.Add(_scheduleType);

        if (ScheduleTypes.Count == 2)
        {
            GameObject.Find("Button_FinishSchedule").GetComponent<Button>().interactable = true;

            ScheduleBoxInfoImage.color = Color.green;

            transform.parent.parent.GetComponentInParent<HorizontalScrollSnap>().NextScreen();
        }
        else
        {
            GameObject.Find("Button_FinishSchedule").GetComponent<Button>().interactable = false;

            ScheduleBoxInfoImage.color = Color.red;
        }
    }

    //일정 제거
    public void RemoveSchedule(GameObject _scheduleObject)
    {
        switch (ScheduleTypes.Count)
        {
            case 0:
                {
                    return;
                }
            case 1:
                {
                    ScheduleBoxInfoImage.color = Color.white;
                    break;
                }
            case 2:
                {
                    ScheduleBoxInfoImage.color = Color.red;
                    break;
                }
        }

        GameObject.Find("Button_FinishSchedule").GetComponent<Button>().interactable = false;

        ScheduleTypes.Remove(_scheduleDictionary[_scheduleObject]);

        _scheduleObject.transform.SetAsLastSibling();
        _scheduleObject.SetActive(false);
    }
}