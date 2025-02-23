using UnityEngine;
using UnityEngine.Playables;

public class PlayCinematic : MonoBehaviour
{
    [SerializeField] private Movement movementScript;
    [SerializeField] private Grab grabScript;
    [SerializeField] private PlayerLife PlayerLife;
    [SerializeField] private Hang hangScript;
    [SerializeField] private WallClimbing wallClimbingScrpt;
    [SerializeField] private GameObject tutorialMessage;

    private PlayableDirector playableDirector;
    private bool onplay;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void SkipToEnd()
    {
        if (playableDirector != null)
        {
            double duration = playableDirector.duration;

            playableDirector.time = duration;
            playableDirector.Evaluate();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!onplay)
            {
                onplay = true;
                DeactivateSpecificScripts();
                playableDirector.Play();
            }
        }
    }

    public void DeactivateSpecificScripts()
    {
        movementScript.enabled = false;
        grabScript.enabled = false;
        PlayerLife.enabled = false;
        hangScript.enabled = false;
        wallClimbingScrpt.enabled = false;
        if (tutorialMessage != null) tutorialMessage.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ActivateSpecificScripts()
    {
        movementScript.enabled = true;
        grabScript.enabled = true;
        PlayerLife.enabled = true;
        hangScript.enabled = true;
        wallClimbingScrpt.enabled = true;
        if (tutorialMessage != null) tutorialMessage.SetActive(true); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
