using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CambiarColorBoton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button miBoton;
    [SerializeField] private Sprite segundaImagenBoton;
    private Sprite imagenOriginal;

    private void Start()
    {
        if (miBoton == null)
            miBoton = GetComponent<Button>();

        imagenOriginal = miBoton.image.sprite; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        miBoton.image.sprite = segundaImagenBoton; 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        miBoton.image.sprite = imagenOriginal; 
    }
}
