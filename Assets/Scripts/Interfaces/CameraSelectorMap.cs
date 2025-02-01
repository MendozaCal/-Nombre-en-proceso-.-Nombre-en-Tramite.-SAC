using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelectorMap : MonoBehaviour
{
    [SerializeField] GameObject Player;
    Transform Transform;
    void Start()
    {
        Transform = Player.GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Transform.position.x;
        transform.position = newPosition;
        }
}
