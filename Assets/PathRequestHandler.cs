using UnityEngine;

public class PathRequestHandler : MonoBehaviour 
{
    Pathfinding pathfinding;

	void Start () 
    {
        pathfinding = GetComponent<Pathfinding>();
	}

    public Vector3[] GetPath(Vector3 start, Vector3 end)
    {            
        return pathfinding.GetWaypointToFollow(start, end);
    }
}