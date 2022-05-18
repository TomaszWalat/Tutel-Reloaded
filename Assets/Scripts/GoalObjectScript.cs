using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObjectScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Fill in only if this a child of another goal (part of a daisy chain) - i.e. leave empty for Level Manager")]
    // Parent of this goal, null is this is the highest level parent
    protected GoalObjectScript parent;

    [SerializeField]
    // Goal completion status, if this has sub-goals, they must all be complete
    protected bool isComplete;

    // Set of children names and their goal scripts
    protected Dictionary<string, GoalObjectScript> children;

    // Set of children names and their goal states
    protected Dictionary<string, bool> progress;

    void Awake()
    {
        children = new Dictionary<string, GoalObjectScript>();
        progress = new Dictionary<string, bool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("This is: " + gameObject.name + " - Start() entered.");

        if(parent != null)
        {
            Debug.Log("This is: " + gameObject.name + " - Start(), if(parent != null) entered.");
            Debug.Log("This is: " + gameObject.name + " - ... -> calling parent.AttachChild(this.gameObject).");

            parent.AttachChild(this.gameObject);
        }
    }


    public bool SetParent(GameObject newParent)
    {
        Debug.Log("This is: " + gameObject.name + " - SetParent() entered.");

        bool success = false;

        if (newParent != null)
        {
            Debug.Log("This is: " + gameObject.name + " - SetParent(), if(newParent != null) entered.");

            if (newParent.TryGetComponent(out GoalObjectScript goScript))
            {
                Debug.Log("This is: " + gameObject.name + " - SetParent(), if(newParent.TryGetComponent(out GoalObjectScript goScript)) entered.");

                bool oldParentDetached = true;

                if (parent != null)
                {
                    Debug.Log("This is: " + gameObject.name + " - SetParent(), if(parent != null) entered.");
                    Debug.Log("This is: " + gameObject.name + " - ... -> calling parent.AttachChild(this.gameObject).");

                    oldParentDetached = parent.DetachChild(this.gameObject);
                }

                if(oldParentDetached)
                {
                    Debug.Log("This is: " + gameObject.name + " - SetParent(), if(oldParentDetached) entered.");

                    parent = goScript;
                    
                    Debug.Log("This is: " + gameObject.name + " - ... -> calling parent.AttachChild(this.gameObject).");

                    parent.AttachChild(this.gameObject);

                    success = true;
                }
            }
        }
        else
        {
            Debug.Log("This is: " + gameObject.name + " - SetParent(), else(newParent != null) entered.");

            if (parent != null)
            {
                Debug.Log("This is: " + gameObject.name + " - SetParent(), if(parent != null) entered.");
                
                Debug.Log("This is: " + gameObject.name + " - ... -> calling parent.DetachChild(this.gameObject).");

                parent.DetachChild(this.gameObject);

                parent = null;

                success = true;
            }
        }
        
        Debug.Log("This is: " + gameObject.name + " - SetParent(), returning bool = " + success);

        return success;
    }

    public GameObject GetParent()
    {
        Debug.Log("This is: " + gameObject.name + " - GetParent() entered.");
        
        Debug.Log("This is: " + gameObject.name + " - GetParent(), returning gameObject = " + parent.gameObject.name);

        return parent.gameObject;
    }

    public bool GetGoalState()
    {
        Debug.Log("This is: " + gameObject.name + " - GetGoalState() entered.");
        
        Debug.Log("This is: " + gameObject.name + " - GetGoalState(), returning bool = " + isComplete);
        return isComplete;
    }

    // Only works if goal is not a parent
    public virtual void SetGoalState(bool complete)
    {
        Debug.Log("This is: " + gameObject.name + " - SetGoalState() entered.");

        if(children.Count <= 0)
        {
            Debug.Log("This is: " + gameObject.name + " - SetGoalState(), if(children.Count <= 0).");

            isComplete = complete;
            
            Debug.Log("This is: " + gameObject.name + " - ... -> calling CheckProgress().");

            CheckProgress();
        }
    }

    public bool AttachChild(GameObject child)
    {
        Debug.Log("This is: " + gameObject.name + " - AttachChild() entered.");

        bool attached = false;

        if (!children.ContainsKey(child.name))
        {
            Debug.Log("This is: " + gameObject.name + " - AttachChild(), if(!children.ContainsKey(child.name)) entered.");

            if (child.TryGetComponent(out GoalObjectScript goScript))
            {
                Debug.Log("This is: " + gameObject.name + " - AttachChild(), if(child.TryGetComponent(out GoalObjectScript goScript)) entered.");
                
                Debug.Log("This is: " + gameObject.name + " - ... -> children.Add(child.name, goScript); child.name = " + child.name);
                Debug.Log("This is: " + gameObject.name + " - ... -> progress.Add(child.name, goScript.GetGoalState()); child.name = " + child.name);

                children.Add(child.name, goScript);
                progress.Add(child.name, goScript.GetGoalState());

                attached = true;
                
                Debug.Log("This is: " + gameObject.name + " - ... -> calling CheckProgress().");

                CheckProgress();
            }
        }
        
        Debug.Log("This is: " + gameObject.name + " - AttachChild(), returning bool = " + attached);

        return attached;
    }

    public bool DetachChild(GameObject child)
    {
        Debug.Log("This is: " + gameObject.name + " - DetachChild() entered.");

        bool detached = false;

        if(children.ContainsKey(child.name))
        { 
            Debug.Log("This is: " + gameObject.name + " - DetachChild(), if(children.ContainsKey(child.name)) entered.");
            Debug.Log("This is: " + gameObject.name + " - ... -> children.Remove(child.name); child.name = " + child.name);
            Debug.Log("This is: " + gameObject.name + " - ... -> progress.Remove(child.name); child.name = " + child.name);

            progress.Remove(child.name);
            children.Remove(child.name);

            detached = true;
            
            Debug.Log("This is: " + gameObject.name + " - ... -> calling CheckProgress().");

            CheckProgress();
        }
        
        Debug.Log("This is: " + gameObject.name + " - DetachChild(), returning bool = " + detached);

        return detached;
    }

    protected virtual async void CheckProgress()
    {
        Debug.Log("This is: " + gameObject.name + " - CheckProgress() entered.");

        bool childrenComplete = true;

        // Check children status
        if (children.Count > 0)
        {
            Debug.Log("This is: " + gameObject.name + " - CheckProgress(), if(children.Count > 0) entered.");

            Dictionary<string, bool>.ValueCollection values = progress.Values;
            
            Debug.Log("This is: " + gameObject.name + " - ... -> entering foreach(bool childState in values) loop");
            Debug.Log("This is: " + gameObject.name + " - ... -> pre-loop - isComplete = " + isComplete);

            // If any children are false, childrenComplete will be false
            foreach (bool childState in values)
            {
                childrenComplete = childrenComplete && childState;
            }

            isComplete = childrenComplete;
            
            Debug.Log("This is: " + gameObject.name + " - ... -> post-loop - isComplete = " + isComplete);
        }

        if(parent != null)
        {
            Debug.Log("This is: " + gameObject.name + " - CheckProgress(), if(parent != null) entered.");
            
            Debug.Log("This is: " + gameObject.name + " - ... -> calling parent.UpdateChildStatus(this.gameObject).");

            parent.UpdateChildStatus(this.gameObject);
        }
    }

    public async void UpdateChildStatus(GameObject child)
    {
        Debug.Log("This is: " + gameObject.name + " - UpdateChildStatus() entered.");

        if (children.TryGetValue(child.name, out GoalObjectScript goScript))
        {
            Debug.Log("This is: " + gameObject.name + " - UpdateChildStatus(), if(children.TryGetValue(child.name, out GoalObjectScript goScript)) entered.");
            
            Debug.Log("This is: " + gameObject.name + " - ... -> progress[child.name] = goScript.GetGoalState(); child's goal state = " + goScript.GetGoalState());

            progress[child.name] = goScript.GetGoalState();

            CheckProgress();
        }
    }
}
