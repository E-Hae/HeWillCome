using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleContents : MonoBehaviour
{
    [SerializeField] private ScheduleType _contentsScheduleType;

    public ScheduleType ContentsScheduleType
    {
        get
        {
            return _contentsScheduleType;
        }
        set
        {
            _contentsScheduleType = value;

            GetComponent<Text>().text = Contents;
        }
    }

    private string Contents
    {
        get
        {
            switch (_contentsScheduleType)
            {
                case ScheduleType.Work:
                    return "일하기";

                case ScheduleType.Shower:
                    return "목욕시키기";

                case ScheduleType.Shopping:
                    return "쇼핑하기";

                case ScheduleType.Rest:
                    return "휴식취하기";

                case ScheduleType.Playing:
                    return "놀아주기";

                default:
                    return "오류";
            }
        }
    }

    private void Awake()
    {
        GetComponent<Text>().text = Contents;
    }
}