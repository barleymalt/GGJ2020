using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveObject : MonoBehaviour
{

    public static InactiveObject instance;

    public InactiveObject()
    {
        instance = this;
    }

    public void makeActive()
    {
        gameObject.SetActive(true);
    }
}