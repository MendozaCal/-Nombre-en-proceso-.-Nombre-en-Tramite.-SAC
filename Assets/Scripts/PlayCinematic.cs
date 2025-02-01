using UnityEngine;
using UnityEngine.Playables;

public class PlayCinematic : MonoBehaviour
{
    [SerializeField] private Movement movementScript;
    [SerializeField] private Grab grabScript;
    [SerializeField] private PlayerLife PlayerLife;
    [SerializeField] private Hang hangScript;
    [SerializeField] private WallClimbing wallClimbingScrpt;

    private PlayableDirector playableDirector;
    private bool onplay;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
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
    }

    public void ActivateSpecificScripts()
    {
        movementScript.enabled = true;
        grabScript.enabled = true;
        PlayerLife.enabled = true;
        hangScript.enabled = true;
        wallClimbingScrpt.enabled = true;
    }
}
