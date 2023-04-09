using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempscript : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        //string tag = gameObject.tag;
        //Debug.Log("tag of this object: " + tag);

    }
    

    // Update is called once per frame
    void Update()
    {

    }
//    private void OnTriggerEnter(Collider other)

    //private void OnTriggerEnter(Collider other)
   // {
     //   Debug.Log("Cube WAS HIT??");
       // Destroy(other, 0);
  //  }
    private void OnTriggerEnter(Collider Collider) {
       if (Collider.gameObject.CompareTag(tag)) {
        Debug.Log("is this reaching?");
        Destroy(gameObject);
    } 
    }
       
}
