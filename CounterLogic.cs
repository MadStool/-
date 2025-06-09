using UnityEngine;
using System.Collections;

public class CounterLogic : MonoBehaviour
{
    [Header("зависимости")]
    [SerializeField] private InputHandler _inputHandler;

    [Header("Настройки")]
    [SerializeField] private float _interval = 0.5f;

    public event System.Action<int> CountChanged;
    public event System.Action CountingStarted;
    public event System.Action CountingStopped;

    private int _currentValue = 0;
    private bool _isActive = false;
    private Coroutine _countingRoutine;

    private void OnValidate()
    {
        Debug.Assert(_inputHandler != null, "InputHandler не назначен!", this);
    }

    private void OnEnable() => _inputHandler.Clicked += Toggle;

    private void OnDisable() => _inputHandler.Clicked -= Toggle;

    private void Toggle()
    {
        _isActive = _isActive == false;

        if (_isActive)
            StartCounting();
        else
            StopCounting();
    }

    private void StartCounting()
    {
        StopCounting();
        CountingStarted?.Invoke();
        _countingRoutine = StartCoroutine(CountingRoutine());
    }

    private void StopCounting()
    {
        if (_countingRoutine != null)
        {
            StopCoroutine(_countingRoutine);
            _countingRoutine = null;
            CountingStopped?.Invoke();
        }
    }

    private IEnumerator CountingRoutine()
    {
        var wait = new WaitForSeconds(_interval);

        while (_isActive)
        {
            yield return wait;
            Increase();
        }
    }

    private void Increase()
    {
        _currentValue++;
        CountChanged?.Invoke(_currentValue);
    }
}
