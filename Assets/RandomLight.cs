using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomLight : MonoBehaviour
{
    Color color1;
    Color color2;
    float counter = 0;


    void Start()
    {
        color1 = Random.ColorHSV();
        color2 = Random.ColorHSV();
    }

    void Update()
    {
        GetComponent<Light>().color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time, 1));

        counter += Time.deltaTime;
        if (counter < 1) return;
        counter = 0;

        color1 = Random.ColorHSV();
        color2 = Random.ColorHSV();
    }
}
