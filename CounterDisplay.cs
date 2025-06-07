using TMPro;
using UnityEngine;

public class CounterDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private TMP_Text _clickText;
    [SerializeField] private float _clickDisplayTime = 0.3f;

    private CounterLogic _counterLogic;
    private InputHandler _inputHandler;
    private bool _isClickVisible;

    private void Awake()
    {
        _counterLogic = FindAnyObjectByType<CounterLogic>();
        _inputHandler = FindAnyObjectByType<InputHandler>();

        if (_counterText == null)
            Debug.LogError("CounterText не назначен!"", this);
    }

    private void OnEnable()
    {
        if (_counterLogic != null)
            _counterLogic.OnCountChanged += UpdateCounter;

        if (_inputHandler != null)
            _inputHandler.OnClick += ShowClick;
    }

    private void OnDisable()
    {
        if (_counterLogic != null)
            _counterLogic.OnCountChanged -= UpdateCounter;

        if (_inputHandler != null)
            _inputHandler.OnClick -= ShowClick;
    }

    private void UpdateCounter(int count)
    {
        if (_counterText != null)
            _counterText.text = $"Счётчик: {count}";
    }

    private void ShowClick()
    {
        if (_clickText == null || _isClickVisible)
            return;

        _isClickVisible = true;
        _clickText.gameObject.SetActive(true);
        _clickText.transform.position = Input.mousePosition;
        Invoke(nameof(HideClick), _clickDisplayTime);
    }

    private void HideClick()
    {
        if (_clickText != null)
        {
            _clickText.gameObject.SetActive(false);
            _isClickVisible = false;
        }
    }
}
