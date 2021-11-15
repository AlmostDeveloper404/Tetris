using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public void MoveDown(Vector3 offset)
    {
        StartCoroutine(Move(offset));
    }

    IEnumerator Move(Vector3 _offset)
    {
        Vector3 startPos = transform.position;
        Vector3 desired = startPos + _offset;
        for (float i = 0; i < 1; i+=Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos,desired,i);
            yield return null;
        }
        transform.position = desired;
    }

    public void DestroyPart()
    {
        Destroy(gameObject);
    }
}
