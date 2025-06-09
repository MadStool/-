using TMPro;
using UnityEngine;

public class CounterDisplay : MonoBehaviour
{
    [Header("зависимости")]
    [SerializeField] private CounterLogic _counter;
    [SerializeField] private InputHandler _inputHandler;

    [Header("UI элементы")]
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private TMP_Text _clickText;

    [Header("настройки")]
    [SerializeField] private float _clickShowTime = 0.3f;
    [SerializeField] private Color _clickColor = Color.white;

    private bool _isClickVisible;

    private void OnValidate()
    {
        Debug.Assert(_counter != null, "Counter не назначен!", this);
        Debug.Assert(_inputHandler != null, "InputHandler не назначен!", this);
        Debug.Assert(_valueText != null, "ValueText не назначен!", this);
        Debug.Assert(_clickText != null, "ClickText не назначен!", this);
    }

    private void OnEnable()
    {
        if (_counter != null && _inputHandler != null)
        {
            _counter.CountChanged += UpdateValue;
            _inputHandler.Clicked += ShowClick;
        }
    }

    private void OnDisable()
    {
        _counter.CountChanged -= UpdateValue;
        _inputHandler.Clicked -= ShowClick;
    }

    private void UpdateValue(int value)
    {
        _valueText.text = $"Счётчик: {value}";
    }

    private void ShowClick()
    {
        if (_isClickVisible) return;

        _isClickVisible = true;
        _clickText.gameObject.SetActive(true);
        _clickText.transform.position = Input.mousePosition;
        _clickText.color = _clickColor;

        Invoke(nameof(HideClick), _clickShowTime);
    }

    private void HideClick()
    {
        _clickText.gameObject.SetActive(false);
        _isClickVisible = false;
    }
}
