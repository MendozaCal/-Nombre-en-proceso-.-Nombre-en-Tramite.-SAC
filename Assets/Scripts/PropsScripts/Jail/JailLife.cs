using UnityEngine;

public class JailLife : PropsLife
{
    private Transform parentObject; 

    private void Awake()
    {
        
        parentObject = transform.parent;
    }

    public override void DestroyProp()
    {
        base.DestroyProp(); 

        if (parentObject != null)
        {
            Debug.Log($"{parentObject.name} y sus hijos fueron destruidos.");
            Destroy(parentObject.gameObject); 
        }
    }
}
