using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : GoalObjectScript
{
    // Script for level/gameplay related stuff

    public enum LevelState { Loading, Ready, InProgress, Paused, Failed, Complete }

    [SerializeField]
    ScreenManagerScript screenManager;

    [SerializeField]
    LevelState state = LevelState.Loading;

    bool isPaused;

    private bool isHoldingP;

    PauseControl pauseControl;

    //void Awake()
    //{
    //}

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Load());
        isPaused = false;
        isHoldingP = false;
        // state = LevelState.Loading;
        pauseControl = new PauseControl();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isHoldingP)
        {
            isHoldingP = true;
            //if (state == LevelState.InProgress || state == LevelState.Ready)
            //{
            //    state = LevelState.Paused;
            //    screenManager.GoToMenu("PauseMenu");
            //}
            //else if(state == LevelState.Paused)
            //{
            //    state = LevelState.InProgress;
            //    screenManager.GoToMenu("ResumeLevel");
            //} 
            if (!isPaused)
            {
                //isPaused = true;
                screenManager.GoToMenu("PauseMenu");
                Pause();
                //Time.timeScale = 0.0f;
            }
            else if(isPaused)
            {
                Resume();
                screenManager.GoToMenu("ResumeLevel");
                //isPaused = false;
                //Time.timeScale = 1.0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            isHoldingP = false;
        }
    }

    public string GetState()
    {
        string sState = "";

        switch (state)
        {
            case LevelState.Loading:
                sState = "Loading";
                break;
            case LevelState.Ready:
                sState = "Ready";
                break;
            case LevelState.InProgress:
                sState = "InProgress";
                break;
            case LevelState.Paused:
                sState = "Paused";
                break;
            case LevelState.Complete:
                sState = "Complete";
                break;
            default: 
                break;
        }
        return sState;
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    IEnumerator Load()
    {
        // Initialise everything with the level here
        // ----

        yield return new WaitForEndOfFrame();

        state = LevelState.Ready;
        //Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        isPaused = true;
        state = LevelState.Paused;
        pauseControl.PauseGame();
    }

    public void Resume()
    {
        pauseControl.ResumeGame();
        state = LevelState.InProgress;
        isPaused = false;
    }

    public void TriggerFailState()
    {
        isPaused = true;
        state = LevelState.Failed;
        screenManager.GoToMenu("FailStateMenu");
        pauseControl.PauseGame();
        //Time.timeScale = 0.0f;
    }

    // ---------- Methods from GoalObjectScript ---------- //

    // Only works if goal is not a parent
    public override void SetGoalState(bool complete)
    {
        if (children.Count <= 0)
        {
            isComplete = complete;

            state = LevelState.Complete;

            CheckProgress();
        }
    }

    protected override async void CheckProgress()
    {
        Debug.Log("This is: " + gameObject.name + " - CheckProgress() in LevelManagerScript entered.");

        bool childrenComplete = true;

        // Check children status
        if (children.Count > 0)
        {
            Dictionary<string, bool>.ValueCollection values = progress.Values;

            // If any children are false, childrenComplete will be false
            foreach (bool childState in values)
            {
                childrenComplete = childrenComplete && childState;
            }

            isComplete = childrenComplete;

            state = LevelState.Complete;

            if (isComplete)
            {
                screenManager.GoToMenu("VictoryMenu");
                //Time.timeScale = 0.0f;
            }
        }
    }
}
