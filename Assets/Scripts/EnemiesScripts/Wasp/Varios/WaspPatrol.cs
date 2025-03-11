using UnityEngine;

public class AvistaPatrol : MovePatrol
{
    WaspDetector waspDetector;
    [SerializeField] GameObject wasp;
    [SerializeField] GameObject Detector;
    [SerializeField] Transform[] waypointsAttackMod;
    public float maxSpeedAttackMode = 25;
    int currentWaypointAttackMode = 0;
    void Start()
    {
        waspDetector = Detector.gameObject.GetComponent<WaspDetector>();
    }
    protected override void Update()
    {
        if (waspDetector.PlayerStay && waypointsAttackMod.Length > 0)
        {
            MoveToWaypointAttackMode();
            calculateDistanceWayPointsAttackMode();
        }
        else
        {
            base.Update();
            if (waypoints.Length > 0)
            {
                wasp.transform.LookAt(waypoints[currentWaypoint]);
            }
        }
    }

    void MoveToWaypointAttackMode()
    {
        if (currentWaypointAttackMode >= waypointsAttackMod.Length) return;

        Vector3 direction = waypointsAttackMod[currentWaypointAttackMode].position - transform.position;
        direction.Normalize();
        transform.Translate(direction * maxSpeedAttackMode * Time.deltaTime, Space.World);
    }

    void calculateDistanceWayPointsAttackMode()
    {
        if (currentWaypointAttackMode >= waypointsAttackMod.Length) return;

        if (Vector3.Distance(transform.position, waypointsAttackMod[currentWaypointAttackMode].position) < 1)
        {
            currentWaypointAttackMode++;

            if (currentWaypointAttackMode >= waypointsAttackMod.Length)
            {
                currentWaypointAttackMode = 0;
            }
        }
    }
}
