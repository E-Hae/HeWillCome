using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInteraction : MonoBehaviour
{
    protected Vector2 _mouseWorldPosition;
    protected RaycastHit2D _raycast;

    private bool _isInteractable = false;
    private Coroutine _interactCoroutine;

    public bool IsInteractable
    {
        get
        {
            return _isInteractable;
        }
        set
        {
            if (_isInteractable == value)
            {
                return;
            }

            _isInteractable = value;

            if (value)
            {
                _interactCoroutine = StartCoroutine(InteractCoroutine());
            }
            else
            {
                StopCoroutine(_interactCoroutine);
                _interactCoroutine = null;
            }
        }
    }

    private IEnumerator InteractCoroutine()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InputMouseLBDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            InputMouseLBUp();
        }

        yield return null;
    }

    protected virtual void InputMouseLBDown()
    { }

    protected virtual void InputMouseLBUp()
    { }
}