using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBuoyancy : MonoBehaviour
{
    [SerializeField] 
    private float surfaceOffset = 0.0f;

    [SerializeField] 
    private float waterBuoyancyFactor = 10.0f;

    [SerializeField] 
    private float pointDepth = -100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.TryGetComponent(out Swimming swimmingScript))
        {
            swimmingScript.SwitchState(true);

            pointDepth = (transform.position.y + surfaceOffset) - other.transform.position.y;
            //Debug.Log("depth: " + pointDepth);
            if (pointDepth > 0)
            {
                //Debug.Log("depth: " + pointDepth);
                other.attachedRigidbody.AddForce(Vector3.up * waterBuoyancyFactor * pointDepth, ForceMode.Force);
            }
            //other.attachedRigidbody.AddForceAtPosition(Vector3.up * 10.0f, other.);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Swimming swimmingScript))
        {
            swimmingScript.SwitchState(false);
            pointDepth = -100.0f;
        }
    }

    //void OnCollisionStay(Collision collisionInfo)
    //{
    //    if (collisionInfo.rigidbody != null)
    //    {
    //        if (collisionInfo.gameObject.TryGetComponent(out Swimming swimScript))
    //        {
    //            var contacts = new ContactPoint[collisionInfo.contactCount];

    //            collisionInfo.GetContacts(contacts);

    //            float pointDepth = collisionInfo.transform.position.y - (transform.position.y);
    //            Debug.Log("depth: " + pointDepth);
    //            foreach (ContactPoint contact in contacts)
    //            {
    //                //float pointDepth = (transform.position.y) - contact.point.y;

    //                Debug.DrawRay(contact.point, Vector3.up * 10, Color.white);
    //                Debug.Log("colision stay");

    //                //collisionInfo.rigidbody.AddForceAtPosition(Vector3.up * 0.1f * Mathf.Abs(pointDepth), contact.point, ForceMode.Force);
    //            }
    //        }
    //    }
    //}
}
