using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{

    float speed = 0.0002f;
    // Update is called once per frame
    void Update()
    {
        float ofs = speed * Time.time;
        // transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(ofs, -1);
        transform.Rotate(new Vector3(0, ofs, 0));
    }
}