using UnityEngine;
using System.Collections;

public class CounterLogic : MonoBehaviour
{
    public event System.Action<int> OnCountChanged;

    [SerializeField] private float _countInterval = 0.5f;
    private int _currentCount = 0;
    private bool _isCounting = false;
    private Coroutine _countingCoroutine;
    private InputHandler _inputHandler;

    private void Awake()
    {
        _inputHandler = FindAnyObjectByType<InputHandler>();

        if (_inputHandler == null)
        {
            Debug.LogError("InputHandler не найден на сцене!");
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
            _inputHandler.OnClick += ToggleCounting;
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
            _inputHandler.OnClick -= ToggleCounting;
    }

    private void ToggleCounting()
    {
        _isCounting = _isCounting == false;

        if (_isCounting)
            StartCounting();
        else
            StopCounting();
    }

    private void StartCounting()
    {
        if (_countingCoroutine != null)
            StopCoroutine(_countingCoroutine);

        _countingCoroutine = StartCoroutine(CountingRoutine());
    }

    private void StopCounting()
    {
        if (_countingCoroutine != null)
        {
            StopCoroutine(_countingCoroutine);
            _countingCoroutine = null;
        }
    }

    private IEnumerator CountingRoutine()
    {
        var wait = new WaitForSeconds(_countInterval);

        while (_isCounting)
        {
            yield return wait;
            IncrementCounter();
        }
    }

    private void IncrementCounter()
    {
        _currentCount++;
        OnCountChanged?.Invoke(_currentCount);
    }
}