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

    private TMP_Text _text;
    private Coroutine _fadeCoroutine;
    private Color _originalColor;

    private void OnValidate()
    {
        Debug.Assert(_inputHandler != null, "InputHandler не назначен!", this);
        TryGetComponent(out _text);
    }

    private void Awake()
    {
        if (_text == null)
            TryGetComponent(out _text);

        if (_text != null)
            _originalColor = _text.color;
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
            _inputHandler.Clicked += HandleClick;
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
            _inputHandler.Clicked -= HandleClick;
    }

    private void HandleClick()
    {
        if (_text == null) return;

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
        _text.gameObject.SetActive(true);
        _text.color = _originalColor;

        yield return new WaitForSeconds(_displayDuration);

        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(_maxAlpha, _minAlpha, elapsed / _fadeDuration);
            _text.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
            yield return null;
        }

        _text.gameObject.SetActive(false);
        _fadeCoroutine = null;
    }
}
