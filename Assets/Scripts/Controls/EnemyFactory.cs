using UnityEngine;
using System.Collections;

public enum SpawnLocation
{
    FRONT,
    BACK,
    RANDOM
};

public class EnemyFactory : MonoBehaviour {

    // used for positioning spawned enemies
    public Vector3 upper_spawn_bound = new Vector3(15.0f, 13.0f, 0.0f);
    public Vector3 lower_spawn_bound = new Vector3(-15.0f, 1.0f, 0.0f);
    // seconds elapsed since last spawn
    public float spawn_elapsed = 0.0f;

    public void Update()
    {
        spawn_elapsed += Time.deltaTime;
    }

    public Enemy spawn(Enemy enemy_prefab, SpawnLocation location) {

        Vector3 position = new Vector3();

        switch (location) {
            case SpawnLocation.FRONT:
                position.x = upper_spawn_bound.x;
                break;
            case SpawnLocation.BACK:
                position.x = lower_spawn_bound.x;
                break;
            case SpawnLocation.RANDOM:
                position.x = Random.Range(lower_spawn_bound.x, upper_spawn_bound.x);
                break;
        }

        position.y = Random.Range(lower_spawn_bound.y, upper_spawn_bound.y);

        Enemy enemy_instance = (Enemy)Instantiate(
           enemy_prefab,
           position,
           new Quaternion()
        );

        spawn_elapsed = 0.0f;

        return enemy_instance;
    }

    public Obstacle spawn(Obstacle obstacle_prefab, SpawnLocation location)
    {
        Obstacle obstacle_instance = spawn((Enemy) obstacle_prefab, location) as Obstacle;
        return obstacle_instance;
    }


}
