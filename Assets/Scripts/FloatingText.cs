using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public float DestroyTime = 3f;
    public Vector2 Offset = new Vector2(0, 10);

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);

        transform.localPosition = Offset;
        transform.Rotate(Vector3.up, -180f);
    }

}
