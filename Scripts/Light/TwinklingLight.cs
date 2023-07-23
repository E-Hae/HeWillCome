using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TwinklingLight : MonoBehaviour
{
    public bool twinkle;

    [SerializeField] private float _periodSeconds = 1f;

    private Light2D _light;

    private float _intensity;

    private float _radian;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _radian = 0f;
    }

    private void Update()
    {
        if (twinkle)
        {
            _radian += 360 * Time.deltaTime * Mathf.Deg2Rad;
            _intensity = Mathf.Sin(_radian / _periodSeconds) + 2f;
            _light.intensity = _intensity;
        }
    }
}