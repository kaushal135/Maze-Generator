using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeCell : MonoBehaviour {

    public Vector2Int coordinates;
    private mazeCellEdge[] edges = new mazeCellEdge[MazeDirections.Count];
    private int initializedEdgeCount;
    public mazeRoom room;

    public mazeCellEdge GetEdge(mazeDirection direction)
    {
        return edges[(int)direction];
    }

    public void SetEdge(mazeDirection direction, mazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgeCount += 1;
    }

    //returns true if the initialized edge count is the same as number of directions
    public bool IsFullyInitialized
    {
        get
        {
            return initializedEdgeCount == MazeDirections.Count;
        }
    }

    public mazeDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null)
                {
                    if (skips == 0)
                    {
                        return (mazeDirection)i;
                    }
                    skips -= 1;
                }
            }
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }

    public void Initialize(mazeRoom room)
    {
        room.Add(this);
        transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
    }

    public void OnPlayerEntered()
    {
        //room.Show();
        for (int i = 0; i < edges.Length; i++)
        {
            edges[i].OnPlayerEntered();
        }
    }

    public void OnPlayerExited()
    {
        //room.Hide();
        for (int i = 0; i < edges.Length; i++)
        {
            edges[i].OnPlayerExited();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
