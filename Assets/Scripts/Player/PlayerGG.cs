using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerGG : MonoBehaviour
{
    public int lives;
    public float respawnTime;

    private CircleCollider2D c2d;
    private CapsuleCollider2D cp2d;
    private Rigidbody2D r2d;
    private PlayerMovement pm;

    public enum State
    {
        ALIVE,
        RESPAWNING,
        DEAD
    }

    [HideInInspector] public State state;

    public static PlayerGG GetInstance()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerGG>();
    }

    // Start is called before the first frame update
    void Start()
    {
        c2d = GetComponent<CircleCollider2D>();
        cp2d = GetComponent<CapsuleCollider2D>();
        r2d = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMovement>();
        state = State.ALIVE;
    }

    private void Update()
    {
        ScoreBar.GetInstance().SetLives(lives);
        Rect cameraRect = CameraMovement.CameraRect();
        if ((transform.position.y < cameraRect.yMin || transform.position.x < cameraRect.xMin) && state == State.ALIVE)
        {
            Die(); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water" && state == State.ALIVE)
        {
            Die();
        }
    }

    private void Die()
    {
        if (pm.Grounded())
        {
            return;
        }

        lives--;
        ScoreBar.GetInstance().SetLives(lives);
        if (lives == 0)
        {
            //lose
            state = State.DEAD;
            c2d.enabled = false;
            cp2d.enabled = false;
            GameOver.GetInstance().Lose();  
        }
        else
        {
            //respawn
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        state = State.RESPAWNING;
        pm.inputEnabled = false;
        c2d.enabled = false;
        cp2d.enabled = false;
        yield return new WaitForSeconds(1);
        r2d.isKinematic = true;
        r2d.velocity = Vector3.zero;
        //transform.position = new Vector3(
        //    CameraMovement.CameraRect().center.x,
        //    CameraMovement.CameraRect().yMax - 1, 0);
        transform.position = FindRespawnPlatform().transform.position + new Vector3(0, 1.5f, 0);

        for (int i = 0; i < respawnTime / 0.6f ; i++)
        {
            pm.sr.color = new Color(pm.sr.color.r, pm.sr.color.g, pm.sr.color.b, 0.5f);
            yield return new WaitForSeconds(0.3f);
            pm.sr.color = new Color(pm.sr.color.r, pm.sr.color.g, pm.sr.color.b, 1f);
            yield return new WaitForSeconds(0.3f);
        }
        r2d.isKinematic = false;
        pm.inputEnabled = true;
        c2d.enabled = true;
        cp2d.enabled = true;
        state = State.ALIVE;
    }

    GameObject FindRespawnPlatform()
    {
        Director director = Director.GetInstance();
        List<GameObject> platforms = director.GetPlatforms().ToList();

        float cameraVelocity = Camera.main.GetComponent<CameraMovement>().speed.x; 
        Rect cameraRect = CameraMovement.CameraRect();

        foreach (GameObject platform in platforms)
        {
            //check that it's on screen and that it will still be on screen after respawn
            if (platform.transform.position.x - (1.5 * respawnTime * cameraVelocity) > cameraRect.xMin)
            {
                return platform;
            }
        }

        //if nothing found, just pack last element and pray
        Debug.Log("WARNING: No suitable respawn platform, fuck it....");
        return platforms[platforms.Count - 1];
    }
}
