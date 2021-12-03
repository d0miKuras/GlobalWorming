using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacterController))]
public class WallRun : MonoBehaviour
{

    [Tooltip("The maximum distance from the wall the wall detection works at. It shoots out rays of this length from the capsule into 4 directions: left, left+45, right, right-45.")]
    public float maxWallDistance = 1f;
    [Tooltip("The number by which the speed multiplied when jumping off a wall.")]
    public float wallSpeedMultiplier = 1.2f;
    [Tooltip("The minimum height the player has to be at after the jump for the wall run to kick in.")]
    public float minimumHeight = 1.2f;
    [Tooltip("The maximum angle (in degrees) the camera will rotate around the Z-axis when the wall run starts.")]
    public float maxAngleRoll = 20f;

    [Range(0.0f, 1.0f)]
    [Tooltip("The value the dot product of the Up vector and the normal of the wall is compared to.")]
    public float normalizedAngleThreshold = 0.1f;
    [Tooltip("The amount of time since the player jumped before the wall run can begin. E.g. if jump duration is set to 0.25, then the wall run will begin 0.25 seconds after the jump.")]
    public float jumpDuration = 1f;
    [Tooltip("The strength of the jump. The higher the value, the further to the side the player will go when jumping of a wall.")]
    public float wallBouncing = 3f;
    [Tooltip("The lerp speed of the camera roll will reach Max Angle Roll. The higher the value, the fast it transitions.")]
    public float cameraTransitionDuration = 1f;
    [Tooltip("The 'gravity' applied on the wall during the wall run. I.e. the reason the player goes downward slightly as the wall run progresses.")]
    public float wallGravityDownForce = 20f;
    [Tooltip("If checked, the player will only be able to enter a wall run while sprinting.")]
    public bool useSprint;


    PlayerCharacterController _controller;
    PlayerInputs _inputs;

    Vector3[] directions;
    RaycastHit[] hits;

    bool isWallRunning = false;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;
    float elapsedTimeSinceJump = 0f;
    float elapsedTimeSinceWallAttach = 0f;
    float elapsedTimeSinceWallDetach = 0f;
    bool jumping;

    bool isPLayerGrounded() => _controller.isGrounded;
    public bool IsWallRunning() => isWallRunning; // allows access from outside, e.g. character controller script

    // Determines whether the player can perform the wallrun. Player has to be moving, if sprinting is required - sprint, and that the player is minimumHeight from the ground.
    bool CanWallRun()
    {
        float verticalAxis = _inputs.GetMove().y;
        bool isSprinting = _inputs.GetSprint();
        isSprinting = !useSprint ? true : isSprinting;

        return !isPLayerGrounded() && verticalAxis > 0 && VerticalCheck() && isSprinting;
    }

    bool VerticalCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumHeight);
    }


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<PlayerCharacterController>();
        _inputs = GetComponent<PlayerInputs>();

        directions = new Vector3[]{
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.forward,
            Vector3.left + Vector3.forward,
            Vector3.left
        };

    }

    void LateUpdate() // TODO: might need to LateUpdate instead
    {
        isWallRunning = false;

        if (_inputs.GetJump())
        {
            jumping = true;
        }

        if (CanAttach())
        {
            hits = new RaycastHit[directions.Length];

            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 dir = transform.TransformDirection(directions[i]);
                Physics.Raycast(transform.position, dir, out hits[i], maxWallDistance);
                if (hits[i].collider != null)
                {
                    Debug.DrawRay(transform.position, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(transform.position, dir * maxWallDistance, Color.red);
                }
            }

            if (CanWallRun())
            {
                
                hits = hits.ToList().Where(hit => hit.collider != null).OrderBy(hit => hit.distance).ToArray();
                if (hits.Length > 0)
                {
                    OnWall(hits[0]); // TODO: maybe problem
                    lastWallPosition = hits[0].point;
                    lastWallNormal = hits[0].normal;
                }
                Debug.Log(hits);
            }
        }
        if (isWallRunning)
        {
            elapsedTimeSinceWallDetach = 0;
            elapsedTimeSinceWallAttach += Time.deltaTime;
            _controller.characterVelocity += Vector3.down * wallGravityDownForce * Time.deltaTime;
        }
        else
        {
            elapsedTimeSinceWallAttach = 0;
            elapsedTimeSinceWallDetach += Time.deltaTime;
        }

    }

    bool CanAttach()
    {
        if (jumping)
        {
            elapsedTimeSinceJump += Time.deltaTime;
            if (elapsedTimeSinceJump > jumpDuration)
            {
                elapsedTimeSinceJump = 0;
                jumping = false;
            }
            return false;
        }
        return true;
    }

    void OnWall(RaycastHit hit)
    {
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if (d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold)
        {
            float verticalInput = _inputs.GetMove().y; // TODO: maybe problem
            Vector3 alongWall = transform.TransformDirection(Vector3.forward);

            Debug.DrawRay(transform.position, alongWall.normalized * 10, Color.green);
            Debug.DrawRay(transform.position, lastWallNormal * 10, Color.magenta);

            _controller.characterVelocity = alongWall * verticalInput * wallSpeedMultiplier;
            isWallRunning = true;
        }
    }

    float CalculateSide()
    {
        if (isWallRunning)
        {
            Vector3 heading = lastWallPosition - transform.position;
            Vector3 perpendicular = Vector3.Cross(transform.forward, heading);
            float dir = Vector3.Dot(perpendicular, transform.up);
            return dir;
        }
        return 0f;
    }

    public float GetCameraRoll()
    {
        float dir = CalculateSide();
        float cameraAngle = _controller.playerCamera.transform.eulerAngles.z;
        float targetAngle = 0f;
        if (dir != 0f)
        {
            targetAngle = Mathf.Sign(dir) * maxAngleRoll;
        }
        return Mathf.LerpAngle(cameraAngle, targetAngle, cameraTransitionDuration);
    }

    public Vector3 GetWallJumpDirection()
    {
        if (isWallRunning)
        {
            return lastWallNormal * wallBouncing + Vector3.up;
        }
        return Vector3.zero;
    }


}
