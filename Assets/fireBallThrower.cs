using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallThrower : MonoBehaviour
{
    public GameObject myPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 bar = transform.position;
        //GameObject myPrefabSpawned = Instantiate(myPrefab, bar, Quaternion.identity);
        //Rigidbody component = myPrefabSpawned.GetComponent<Rigidbody>();
        //component.AddForce(bar * 50);
        InvokeRepeating("LaunchProjectile", 0, 3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LaunchProjectile() {
        Vector3 spawnVector = transform.position;
        Vector3 bar = GameObject.Find("Cube").transform.position;

        //Vector3 bar = gameObject.transform.position("KingBlack");
        GameObject myPrefabSpawned = Instantiate(myPrefab, spawnVector, Quaternion.identity);
        Rigidbody component = myPrefabSpawned.GetComponent<Rigidbody>();
        Vector3 result = Vector3.Normalize(bar - spawnVector);
        //component.AddForce(  (bar - spawnVector)   * 10);
        component.AddForce(result * 100);
        Destroy(myPrefabSpawned, 20);
    }
}