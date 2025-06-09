using TMPro;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class ClickIndicator : MonoBehaviour
{
    [Header("зависимости")]
    [SerializeField] private InputHandler _inputHandler;

    [Header("настройка анимации")]
    [SerializeField] private float _displayDuration = 0.3f;
    [SerializeField] private float _fadeDuration = 0.1f;
    [SerializeField] private float _maxAlpha = 1f;
    [SerializeField] private float _minAlpha = 0f;

    public event System.Action ClickShowed;

    private TMP_Text _text;
    private Coroutine _fadeCoroutine;
    private Color _originalColor;

    private void OnValidate()
    {
        Debug.Assert(_inputHandler != null, "InputHandler не назначен!", this);
        GetTextComponent();
    }

    private void Awake()
    {
        GetTextComponent();
        CacheOriginalColor();
    }

    private void OnEnable() => _inputHandler.Clicked += HandleClick;

    private void OnDisable() => _inputHandler.Clicked -= HandleClick;

    private void GetTextComponent()
    {
        _text = GetComponent<TMP_Text>();
        Debug.Assert(_text != null, "TMP_Text component не нацден!", this);
    }

    private void CacheOriginalColor()
    {
        if (_text != null)
            _originalColor = _text.color;
    }

    private void HandleClick()
    {
        UpdatePosition();
        RestartFade();
    }

    private void UpdatePosition() => _text.transform.position = Input.mousePosition;

    private void RestartFade()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeAnimation());
    }

    private IEnumerator FadeAnimation()
    {
        ShowText();
        yield return new WaitForSeconds(_displayDuration);
        yield return FadeOut();
        HideText();
        _fadeCoroutine = null;
    }

    private void ShowText()
    {
        _text.gameObject.SetActive(true);
        _text.color = _originalColor;
        ClickShowed?.Invoke();
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            UpdateAlpha(elapsed);
            yield return null;
        }
    }

    private void UpdateAlpha(float elapsed)
    {
        float progress = elapsed / _fadeDuration;
        float alpha = Mathf.Lerp(_maxAlpha, _minAlpha, progress);
        _text.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
    }

    private void HideText() => _text.gameObject.SetActive(false);
}
