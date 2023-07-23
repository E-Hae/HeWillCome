using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    [SerializeField] private ScreenOrientation screenOrientation;

    private void Start()
    {
        ResolutionFix();
    }

    public void ResolutionFix()
    {
        float targetWidthAspect;
        float targetHeightAspect;

        if (screenOrientation == ScreenOrientation.Landscape)
        {
            targetWidthAspect = 16f;
            targetHeightAspect = 9f;
        }
        else
        {
            targetWidthAspect = 9f;
            targetHeightAspect = 16f;
        }

        float targetUIWidthAspect = targetWidthAspect;
        float targetUIHeightAspect = targetHeightAspect;

        float targetWidthAspectPort = targetWidthAspect / targetHeightAspect;
        float targetHeightAspectPort = targetHeightAspect / targetWidthAspect;

        float targetUIWidthAspectPort = targetUIWidthAspect / targetUIHeightAspect;
        float targetUIHeightAspectPort = targetUIHeightAspect / targetUIWidthAspect;

        float currentWidthAspectPort = (float)Screen.width / (float)Screen.height;
        float currentHeightAspectPort = (float)Screen.height / (float)Screen.width;

        float viewPortW = targetWidthAspectPort / currentWidthAspectPort;
        float viewPortH = targetHeightAspectPort / currentHeightAspectPort;

        float viewPortUIW = targetUIWidthAspectPort / currentWidthAspectPort;
        float viewPortUIH = targetUIHeightAspectPort / currentHeightAspectPort;

        if (viewPortH > 1)
        {
            viewPortH = 1;
        }
        if (viewPortW > 1)
        {
            viewPortW = 1;
        }
        Camera camMain = GameObject.Find("Main Camera").GetComponent<Camera>();
        camMain.rect = new Rect((1f - viewPortW) / 2f, (1f - viewPortH) / 2f, viewPortW, viewPortH);

        if (viewPortUIH > 1)
        {
            viewPortUIH = 1;
        }
        if (viewPortUIW > 1)
        {
            viewPortUIW = 1;
        }
        Camera camUI = GameObject.Find("UICamera").GetComponent<Camera>();
        camUI.rect = new Rect((1f - viewPortUIW) / 2f, (1f - viewPortUIH) / 2f, viewPortUIW, viewPortUIH);
    }
}