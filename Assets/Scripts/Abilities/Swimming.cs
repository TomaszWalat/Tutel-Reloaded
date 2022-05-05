using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Swimming : Ability
{
    [SerializeField]
    private bool isUnderwater;
    [SerializeField] 
    private float currentDrag = -1.0f;
    [SerializeField] 
    private float currentAngularDrag = -1.0f;

    [SerializeField]
    private float underwaterDrag = 3.0f;
    [SerializeField]
    private float underwaterAngularDrag = 1f;
    [SerializeField]
    private float airDrag = 0f;
    [SerializeField]
    private float airAngularDrag = 0.05f;


    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(bool isUnderwater)
    {
        if (isUnderwater)
        {
            this.isUnderwater = isUnderwater;
            m_rigidbody.drag = underwaterDrag;
            m_rigidbody.angularDrag = underwaterAngularDrag;
            currentDrag = underwaterDrag;
            currentAngularDrag = underwaterAngularDrag;
        }
        else
        {
            this.isUnderwater = isUnderwater;
            m_rigidbody.drag = airDrag;
            m_rigidbody.angularDrag = airAngularDrag;
            currentDrag = airDrag;
            currentAngularDrag = airAngularDrag;
        }
    }
}
