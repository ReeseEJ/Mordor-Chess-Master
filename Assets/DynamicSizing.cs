using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSizing : MonoBehaviour
{
    public float scalingFactor = 1.0f;
    private float timer = 0.0f;
    private bool ifScaled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.2f && ifScaled == false)
        {
            ScaleUp();
            ifScaled = true;
        }
    }

    void ScaleUp()
    {
        transform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor);
    }
}
