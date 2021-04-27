using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePanel : MonoBehaviour
{
    public void Activate(GameObject panel)
    {
        panel.SetActive(true);
    }
    
    public void Deactivate(GameObject panel)
    {
        panel.SetActive(false);
    }
}
