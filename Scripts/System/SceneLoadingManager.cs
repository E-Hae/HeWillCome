using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager Instance { get; private set; }

    [Header("Progress Text")]
    //�д� �� �ؽ�Ʈ
    [SerializeField] private Text _readingText;

    //�ҷ����� �� �ؽ�Ʈ
    [SerializeField] private Text _loadingText;

    //�ؽ�Ʈ�� ��Ÿ���� ���������� �ɸ��� �ð�
    [SerializeField] private float _timeToShowAndHide;

    public AsyncOperation Async { get; private set; }

    private void Awake()
    {
        //�ʱ�ȭ
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
    /// �񵿱� �ε�
    /// </summary>
    /// <param name="_sceneIndex"> �� �ε��� </param>
    /// <param name="_timeToFade"> ���̵� �ð� </param>
    public void LoadScene(int _sceneIndex, float _timeToFade)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(_sceneIndex, _timeToFade));
    }

    //�񵿱� �ε� �ڷ�ƾ
    private IEnumerator LoadSceneAsyncCoroutine(int _sceneIndex, float _timeToFade)
    {
        //���̵����̸� ��ٷȴٰ� ����
        while (Fade.Instance.isFading)
        {
            yield return null;
        }

        //���̵�-�ƿ�
        yield return StartCoroutine(Fade.Instance.FadeOutCoroutine(_timeToFade));

        //�񵿱� �ε�
        this.Async = SceneManager.LoadSceneAsync(_sceneIndex);
        this.Async.allowSceneActivation = false;

        //�д��� ���̱�
        yield return StartCoroutine(this.ShowProgressTextCoroutine(_readingText));

        while (this.Async.progress < 0.9f)
        {
            yield return null;
        }

        //�д��� ����� �ҷ������� ���̱�
        yield return StartCoroutine(this.HideProgressTextCoroutine(_readingText));
        yield return StartCoroutine(this.ShowProgressTextCoroutine(_loadingText));

        //�� �̵� Ȱ��ȭ
        this.Async.allowSceneActivation = true;

        //�񵿱� �۾��� �������� null�� �ʱ�ȭ
        while (!this.Async.isDone)
        {
            yield return null;
        }
        this.Async = null;

        //�ؽ�Ʈ ������� ���̵�ƿ�
        yield return StartCoroutine(this.HideProgressTextCoroutine(_loadingText));
        yield return StartCoroutine(Fade.Instance.FadeInCoroutine(_timeToFade));
    }

    //�ؽ�Ʈ ���İ� ���� �ڷ�ƾ
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

    //�ؽ�Ʈ ���İ� ���� �ڷ�ƾ
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