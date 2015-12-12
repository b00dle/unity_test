using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainScript : MonoBehaviour {
    // used for playback
    private AudioSource audio_src_;
    // back ground music track
    public AudioClip sound_loop;
    public float sound_loop_volumn;
    // used for spawning enemies
    private EnemyFactory enemy_factory_;
    public Obstacle obstacle_prefab;
    private List<Enemy> alive_enemies_ = new List<Enemy>();
    public int max_enemies = 5;
    public float spawn_delay = 4.0f;

    // Use this for initialization
    void Start () {
        // create player for audio clips
        audio_src_ = gameObject.AddComponent<AudioSource>();
        
        // create spawn factory for enemies
        enemy_factory_ = gameObject.AddComponent<EnemyFactory>();
        
        // start sound loop
        if (sound_loop)
        {
            audio_src_.clip = sound_loop;
            audio_src_.loop = true;
            audio_src_.volume = sound_loop_volumn;
            audio_src_.Play();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleEnemies();    
	}

    // evaluates list of enemies and spawns new if necessary
    void HandleEnemies()
    {
        CleanAliveEnemies();
        if (alive_enemies_.Count < max_enemies && enemy_factory_.spawn_elapsed > spawn_delay)
        {
            if (!obstacle_prefab)
                return;
            bool front = Random.Range(0, 2) > 0;

            Obstacle obstacle_instance;
            if (front) {
                obstacle_instance = enemy_factory_.spawn(obstacle_prefab, SpawnLocation.FRONT);
                obstacle_instance.movement_vector.x *= Random.Range(1.0f, 4.0f);
            } else {
                obstacle_instance = enemy_factory_.spawn(obstacle_prefab, SpawnLocation.BACK);
                obstacle_instance.movement_vector.x *= -1.0f * Random.Range(1.0f, 4.0f);
            }

            float scale = Random.Range(0.5f, 2.0f);
            obstacle_instance.GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);
            alive_enemies_.Add(obstacle_instance);
        }
    }

    // removes enemies from list that are dead
    void CleanAliveEnemies()
    {
        bool clean = false;
        while (!clean) {
            clean = true;
            for(int i = 0; i < alive_enemies_.Count; ++i)
            {
                if (!alive_enemies_[i])
                {
                    alive_enemies_.RemoveAt(i);
                    clean = false;
                    break;
                }
            }
        }
    }
}
