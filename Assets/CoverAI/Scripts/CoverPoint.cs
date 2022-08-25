using UnityEngine;

public class CoverPoint
{
    private Vector3 position;
    private GameObject occupiedBy = null;

    public CoverPoint(Vector3 position)
    {
        this.position = position;
        this.occupiedBy = null;
    }

    public Vector3 getPosition()
    {
        return new Vector3(position.x, position.y, position.z);
    }

    public bool isOccupied()
    {
        return this.occupiedBy != null;
    }

    public bool isOccupiedBy(GameObject gameObject)
    {
        if (!this.isOccupied())
            return false;
        return GameObject.ReferenceEquals(gameObject, this.occupiedBy);
    }

    public bool bookPoint(GameObject gameObject)
    {
        if (this.isOccupied())
        {
            return false;
        }
        this.occupiedBy = gameObject;
        return true;
    }

    public override bool Equals(object obj)
    {
        CoverPoint other = obj as CoverPoint;
        if (obj == null)
            return false;
        return Vector3.Equals(this.position, other.position);
    }

    public override int GetHashCode()
    {
        return this.position.GetHashCode();
    }

    public void drawGizmo()
    {
        if(this.isOccupied())
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        Gizmos.DrawSphere(this.position, 0.5f);
    }
}