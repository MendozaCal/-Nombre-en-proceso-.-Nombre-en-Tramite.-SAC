using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawnerReaction : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Transform minLimit; // Límite izquierdo
    public Transform maxLimit; // Límite derecho

    void Update()
    {
        if (player != null)
        {
            // Obtener la posición deseada en X dentro de los límites
            float limit = Mathf.Clamp(player.position.x, minLimit.position.x, maxLimit.position.x);

            // Aplicar la nueva posición sin cambiar Y ni Z
            transform.position = new Vector3(limit, transform.position.y, player.position.z);
        }
    }
}
