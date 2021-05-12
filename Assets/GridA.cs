using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridA : MonoBehaviour
{
    public bool onlyDisplayPathGizmos;
    [SerializeField] Transform player;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] Vector2 gridWorldSize;
    [SerializeField] float nodeRadius;

    public List<Node> path;
    public Node[,] grid;

    Vector2 bottomLeft;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = bottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius)
                                                    + Vector2.up * (y * nodeDiameter + nodeRadius);

                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, obstacleMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        foreach (Vector2Int dir in directions)
        {
            int checkX = node.gridX + dir.x;
            int checkY = node.gridY + dir.y;

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }

    public void SetNodeWalkable(Vector2 pos)
    {
        Node nodeToSet = GetNodeFromWorldPos(pos);
        if (nodeToSet != null)
            nodeToSet.walkable = true;
    }

    public Node GetNodeFromWorldPos(Vector2 worldPosition)
    {
        /*float percentX = worldPosition.x / gridWorldSize.x + 0.5f;
        float percentY = worldPosition.y / gridWorldSize.y + 0.5f;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        print(x + " " + y);*/
        int x = (int)(worldPosition.x - bottomLeft.x);
        int y = (int)(worldPosition.y - bottomLeft.y);
        if (x < gridSizeX && y < gridSizeY)
            return grid[x, y];
        return null;
    }

    /*void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);

        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                Gizmos.color = Color.black;
                foreach (Node n in path)
                {
                    Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
                }
            }
        }
        else
        {
            if (grid != null)
            {
                Node playerNode = GetNodeFromWorldPos(player.position);
                foreach (Node n in grid)
                {
                    Gizmos.color = n.walkable ? Color.white : Color.red;
                    if (n == playerNode)
                        Gizmos.color = Color.cyan;
                    if (path != null)
                    {
                        if (path.Contains(n))
                            Gizmos.color = Color.black;
                    }
                    Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
                }
            }
        }
    }*/
}
