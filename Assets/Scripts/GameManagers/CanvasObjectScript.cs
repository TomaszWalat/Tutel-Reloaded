using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasObjectScript : MonoBehaviour
{
    // Script for toggling a UI on/off

    [SerializeField]
    ScreenManagerScript screenManager;

    [SerializeField]
    Canvas uiCanvas;

    [SerializeField]
    bool isActive;

    private void Awake()
    {
        if (uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (screenManager != null)
        {
            if (uiCanvas != null)
            {
                screenManager.AttachUI(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableUI()
    {
        isActive = true;
        uiCanvas.gameObject.SetActive(true);
    }

    public void DisableUI()
    {
        isActive = false;
        uiCanvas.gameObject.SetActive(false);
    }

    public bool IsEnabled()
    {
        return isActive;
    }
}
