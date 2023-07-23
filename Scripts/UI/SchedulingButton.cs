using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SchedulingButton : MonoBehaviour
{
    [field: SerializeField] public ScheduleType SchedulingType { get; private set; }

    private void Start()
    {
        //버튼에 클릭시 일정짜기 추가
        GetComponent<Button>().onClick.AddListener(Scheduling);
    }

    //일정짜기
    private void Scheduling()
    {
        HorizontalScrollSnap _scheduleScrollSnap = transform.parent.GetComponentInChildren<HorizontalScrollSnap>();
        Transform _contentsTransform = _scheduleScrollSnap.transform.Find("Content");

        for (var i = 0; i < _scheduleScrollSnap.CurrentPage; i++)
        {
            //이전 날짜들에 일정이 비었으면 비어있는 날짜로 스크롤 이동하고 함수 리턴
            if (_contentsTransform.GetChild(i).GetComponent<Schedule>().ScheduleTypes.Count < 2)
            {
                _scheduleScrollSnap.GoToScreen(i);
                return;
            }
        }

        //현재 스크롤 페이지에 일정 추가
        Schedule _schedule = _contentsTransform.GetChild(_scheduleScrollSnap.CurrentPage).GetComponent<Schedule>();
        _schedule.AddSchedulue(this.SchedulingType);
    }
}