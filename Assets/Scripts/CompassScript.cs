using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour
{
    public RawImage compassImg;
    public Transform player;
    
    public GameObject waypointPrefab;
    public List<Waypoint> waypoints = new List<Waypoint>();
    public float waypointVisibilityDistance = 60f;
    private float compassIncrement;

    void Start()
    {
        compassIncrement = compassImg.GetComponent<RectTransform>().rect.width / 360f;
    }

    void Update()
    {
        compassImg.uvRect = new Rect(player.localEulerAngles.y / 360f, 0, 1, 1);

        foreach (Waypoint waypoint in waypoints) {
            waypoint.image.rectTransform.anchoredPosition = GetPositionOnCompass(waypoint);
            
            float distance = Vector3.Distance(waypoint.transform.position, player.transform.position);
            
            waypoint.image.enabled = false;
            // only show waypoints if the player is close enough
            if (distance < waypointVisibilityDistance)
            {
                waypoint.image.enabled = true;
            }
        }
    }

    public void AddWaypoint(Waypoint waypoint)
    {
        GameObject newWaypoint = Instantiate(waypointPrefab, this.transform);
        waypoint.image = newWaypoint.GetComponent<RawImage>();
        waypoint.image.color = waypoint.color;
        waypoints.Add(waypoint);
    }

    Vector2 GetPositionOnCompass(Waypoint waypoint)
    {
        Vector2 playerPosition = new Vector2(player.position.x, player.position.z);
        Vector2 playerForward = new Vector2(player.forward.x, player.forward.z);

        float angle = Vector2.SignedAngle(waypoint.position - playerPosition, playerForward);

        return new Vector2(compassIncrement * angle, 0f);
    }
}