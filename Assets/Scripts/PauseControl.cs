using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl
{
    //public static bool isGamePause;
    public bool isGamePause;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void PauseGame()
    {
        isGamePause = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = float.Epsilon;
        //Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        isGamePause = false;
    }

    public bool IsGamePaused()
    {
        return isGamePause;
    }
}
