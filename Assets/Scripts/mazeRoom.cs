using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeRoom : ScriptableObject {

    public int settingsIndex;
    public mazeRoomSettings settings;

    private List<mazeCell> cells = new List<mazeCell>();
    public void Add(mazeCell cell)
    {
        cell.room = this;
        cells.Add(cell);
    }

    public void Assimilate(mazeRoom room)
    {
        for(int i = 0; i < room.cells.Count; i++)
        {
            Add(room.cells[i]);
        }
    }

    public void Hide()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].Hide();
        }
    }

    public void Show()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].Show();
        }
    }

}
