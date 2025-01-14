using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private Vector3 wallJumpDirection = new Vector3(1f, 1f, 0f);

    private Transform playerTransform;
    private CharacterController controller;

    public bool IsTouchingWall { get; private set; }

    private void Awake()
    {
        playerTransform = transform;
        controller = GetComponent<CharacterController>();
    }

    public void CheckWallState()
    {
        IsTouchingWall = Physics.Raycast(playerTransform.position, playerTransform.right, wallCheckDistance, wallLayer) ||
                         Physics.Raycast(playerTransform.position, -playerTransform.right, wallCheckDistance, wallLayer);
    }

    public void PerformWallJump(ref Vector3 velocity, float gravity)
    {
        if (IsTouchingWall && Input.GetButtonDown("Jump"))
        {
            Vector3 jumpDirection = wallJumpDirection;
            jumpDirection.x *= (Physics.Raycast(playerTransform.position, playerTransform.right, wallCheckDistance, wallLayer)) ? -1 : 1;

            velocity = jumpDirection * wallJumpForce;
            velocity.y = Mathf.Sqrt(wallJumpForce * -2f * gravity);

            Debug.Log("Wall Jump Performed!");
        }
    }

    private void OnDrawGizmos()
    {
        if (!playerTransform) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerTransform.position, playerTransform.position + playerTransform.right * wallCheckDistance);
        Gizmos.DrawLine(playerTransform.position, playerTransform.position - playerTransform.right * wallCheckDistance);
    }


}
