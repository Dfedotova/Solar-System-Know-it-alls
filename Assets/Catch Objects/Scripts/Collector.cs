using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Planet") || target.CompareTag("BlackHole") || target.CompareTag("UFO"))
            Destroy(target.gameObject);
    }
}