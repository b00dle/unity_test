using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour {
    
    void Start()
    {

    }

    // Update is called once per frame
	void Update () {
        Vector3 mouse_world = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.GetComponent<Transform>().position.z));
        GetComponent<Transform>().position = mouse_world;
    }

    // returns the current crosshair position
    public Vector3 GetPosition()
    {
        return GetComponent<Transform>().position;
    }
}
