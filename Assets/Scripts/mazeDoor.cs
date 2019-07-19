using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeDoor : mazePassage {

    public Transform hinge;

    private static Quaternion normalRotation = Quaternion.Euler(0f, -90f, 0f);
    private static Quaternion mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

    private bool isMirrored;

    private mazeDoor OtherSideOfDoor
    {
        get
        {
            return otherCell.GetEdge(direction.GetOpposite()) as mazeDoor;
        }
    }

    public override void Initialize(mazeCell primary, mazeCell other, mazeDirection direction)
    {
        base.Initialize(primary, other, direction);
        if(OtherSideOfDoor != null)
        {
            isMirrored = true;
            hinge.localScale = new Vector3(-1f, 1f, 1f);
            Vector3 p = hinge.localPosition;
            p.x = -p.x;
            hinge.localPosition = p;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != hinge)
            {
                child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
            }
        }
    }

    public override void OnPlayerEntered()
    {
        //other door                           //my door                       //if check           //else
        OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
        //OtherSideOfDoor.cell.room.Show();
    }

    public override void OnPlayerExited()
    {
        OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
        //OtherSideOfDoor.cell.room.Hide();
    }
}
