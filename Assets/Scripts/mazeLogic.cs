using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeLogic : MonoBehaviour {

    public Vector2Int size;
    public mazeCell cellPrefab;
    private mazeCell[,] cells;
    //public float generationStepDelay;

    public mazePassage passagePrefab;
    public mazeWall[] wallPrefabs;
    public mazeDoor doorPrefab;
    public mazeRoomSettings[] roomSettings;
    private List<mazeRoom> rooms = new List<mazeRoom>();

    [Range(0f, 1f)]
    public float doorProbability;





	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //creating the maze
    public void Generate()
    {
        cells = new mazeCell[size.x, size.y];

        List<mazeCell> activeCells = new List<mazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            DoNextGenerationStep(activeCells);
        }

        //for(int i = 0; i < rooms.Count; i++)
        //{
        //    rooms[i].Hide();
        //}
    }

    //for generating the maze
    /*public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new mazeCell[size.x, size.y];

        List<mazeCell> activeCells = new List<mazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
    }*/

    //creating individual cell
    private mazeCell CreateCell(Vector2Int cord)
    {
        mazeCell newCell = Instantiate(cellPrefab) as mazeCell;
        cells[cord.x, cord.y] = newCell;
        newCell.coordinates = cord;
        newCell.name = "Maze Cell " + cord.x + ", " + cord.y;
        newCell.transform.parent = transform;

        //calculates the position by generally increasing by 1unit
        newCell.transform.localPosition = new Vector3(cord.x - size.x * 0.5f + 0.5f, 0f, cord.y - size.y * 0.5f + 0.5f);

        return newCell;
    }

    //property for random cordinates
    public Vector2Int RandomCoordinates
    {
        get
        {
            return new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        }
    }

    //returns a bool if the cordinate is within user defined boundry
    public bool ContainsCoordinates(Vector2Int cordinate)
    {
        return cordinate.x >= 0 && cordinate.x < size.x && cordinate.y >= 0 && cordinate.y < size.y;
    }

    //returns a cell from our array
    public mazeCell GetCell(Vector2Int coordinates)
    {
        return cells[coordinates.x, coordinates.y];
    }

    //gets called first time
    private void DoFirstGenerationStep(List<mazeCell> activeCells)
    {
        mazeCell newCell = CreateCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
        activeCells.Add(newCell);
    }

    //checks if the cell already exists, if not adds it to the list
    private void DoNextGenerationStep(List<mazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        mazeCell currentCell = activeCells[currentIndex];

        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }


        mazeDirection direction = currentCell.RandomUninitializedDirection;
        Vector2Int coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            mazeCell neighbor = GetCell(coordinates);

            if(neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else if(currentCell.room.settingsIndex == neighbor.room.settingsIndex)
            {
                CreatePassageInSameRoom(currentCell, neighbor, direction);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
        }
    }

    private void CreatePassage(mazeCell cell, mazeCell otherCell, mazeDirection direction)
    {
        mazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
        mazePassage passage = Instantiate(prefab) as mazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(prefab) as mazePassage;
        if(passage is mazeDoor)
        {
            otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
        }
        else
        {
            otherCell.Initialize(cell.room);
        }

        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(mazeCell cell, mazeCell otherCell, mazeDirection direction)
    {
        mazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as mazeWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as mazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    private mazeRoom CreateRoom(int indexToExclude)
    {
        mazeRoom newRoom = ScriptableObject.CreateInstance<mazeRoom>();
        newRoom.settingsIndex = Random.Range(0, roomSettings.Length);

        if(newRoom.settingsIndex == indexToExclude)
        {
            newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
        }
        newRoom.settings = roomSettings[newRoom.settingsIndex];
        rooms.Add(newRoom);
        return newRoom;
    }

    private void CreatePassageInSameRoom(mazeCell cell, mazeCell otherCell, mazeDirection direction)
    {
        mazePassage passage = Instantiate(passagePrefab) as mazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as mazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());

        if(cell.room != otherCell.room)
        {
            mazeRoom roomToAssimilate = otherCell.room;
            cell.room.Assimilate(roomToAssimilate);
            rooms.Remove(roomToAssimilate);
            Destroy(roomToAssimilate);
        }

    }

}
