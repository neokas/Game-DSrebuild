﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyIUserInput : IUserInput {

	// Use this for initialization
	IEnumerator Start () {
        while (true)
        {
            //Dup = 1.0f;
            //Dright = 0f;
            //Camera_right = 1.0f;
            //Camera_up = 0;
            //run = true;
            //yield return new WaitForSeconds(3.0f);
            //Dup = 0f;
            //Dright = 0f;
            //Camera_right = 0f;
            //Camera_up = 0;
            //yield return new WaitForSeconds(1.0f);

            rb = true;
            yield return 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
        UpdateDmagDvec(Dup, Dright);
	}
}
