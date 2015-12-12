using UnityEngine;
using System.Collections;

public class WeaponProjectile : Damagable {

    // Audio settings
    public AudioClip shot_sound;
    public AudioClip hit_sound;
    public float audio_volumn = 1.0f;
    // used for playback
    protected AudioSource audio_src;
    // helps to finish playing the sound before death
    protected float sound_timer = -1.0f;

    // weapon delas damage to layer shooting
    public bool friendly_fire = false;

    // collision detonator animation
    public Detonator detonator_prefab;

    // amount of damage dealt by this projectile
    public int damage = 1;

    // flag that states if collision was detected
    protected bool has_collided_ = false;

	// Use this for initialization
	protected override void Start()
    {
        base.Start();

        audio_src = gameObject.AddComponent<AudioSource>();
        if (shot_sound)
        {
            audio_src.Stop();
            audio_src.PlayOneShot(shot_sound, audio_volumn);
        }
    }
	
	// Update is called once per frame
	protected override void Update()
    {
        base.Update();

        if (sound_timer > 0.0f)
        {
            sound_timer += Time.deltaTime;
            if (sound_timer >= hit_sound.length)
                Die();
        }
    }

    // weapon hit a target
    protected void OnTriggerEnter(Collider hit_object)
    {
        has_collided_ = true;

        Damagable damagable = hit_object.gameObject.GetComponent<Damagable>();
        if (damagable) {
            if (hit_object.gameObject.layer != LayerMask.NameToLayer("Indestructable")) {
                if (!friendly_fire && hit_object.gameObject.layer != gameObject.layer)
                    damagable.DealDamage(damage);
            }
        }            

        if (detonator_prefab)
            Detonate();
        
        if (hit_object.gameObject.GetType().IsSubclassOf(typeof(Damagable))) {
            if (hit_object.gameObject.layer != LayerMask.NameToLayer("Indestructable")) {
                if (!friendly_fire && hit_object.gameObject.layer != gameObject.layer)
                   Destroy(hit_object.gameObject);
            }
        }

        DealDamage(damage);
    }

    // remove weapon projectile
    protected override void Die(bool force = false)
    {
        if(sound_timer >= hit_sound.length || !hit_sound || force)    
            base.Die();
    }

    virtual protected void Detonate()
    {
        // hide mesh components
        Renderer renderer = GetComponent<Renderer>();
        if(renderer)
            GetComponent<Renderer>().enabled = false;

        // play hit sound
        if (hit_sound && sound_timer < 0.0f)
        {
            sound_timer = 0.0001f;
            audio_src.Stop();
            audio_src.PlayOneShot(hit_sound);
        }

        // show detonation animation
        Detonator detonator_instance = (Detonator) Instantiate(
            detonator_prefab,
            GetComponent<Transform>().position,
            GetComponent<Transform>().rotation
        );

        // to make sure shooting layer is noty affected by force of detonation
        if(!friendly_fire)
            detonator_instance.gameObject.layer = gameObject.layer;
    }
}
