using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour
{
    public static string TargetTag = "";

    static Vector3 aimProjection;
    static Vector3 aimRealPosition;
    static Vector3 targetPosition;

    public float MouseSensivity = 100f;
    public LayerMask RaycastMask;

    RectTransform aimRectTransform;
    RectTransform parent;

    Camera playerCamera;

    Vector3 oldMousePosition;

	void Start ()
    {
        init();
	}

    void init()
    {
        aimRectTransform = GetComponent<RectTransform>();
        parent = aimRectTransform.parent.parent.GetComponent<RectTransform>();

        playerCamera = Camera.main;
        Cursor.visible = false;
    }
	
	void FixedUpdate ()
    {
        moveAim();
        clampAim();
        calcRealPosition();
	}

    void moveAim()
    {
        aimRectTransform.position = playerCamera.WorldToScreenPoint(aimRealPosition);
    }

    void clampAim()
    {
        float x = aimRectTransform.anchoredPosition.x;
        float y = aimRectTransform.anchoredPosition.y;
        x = Mathf.Clamp(x, -parent.sizeDelta.x / 2f, parent.sizeDelta.x / 2f);
        y = Mathf.Clamp(y, -parent.sizeDelta.y / 2f, parent.sizeDelta.y / 2f);
        aimRectTransform.anchoredPosition = new Vector2(x, y);
    }

    void calcRealPosition()
    {
        aimProjection = new Vector3(aimRectTransform.anchoredPosition.x, 0, aimRectTransform.anchoredPosition.y);
        aimProjection = aimProjection.normalized;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100f, RaycastMask))
        {
            /*
            AUTO AIM
            if (hitInfo.collider.tag == "Enemy")
            {
                aimRealPosition = hitInfo.collider.transform.position;
            }
            else
            {
                aimRealPosition = hitInfo.point;
            }
            */

            TargetTag = hitInfo.collider.tag;
            targetPosition = hitInfo.collider.transform.position;

            aimRealPosition = hitInfo.point;
        }
    }

    public static Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    public static Vector3 GetAimDirection()
    {
        return aimProjection;
    }

    public static Vector3 GetAimRealPosition()
    {
        return aimRealPosition;
    }
}
