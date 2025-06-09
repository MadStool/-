using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event System.Action Clicked;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Clicked?.Invoke();
    }
}
