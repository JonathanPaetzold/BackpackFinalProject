using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour {


    public float toughness;
    public float weight;
    public Boolean damaged;
    public Boolean activated;
    public Boolean interacted;
    public Boolean clickable;
    public int rows;
    public int cols;

	// Use this for initialization
	void Start () {
        clickable = true;
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void SetPosition(float x, float y, float z)
    {
        this.transform.position = new Vector3(x,y,z);
        //this.transform.rotation = Quaternion.Euler(0, 0, 0);

    }

}
