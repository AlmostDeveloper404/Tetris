using UnityEngine;

public class Tetramino : MonoBehaviour
{
    public Part[] parts;

    private void Awake()
    {
        parts = GetComponentsInChildren<Part>();
    }

    public void MoveTetramino(Vector2Int dir)
    {
        transform.position += new Vector3(dir.x,dir.y);
    }

    public void RotateCW()
    {
        transform.Rotate(0f,0f,90f);
    }
    public void RotateCCW()
    {
        transform.Rotate(0f,0f,-90f);
    }
}
