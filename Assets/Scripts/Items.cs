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
    public Vector3 initPos;
    public Sprite extra;
    public Boolean canPuncture;
    public Boolean punctureable;

    // Use this for initialization
    void Start () {
        clickable = true;
        initPos = this.transform.position;
        interacted = false;
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void SetPosition(Vector3 finalPosition)
    {
        this.transform.position = finalPosition;
        //this.transform.rotation = Quaternion.Euler(0, 0, 0);

    }

}
