using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    // movement
    public Vector3 input;
    public float move_speed = 15.0f;
    public float x_teleport = 10.0f;
    public float y_drain = 5.0f;
    
    // shooting
    public float bomb_delay = 1.5f;
    private float bomb_cooldown_timer_ = 0.0f;
    public float projectile_delay = 0.25f;
    private float projectile_cooldown_timer_ = 0.0f;

    // weapons
    public Bomb bomb_prefab_;
    public Bullet bullet_prefab_;
    // crosshair
    public CrossHair cross_hair_prefab_;
    private CrossHair cross_hair_instance_;

    // Use this for initialization
    void Start()
    {
        cross_hair_instance_ = Instantiate(
            cross_hair_prefab_,
            GetComponent<Transform>().position,
            GetComponent<Transform>().rotation
        ) as CrossHair;
    }

    // Update is called once per frame
    void Update () {
        HandleGravity();
        HandleMovement();
        HandleFire();
    }
    
    // Controls custom gravity
    void HandleGravity() {
        GetComponent<Rigidbody>().AddForce(
            0.25f * Physics.gravity * GetComponent<Rigidbody>().mass   
        );
    }

    // Controls movement input
    void HandleMovement() {
        float x_pos = GetComponent<Transform>().position.x;
        if (Mathf.Abs(x_pos) > x_teleport)
            HandleTeleport(x_pos);

        HandleForce();
    }

    // Helper function for HandleMovement
    void HandleTeleport(float x_pos)
    {
        if (x_pos > x_teleport)
            x_pos = -x_teleport;
        else
            x_pos = x_teleport;

        Vector3 pos = new Vector3();
        pos.Set(
            x_pos,
            GetComponent<Transform>().position.y,
            GetComponent<Transform>().position.z
        );
        GetComponent<Transform>().position = pos;
    }

    // Helper function for HandleMovement
    void HandleForce()
    {
        input = new Vector3(0.0f, 0.0f, 0.0f);

        float x = move_speed * Input.GetAxis("Right");
        if (Mathf.Abs(GetComponent<Rigidbody>().velocity.x) < 10.0f)
            input.x = x;
        else if (GetComponent<Rigidbody>().velocity.x >= 10.0f && x <= 0.0f)
            input.x = x;
        else if (GetComponent<Rigidbody>().velocity.x <= -10.0f && x >= 0.0f)
            input.x = x;

        float y = move_speed * Input.GetAxis("Up");
        if (Mathf.Abs(GetComponent<Rigidbody>().velocity.y) < 2.5f)
            input.y = y;
        else if (GetComponent<Rigidbody>().velocity.y >= 2.5f && y <= 0.0f)
            input.y = y;
        else if (GetComponent<Rigidbody>().velocity.y <= -2.5f && y >= 0.0f)
            input.y = y;

        if (input.y > 0.0f && GetComponent<Transform>().position.y > y_drain)
            input.y = 0.0f;

        GetComponent<Rigidbody>().AddForce(input);
    }

    // Controls firing input
    void HandleFire()
    {
        HandleProjectileFire();
        HandleBombFire();
    }

    // Helper function for HandleFire
    void HandleBombFire()
    {
        bomb_cooldown_timer_ -= Time.deltaTime;

        if (Input.GetButton("BombFire") && bomb_cooldown_timer_ <= 0.0)
        {
            bomb_cooldown_timer_ = bomb_delay;

            Vector3 offset_pos = new Vector3(0.0f, -1.0f, 0.0f);
            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);

            Bomb bomb = Instantiate(
                bomb_prefab_,
                GetComponent<Transform>().position + offset_pos,
                rotation
            ) as Bomb;

            bomb.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * 0.5f;
        }
    }

    void HandleProjectileFire()
    {
        projectile_cooldown_timer_ -= Time.deltaTime;

        if (Input.GetButton("ProjectileFire") && projectile_cooldown_timer_ <= 0.0)
        {
            projectile_cooldown_timer_ = projectile_delay;

            Vector3 target_world = cross_hair_instance_.GetPosition();
            Vector3 pos_world = GetComponent<Transform>().position;
            Vector3 dir = target_world - pos_world;
            Vector3 fwd = new Vector3(1.0f, 0.0f, 0.0f);

            dir.Normalize();

            Quaternion rotation = Quaternion.Euler(0.0f, 90.0f, 90.0f);
            
            if((dir- fwd).magnitude > 0.001) {
                Vector3 axis = Vector3.Cross(fwd, dir);
                float angle = Mathf.Acos(Vector3.Dot(dir, fwd)) * Mathf.Rad2Deg;
                rotation = Quaternion.AngleAxis(angle, axis) * rotation;
            }
            
            Bullet bullet = Instantiate(
                bullet_prefab_,
                GetComponent<Transform>().position,
                rotation
            ) as Bullet;

            bullet.shot_direction = dir;
        }
    }
}
