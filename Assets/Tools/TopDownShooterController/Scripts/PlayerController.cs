using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static Transform PlayerTransform;

    public float MovementSpeed = 5f;
    public bool RotationSmooth = true;
    public Transform WeaponPivot;
    public Transform PlayerModel;

    public string NoAimOffsetTag = "NoAimOffset";
    public string LevelTag = "Level";
    public string EnemyTag = "Enemy";

    Rigidbody playerBody;
    Transform playerTransform;

	void Start ()
    {
        init();
	}

    void init()
    {
        playerBody = GetComponent<Rigidbody>();
        playerTransform = transform;

        PlayerTransform = transform;
    }
	
    void dieEventHandler()
    {
        enabled = false;
    }

	void FixedUpdate ()
    {
        movePlayer();
        rotatePlayer();
        rotateWeaponPivot();
	}

    void movePlayer()
    {
        Vector3 movementAmount = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movementAmount = Vector3.ClampMagnitude(movementAmount, 1f);

        float speedMultiplier = 1f;

        float angle = Vector3.Angle(movementAmount, AimController.GetAimDirection());

        speedMultiplier = (1f - angle / 360f);

        playerBody.MovePosition(playerBody.position + movementAmount * MovementSpeed * speedMultiplier * Time.fixedDeltaTime);
    }

    void rotatePlayer()
    {
        Quaternion rotation = Quaternion.FromToRotation(playerTransform.forward, (AimController.GetAimRealPosition() - playerTransform.position).normalized) * playerBody.rotation;
        if (RotationSmooth)
        {
            playerBody.MoveRotation(Quaternion.Slerp(playerBody.rotation, Quaternion.Euler(rotation.eulerAngles.y * Vector3.up), Time.fixedDeltaTime * 20f));
        } else
        {
            playerBody.MoveRotation(Quaternion.Euler(rotation.eulerAngles.y * Vector3.up));
        }
    }

    void rotateWeaponPivot()
    {
        float aimY = AimController.GetAimRealPosition().y;
        float playerY = playerTransform.position.y;
        float targetY = AimController.GetTargetPosition().y;

        float yDist = Mathf.Abs(playerY - targetY);
        Vector3 targetAimPoint = AimController.GetAimRealPosition();

        if(AimController.TargetTag == NoAimOffsetTag)
        {
            targetAimPoint.y = playerY + 1f;
        }

        if (AimController.TargetTag == LevelTag)
        {
            yDist = Mathf.Abs(playerY - aimY);

            if (yDist > 0.3f)
            {
                targetAimPoint.y = aimY + 1f;
            }
            else
            {
                targetAimPoint.y = playerY + 1f;
            }
        }

        if (AimController.TargetTag == EnemyTag)
        {
            if (yDist > 0.3f)
            {
                targetAimPoint.y = aimY;
            }
            else
            {
                targetAimPoint.y = targetY + 1f;
            }
        }

        Quaternion rotation = Quaternion.FromToRotation(WeaponPivot.forward, (targetAimPoint - WeaponPivot.position).normalized) * WeaponPivot.rotation;
        WeaponPivot.rotation = Quaternion.Euler((Vector2)rotation.eulerAngles);
    }
}
