using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shaders : MonoBehaviour
{
   // private RaycastHit raycastHit;
  //  private Outline HaveOutlineObject;
   // public LayerMask excludeLayers;
   // private GameObject Shadercontroller;

 //   void Update()
   // {
      //  if (Camera.main != null)
      //  {
      //      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           // if (Physics.Raycast(ray, out raycastHit, 10f, ~excludeLayers))
           // {
            //    GameObject Obethit = raycastHit.transform.gameObject;
            //    Outline outline = Obethit.GetComponent<Outline>();

              //  if (outline != null)
               // {
                 //   if (HaveOutlineObject != outline)
                    //{
                  //      if (HaveOutlineObject != null)
                      //  {
                          //  HaveOutlineObject.enabled = false;
                        //}
                        //outline.enabled = true;
                        //Debug.Log("Outline activado en: " + raycastHit.transform.name);
                        //HaveOutlineObject = outline;
                   // }
                //}
                //else if (HaveOutlineObject != null)
               // {
                   // HaveOutlineObject.enabled = false;
                   // HaveOutlineObject = null;
               // }

          //  }
       // }       
   // }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}
    
