using UnityEngine;

public class SlipperyRamp : MonoBehaviour
{
    public float slideForce = 5f;

    private void OnTriggerStay(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(other.transform.position, Vector3.down, out hit, 2f))
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal);
                slopeDirection.Normalize();

                controller.Move(slopeDirection * slideForce * Time.deltaTime);
            }
        }
    }
}
