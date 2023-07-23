using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager Instance { get; private set; }

    [Header("Progress Text")]
    //읽는 중 텍스트
    [SerializeField] private Text _readingText;

    //불러오는 중 텍스트
    [SerializeField] private Text _loadingText;

    //텍스트가 나타나고 사라지기까지 걸리는 시간
    [SerializeField] private float _timeToShowAndHide;

    public AsyncOperation Async { get; private set; }

    private void Awake()
    {
        //초기화
        if (Instance == null)
        {
            Instance = this;

            Async = null;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 비동기 로드
    /// </summary>
    /// <param name="_sceneIndex"> 씬 인덱스 </param>
    /// <param name="_timeToFade"> 페이드 시간 </param>
    public void LoadScene(int _sceneIndex, float _timeToFade)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(_sceneIndex, _timeToFade));
    }

    //비동기 로드 코루틴
    private IEnumerator LoadSceneAsyncCoroutine(int _sceneIndex, float _timeToFade)
    {
        //페이드중이면 기다렸다가 실행
        while (Fade.Instance.isFading)
        {
            yield return null;
        }

        //페이드-아웃
        yield return StartCoroutine(Fade.Instance.FadeOutCoroutine(_timeToFade));

        //비동기 로드
        this.Async = SceneManager.LoadSceneAsync(_sceneIndex);
        this.Async.allowSceneActivation = false;

        //읽는중 보이기
        yield return StartCoroutine(this.ShowProgressTextCoroutine(_readingText));

        while (this.Async.progress < 0.9f)
        {
            yield return null;
        }

        //읽는중 숨기고 불러오는중 보이기
        yield return StartCoroutine(this.HideProgressTextCoroutine(_readingText));
        yield return StartCoroutine(this.ShowProgressTextCoroutine(_loadingText));

        //씬 이동 활성화
        this.Async.allowSceneActivation = true;

        //비동기 작업이 끝났으면 null로 초기화
        while (!this.Async.isDone)
        {
            yield return null;
        }
        this.Async = null;

        //텍스트 사라지고 페이드아웃
        yield return StartCoroutine(this.HideProgressTextCoroutine(_loadingText));
        yield return StartCoroutine(Fade.Instance.FadeInCoroutine(_timeToFade));
    }

    //텍스트 알파값 증가 코루틴
    private IEnumerator ShowProgressTextCoroutine(Text _progressText)
    {
        _progressText.gameObject.SetActive(true);

        Color _textColor = Color.white;
        _textColor.a = 0f;

        while (_textColor.a < 1f)
        {
            _textColor.a += (1f / _timeToShowAndHide) * Time.deltaTime;
            _progressText.color = _textColor;

            yield return null;
        }
    }

    //텍스트 알파값 감소 코루틴
    private IEnumerator HideProgressTextCoroutine(Text _progressText)
    {
        Color _textColor = Color.white;
        _textColor.a = 1f;

        while (_textColor.a > 0f)
        {
            _textColor.a -= (1f / _timeToShowAndHide) * Time.deltaTime;
            _progressText.color = _textColor;

            yield return null;
        }

        _progressText.gameObject.SetActive(false);
    }
}