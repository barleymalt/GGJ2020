using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class palla : MonoBehaviour
{
    
    void Update()
    {
        if (gameObject.transform.position.y < -5)
            gameObject.transform.position = new Vector3(0,2,0);
    }
}
