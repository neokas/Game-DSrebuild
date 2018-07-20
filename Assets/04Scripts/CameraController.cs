using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    private IUserInput pi;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 100.0f;
    public float cameraDampValue = 0.05f;
    public Image lockDot;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject camera;

    private Vector3 cameraDampVelocity;

    [SerializeField]
    private LockTarget lockTarget;
    public bool LockState;
   

	// Use this for initialization
	void Awake () {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        model = playerHandle.GetComponent<ActorController>().model;
        camera = Camera.main.gameObject;

        IUserInput[] inputs = playerHandle.GetComponents<IUserInput>();
        foreach(var input in inputs)
        {
            if(input.enabled == true)
            {
                pi = input;
                break;
            }
        }

        Cursor.lockState = CursorLockMode.Locked;

        lockDot.enabled = false;
    }

    private void Update()
    {
        if(lockTarget !=null)
        {
            lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));

            if(Vector3.Distance(model.transform.position,lockTarget.obj.transform.position)>10.0f)
            {
                lockTarget = null;
                lockDot.enabled = false;
                LockState = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles; //记录模型旋转角,用于恢复

            //水平旋转
            playerHandle.transform.Rotate(Vector3.up, pi.Camera_right * Time.fixedDeltaTime * horizontalSpeed);

            //垂直旋转
            tempEulerX -= pi.Camera_up * Time.fixedDeltaTime * verticalSpeed;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);  //旋转角限制
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

            model.transform.eulerAngles = tempModelEuler; //恢复模型旋转角
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }

        //相机跟随 抖动太强，将Update方法修改为FixedUpdate
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        camera.transform.LookAt(cameraHandle.transform);

    }

    public void LockUnLock()
    {
        //if(lockTarget ==null)
        //{
        //try to lock
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin1 + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, LayerMask.GetMask("Enemy"));

        if(cols.Length ==0)
        {
            lockTarget = null;
            lockDot.enabled = false;
            LockState = false;
        }
        else
        {
            foreach (var col in cols)
            {
                if(lockTarget !=null && lockTarget.obj == col.gameObject)
                {
                    lockTarget = null;
                    lockDot.enabled = false;
                    LockState = false;
                    break;
                }
                lockTarget = new LockTarget(col.gameObject,col.bounds.extents.y);
                lockDot.enabled = true;
                LockState = true;
                break;
            }
        }

    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject _obj,float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
        }
    }

}
