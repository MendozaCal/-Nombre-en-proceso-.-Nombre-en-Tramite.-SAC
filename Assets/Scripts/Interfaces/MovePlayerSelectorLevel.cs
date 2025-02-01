using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayerSelectorLevel : MonoBehaviour
{
    public Transform[] targetPoints;
    public float moveSpeed = 5f;
    private bool isMoving = false;

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
                TryMove(Vector3.forward);
            if (Input.GetKeyDown(KeyCode.S))
                TryMove(Vector3.back);
            if (Input.GetKeyDown(KeyCode.A))
                TryMove(Vector3.left);
            if (Input.GetKeyDown(KeyCode.D))
                TryMove(Vector3.right);
        }
    }

    private void TryMove(Vector3 direction)
    {
        Vector3 currentPos = transform.position;

        var validPoints = targetPoints
            .Where(point =>
                Vector3.Dot((point.position - currentPos).normalized, direction.normalized) > 0.9f &&
                Mathf.Approximately(point.position.y, currentPos.y) &&
                Mathf.Abs((point.position - currentPos).magnitude) > 0.1f
            )
            .OrderBy(point => Vector3.Distance(point.position, currentPos))
            .ToList();

        if (validPoints.Count > 0)
        {
            StartCoroutine(MoveToPosition(validPoints[0].position));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}
