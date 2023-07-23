using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheftInteraction : MouseInteraction
{
    [SerializeField] private Transform _catTransform;
    [SerializeField] private float _catMovementSeconds;

    private Coroutine _moveCatCoroutine;

    private float _lastMousePosX;

    protected override void InputMouseLBDown()
    {
        this._lastMousePosX = Input.mousePosition.x;

        //부술 수 있는 장애물 클릭시 부수기
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _raycast = Physics2D.Raycast(_mouseWorldPosition, Vector2.zero, 0f);
        if (_raycast.collider != null)
        {
            GameObject hitObject = _raycast.collider.gameObject;

            hitObject.GetComponent<IInteractionObject>()?.Interact();
        }
    }

    protected override void InputMouseLBUp()
    {
        //고양이 좌우 이동
        if (this._lastMousePosX - Input.mousePosition.x < -50f)
        {
            if (_catTransform.localPosition.x == -2.3f || _catTransform.localPosition.x == 0f)
            {
                if (_moveCatCoroutine != null)
                {
                    StopCoroutine(_moveCatCoroutine);
                }
                _moveCatCoroutine = StartCoroutine(MoveCatCoroutine(_catTransform.localPosition.x + 2.3f));
            }
        }
        else if (_lastMousePosX - Input.mousePosition.x > 50f)
        {
            if (_catTransform.localPosition.x == 2.3f || _catTransform.localPosition.x == 0f)
            {
                if (_moveCatCoroutine != null)
                {
                    StopCoroutine(_moveCatCoroutine);
                }
                _moveCatCoroutine = StartCoroutine(MoveCatCoroutine(_catTransform.localPosition.x - 2.3f));
            }
        }
    }

    private IEnumerator MoveCatCoroutine(float _arrivalPosX)
    {
        float _time = 0f;
        Vector3 startPosition = _catTransform.localPosition;
        float valuePerSeconds = 1f / _catMovementSeconds;
        while (_catTransform.localPosition.x != _arrivalPosX)
        {
            _time += valuePerSeconds * Time.deltaTime;
            _catTransform.localPosition = new Vector3(Mathf.Lerp(startPosition.x, _arrivalPosX, _time), startPosition.y, startPosition.z);
            yield return null;
        }
    }
}