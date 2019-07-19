using UnityEngine;

public enum mazeDirection {
	North,
    East,
    South,
    West
}

public static class MazeDirections
{
    //number of directions we have
    public const int Count = 4;

    //property for a random direction
    public static mazeDirection RandomValue
    {
        get
        {
            return (mazeDirection)Random.Range(0, Count);
        }
    }

    //vector2 representation of the enums
    private static Vector2Int[] vectors = {
        new Vector2Int(0, 1), //north
        new Vector2Int(1, 0), //east
        new Vector2Int(0, -1), //south
        new Vector2Int(-1, 0) //west
    };

    //pass in the direction we want and get the vector2 representation of it
    public static Vector2Int ToIntVector2(this mazeDirection direction)
    {
        return vectors[(int)direction];
    }

    //the opposite directions
    private static mazeDirection[] opposites =
    {
        mazeDirection.South,
        mazeDirection.West,
        mazeDirection.North,
        mazeDirection.East
    };

    //pass in the direction and get the opposite
    public static mazeDirection GetOpposite(this mazeDirection direction)
    {
        return opposites[(int)direction];
    }

    //defining the rotations
    private static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    public static Quaternion ToRotation(this mazeDirection direction)
    {
        return rotations[(int)direction];
    }

    public static mazeDirection GetNextClockwise(this mazeDirection direction)
    {
        return (mazeDirection)(((int)direction + 1) % Count);
    }

    public static mazeDirection GetNextCounterclockwise(this mazeDirection direction)
    {
        return (mazeDirection)(((int)direction + Count - 1) % Count);
    }

}