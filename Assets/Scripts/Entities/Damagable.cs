using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour {

    // health and death properties
    public Detonator death_detonator_prefab;
    public HealthBar health_bar_prefab;
    public Vector3 health_bar_pos = new Vector3(0.0f, 0.5f, 0.0f);
    public float health_bar_size = 1.0f;
    protected HealthBar health_bar_instance;


    public float x_vanish = 20.0f;
    public float y_vanish = 20.0f;
    
    public int max_health = 1;
    private int health;
    
	// Use this for initialization
	protected virtual void Start() {
        health = max_health;

	    if(health_bar_prefab)
        {
            health_bar_instance = (HealthBar) Instantiate(
                health_bar_prefab,
                GetComponent<Transform>().position,
                GetComponent<Transform>().rotation
            );

            Vector3 hb_scale = new Vector3(health_bar_size, health_bar_size, health_bar_size);

            health_bar_instance.gameObject.transform.SetParent(gameObject.transform);
            health_bar_instance.GetComponent<Transform>().localPosition = health_bar_pos;
            health_bar_instance.GetComponent<Transform>().localScale = hb_scale;
        }
	}
	
	// Update is called once per frame
	protected virtual void Update() {
        HandleVanish();
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        float dmg = col.relativeVelocity.magnitude;
        int floor_dmg = Mathf.FloorToInt(dmg);
        if (dmg - floor_dmg > 0.5f)
            DealDamage(Mathf.CeilToInt(dmg));
        else
            DealDamage(floor_dmg);
    }

    // function that evaluates death of object, based on exceeding x and y limits
    protected virtual void HandleVanish()
    {
        if (x_vanish > 0.0f)
        {
            float x = GetComponent<Transform>().position.x;
            if (Mathf.Abs(x) > x_vanish)
                Die(true);
        }
        else if (y_vanish > 0.0f)
        {
            float y = GetComponent<Transform>().position.y;
            if (Mathf.Abs(y) > y_vanish)
                Die(true);
        }
    }

    // function called to take damage
    public virtual void DealDamage(int damage)
    {
        health -= damage;
        if (health_bar_instance)
            health_bar_instance.setHealthScale((float) health / (float) max_health);
        if (health <= 0)
            Die();
    }

    public virtual void DealExplosionForceDamage(float explosion_force, Vector3 pos, float radius, float upwards)
    {
        float dist = (pos - GetComponent<Transform>().position).magnitude;
        if (radius < dist)
            return;

        explosion_force *= 1 / (dist * 2);
        explosion_force /= 50.0f;

        DealDamage((int)explosion_force);
    }

    // function called when health <= 0
    protected virtual void Die(bool force = false)
    {
        if(death_detonator_prefab)
        {
            Detonator detonator_instance = (Detonator)Instantiate(
                death_detonator_prefab,
                GetComponent<Transform>().position,
                GetComponent<Transform>().rotation
            );
        }

        Destroy(gameObject);
    }
}
