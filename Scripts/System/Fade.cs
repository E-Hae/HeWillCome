using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private static Fade _instance;

    public static Fade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Fade>();
            }

            return _instance;
        }
    }

    /// <summary>
    /// fade 중이면 true
    /// </summary>
    public bool isFading = false;

    [SerializeField] private Image _fadingImage;

    /// <summary>
    /// 화면이 어두워지는 효과
    /// </summary>
    /// <param name="_timeToFade"></param>
    /// <returns></returns>
    public IEnumerator FadeOutCoroutine(float _timeToFade)
    {
        isFading = true;

        Color _fadeColor = this._fadingImage.color;

        while (_fadeColor.a < 1)
        {
            _fadeColor.a += (1f / _timeToFade) * Time.deltaTime;
            this._fadingImage.color = _fadeColor;

            yield return null;
        }

        isFading = false;
    }

    /// <summary>
    /// 화면이 보이는 효과
    /// </summary>
    /// <param name="_timeToFade"></param>
    /// <returns></returns>
    public IEnumerator FadeInCoroutine(float _timeToFade)
    {
        isFading = true;

        Color _fadeColor = this._fadingImage.color;

        while (_fadeColor.a > 0f)
        {
            _fadeColor.a -= (1f / _timeToFade) * Time.deltaTime;
            this._fadingImage.color = _fadeColor;

            yield return null;
        }

        isFading = false;
    }
}