using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow CameraInstance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<CameraFollow>();
            }

            return instance;
        }
    }

    static CameraFollow instance;

    public float FollowSpeed = 5f;

    [SerializeField]
    Transform target;

    Transform cameraTransform;
    Vector3 followOffset;

    void Start()
    {
        var test23 = GameObject.FindGameObjectsWithTag("Player");
        if (test23.Length > 0)
        {
            target = test23[0].transform;
        }
        var test1 = target?.position.x;
        var test21 = target?.position.y;
        var test31 = target?.position.z;//
        init();

        if (target != null)
        {
            SetNewTarget(target, true);
        }
        //else
        //{
        //    target = GameObject.Find("player").transform;
        //    if (target != null)
        //        SetNewTarget(target, true);
        //}
    }

    void init()
    {
        cameraTransform = transform;
        var test432 = 3;

    }
	
	void FixedUpdate ()
    {
        moveCamera();
	}
    
    void moveCamera()
    {
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, target.position - followOffset, Time.fixedDeltaTime * FollowSpeed);
    }

    public void SetNewTarget(Transform newTarget, bool calcOffset = false)
    {
        target = newTarget;

        if(calcOffset)
        {
            followOffset = newTarget.position - cameraTransform.position;
        }
    }
}
