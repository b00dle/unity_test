using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public GameObject health;

    public void setHealthScale(float health_scale) {
        if (health_scale > 1.0f) {
            health_scale = 1.0f;
        }

        health.GetComponent<Transform>().localScale = new Vector3(health_scale, 1.0f, 1.0f);
    }
}
