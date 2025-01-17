using UnityEngine;

public class SlipperyRamp : MonoBehaviour
{
    public float slideForce = 5f; 

    private void OnTriggerStay(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            if (Physics.Raycast(other.transform.position, Vector3.down, out RaycastHit hit, 1.5f))
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;

                controller.Move(slopeDirection * slideForce * Time.deltaTime);
            }
        }
    }
}
