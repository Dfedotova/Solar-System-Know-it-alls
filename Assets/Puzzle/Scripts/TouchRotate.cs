using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!GameControl.Win)
            transform.Rotate(0f, 0f, 90f);
    }
}