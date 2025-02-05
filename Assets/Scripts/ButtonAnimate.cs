using UnityEngine;

public class ButtonAnimate : MonoBehaviour
{
    [SerializeField] private float minScale = 1f;       
    [SerializeField] private float maxScale = 1.4f;    
    [SerializeField] private float speed = 1f;        

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);

        float scale = Mathf.Lerp(minScale, maxScale, t);

        transform.localScale = new Vector3(scale, scale, scale);
    }
}