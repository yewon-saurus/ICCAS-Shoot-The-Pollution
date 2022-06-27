using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missbackCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -5, 13);
    }

    // Update is called once per frame
    void Update()
    {
        if (GunCtrl.miss == 1) {
            transform.position = new Vector3(0, -4.2f, 13);
        }
        else if (GunCtrl.miss == 2) {
            transform.position = new Vector3(0, -3.1f, 13);
        }
        else if (GunCtrl.miss == 3) {
            transform.position = new Vector3(0, -2, 13);
        }
        else if (GunCtrl.miss == 4) {
            transform.position = new Vector3(0, -0.9f, 13);
        }
        else if (GunCtrl.miss == 5) {
            transform.position = new Vector3(0, 0.5f, 13);
        }
        else if (GunCtrl.miss == 6) {
            transform.position = new Vector3(0, 1.6f, 13);
        }
        else if (GunCtrl.miss == 7) {
            transform.position = new Vector3(0, 2.7f, 13);
        }
        else if (GunCtrl.miss == 8) {
            transform.position = new Vector3(0, 3.8f, 13);
        }
        else if (GunCtrl.miss == 9) {
            transform.position = new Vector3(0, 4.9f, 13);
        }
        else if (GunCtrl.miss >= 10) {
            transform.position = new Vector3(0, 6, 13);
        }
        else {
            transform.position = new Vector3(0, -5, 13);
        }
    }
}
