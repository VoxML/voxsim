﻿using UnityEngine;
using System.Collections;

public class GraspScript : MonoBehaviour {
	private Animator anim;
	public int grasper;
	public GameObject leftGrasperCoord,rightGrasperCoord;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			grasper = 1;
			Debug.Log (grasper);
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			grasper = 2;
			Debug.Log (grasper);
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			grasper = 3;
			Debug.Log (grasper);
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			grasper = 4;
			Debug.Log (grasper);
		} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
			grasper = 5;
			Debug.Log (grasper);
		} else if (Input.GetKeyDown (KeyCode.Alpha6)) {
			grasper = 6;
			Debug.Log (grasper);
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			grasper = 0;
			Debug.Log (grasper);
		}
		anim.SetInteger ("anim", grasper);
	}
}
