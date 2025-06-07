using TMPro;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class ClickIndicator : MonoBehaviour
{
    private const string InputHandlerMissingErrorMessage = "InputHandler не найден на сцене!";
    private const string TextComponentMissingErrorMessage = "Компонент TMP_Text отсутствует!";

    [Header("Animation Settings")]
    [SerializeField] private float _displayDurationInSeconds = 0.3f;
    [SerializeField] private float _fadeAnimationDurationInSeconds = 0.1f;
    [SerializeField] private float _fullyVisibleAlpha = 1f;
    [SerializeField] private float _fullyTransparentAlpha = 0f;

    private TMP_Text _clickIndicatorText;
    private InputHandler _inputHandler;
    private Coroutine _currentFadeAnimationCoroutine;
    private Color _originalTextColor;

    private void Awake()
    {
        InitializeComponents();
        CacheOriginalTextColor();
        ValidateDependencies();
    }

    private void InitializeComponents()
    {
        _clickIndicatorText = GetComponent<TMP_Text>();
        _inputHandler = FindAnyObjectByType<InputHandler>();
    }

    private void CacheOriginalTextColor()
    {
        if (_clickIndicatorText != null)
            _originalTextColor = _clickIndicatorText.color;
    }

    private void ValidateDependencies()
    {
        if (_inputHandler == null)
        {
            Debug.LogError(InputHandlerMissingErrorMessage, this);
            enabled = false;
            return;
        }

        if (_clickIndicatorText == null)
        {
            Debug.LogError(TextComponentMissingErrorMessage, this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
            _inputHandler.OnClick += HandleClickInput;
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
            _inputHandler.OnClick -= HandleClickInput;
    }

    private void HandleClickInput()
    {
        UpdateIndicatorPosition();
        RestartFadeAnimation();
    }

    private void UpdateIndicatorPosition() => _clickIndicatorText.transform.position = Input.mousePosition;

    private void RestartFadeAnimation()
    {
        if (_currentFadeAnimationCoroutine != null)
            StopCoroutine(_currentFadeAnimationCoroutine);

        _currentFadeAnimationCoroutine = StartCoroutine(PlayFadeAnimation());
    }

    private IEnumerator PlayFadeAnimation()
    {
        ShowTextWithOriginalColor();
        yield return WaitForDisplayDuration();
        yield return FadeOutText();
        HideText();
        ResetAnimationCoroutine();
    }

    private void ShowTextWithOriginalColor()
    {
        _clickIndicatorText.gameObject.SetActive(true);
        _clickIndicatorText.color = _originalTextColor;
    }

    private WaitForSeconds WaitForDisplayDuration()
    {
        return new WaitForSeconds(_displayDurationInSeconds);
    }

    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _fadeAnimationDurationInSeconds)
        {
            elapsedTime += Time.deltaTime;
            UpdateTextAlpha(elapsedTime);
            yield return null;
        }
    }

    private void UpdateTextAlpha(float elapsedTime)
    {
        float progress = elapsedTime / _fadeAnimationDurationInSeconds;
        float currentAlpha = Mathf.Lerp(_fullyVisibleAlpha, _fullyTransparentAlpha, progress);
        _clickIndicatorText.color = new Color(
            _originalTextColor.r,
            _originalTextColor.g,
            _originalTextColor.b,
            currentAlpha
        );
    }

    private void HideText() => _clickIndicatorText.gameObject.SetActive(false);

    private void ResetAnimationCoroutine() => _currentFadeAnimationCoroutine = null;
}