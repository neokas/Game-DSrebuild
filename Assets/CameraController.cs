﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public PlayerInput pi;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 100.0f;
    public float cameraDampValue = 0.05f;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject camera;

    private Vector3 cameraDampVelocity;
   

	// Use this for initialization
	void Awake () {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        model = playerHandle.GetComponent<ActorController>().model;
        camera = Camera.main.gameObject;
	}

    // Update is called once per frame
    void FixedUpdate() {

        Vector3 tempModelEuler = model.transform.eulerAngles; //记录模型旋转角

        //水平旋转
        playerHandle.transform.Rotate(Vector3.up, pi.Camera_right * Time.fixedDeltaTime * horizontalSpeed);

        //垂直旋转
        tempEulerX -= pi.Camera_up * Time.fixedDeltaTime * verticalSpeed;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);  //旋转角限制
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

        model.transform.eulerAngles = tempModelEuler; //恢复模型旋转角

        //相机跟随 抖动太强，不做lerp和smoothDamp
        //camera.transform.position = Vector3.Lerp(camera.transform.position,transform.position,0.2f);
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        camera.transform.eulerAngles = transform.eulerAngles;

        //camera.transform.position = transform.position;
    }
}
