using UnityEngine;
using System.Collections;

public class Obstacle : Enemy {

    // movement direction of this instance
    public Vector3 movement_vector = new Vector3(-1.0f, 0.0f, 0.0f);

	// Update is called once per frame
    protected override void Update () {
        base.Update();

        // move along movement vector
        Vector3 pos = GetComponent<Transform>().position;
        pos = pos + Time.deltaTime * movement_vector;
        GetComponent<Transform>().position = pos;
	}
}
