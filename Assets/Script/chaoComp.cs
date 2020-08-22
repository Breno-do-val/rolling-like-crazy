using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaoComp : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<jogadorComportamento>())
        {
            collision.rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
    
}
