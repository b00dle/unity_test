using UnityEngine;
using System.Collections;

public class Bullet : WeaponProjectile {
    
    // shot properties
    public Vector3 shot_direction = new Vector3(1.0f, 0.0f, 0.0f);
    public float shot_speed = 10.0f;

    // overrides BC Start()
    protected override void Start()
    {
        // BC start
        base.Start();

        // shorten destroy time
        if(detonator_prefab)
            detonator_prefab.destroyTime = 2.5f;
    }
    
    // overrides BC Update()
    protected override void Update()
    {
        // calc flying distance
        float distance = shot_speed * Time.deltaTime;
        Vector3 new_pos = GetComponent<Transform>().position + distance * shot_direction;
        
        // apply new position
        GetComponent<Transform>().position = new_pos;

        // BC update
        base.Update();
    }

    // overrides BC Detonate
    protected override void Detonate()
    {
        // play hit sound
        if (hit_sound && sound_timer < 0.0f)
        {
            sound_timer = 0.0001f;
            if(audio_src.isPlaying)
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
        if (!friendly_fire)
            detonator_instance.gameObject.layer = gameObject.layer;
    }
}
