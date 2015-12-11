using UnityEngine;
using System.Collections;

public class Bomb : WeaponProjectile {
    // smoke spray trail prefab
    public DetonatorSprayHelper smoke_prefab;
    // instance
    private DetonatorSprayHelper smoke_instance_;

    protected override void Start()
    {
        base.Start();

        if (smoke_prefab)
        {
            smoke_instance_ = (DetonatorSprayHelper)Instantiate(
                smoke_prefab,
                GetComponent<Transform>().position,
                GetComponent<Transform>().rotation
            );
        }
    }

    protected override void Update()
    {
        base.Update();

        if(smoke_instance_ && !has_collided_)
            smoke_instance_.GetComponent<Transform>().position = GetComponent<Transform>().position;
    }
}
