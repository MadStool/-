using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event System.Action OnClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClick?.Invoke();
    }
}