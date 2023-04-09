using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRChessInteract : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Right Hand")
        {
            Debug.Log("Enter");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Right Hand")
        {
            Debug.Log("Stay");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Right Hand")
        {
            Debug.Log("Exit");
        }
    }
}
