using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SchedulingButton : MonoBehaviour
{
    [field: SerializeField] public ScheduleType SchedulingType { get; private set; }

    private void Start()
    {
        //��ư�� Ŭ���� ����¥�� �߰�
        GetComponent<Button>().onClick.AddListener(Scheduling);
    }

    //����¥��
    private void Scheduling()
    {
        HorizontalScrollSnap _scheduleScrollSnap = transform.parent.GetComponentInChildren<HorizontalScrollSnap>();
        Transform _contentsTransform = _scheduleScrollSnap.transform.Find("Content");

        for (var i = 0; i < _scheduleScrollSnap.CurrentPage; i++)
        {
            //���� ��¥�鿡 ������ ������� ����ִ� ��¥�� ��ũ�� �̵��ϰ� �Լ� ����
            if (_contentsTransform.GetChild(i).GetComponent<Schedule>().ScheduleTypes.Count < 2)
            {
                _scheduleScrollSnap.GoToScreen(i);
                return;
            }
        }

        //���� ��ũ�� �������� ���� �߰�
        Schedule _schedule = _contentsTransform.GetChild(_scheduleScrollSnap.CurrentPage).GetComponent<Schedule>();
        _schedule.AddSchedulue(this.SchedulingType);
    }
}