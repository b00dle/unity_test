using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour {

    public Detonator death_detonator_prefab;

    public int health = 1;
    
	// Use this for initialization
	protected virtual void Start() {
	
	}
	
	// Update is called once per frame
	protected virtual void Update() {
        
    }

    // function called to take damage
    public virtual void DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
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
