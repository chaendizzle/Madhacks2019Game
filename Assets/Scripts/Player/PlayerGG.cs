using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGG : MonoBehaviour
{
    public int lives;

    private CircleCollider2D c2d;
    private CapsuleCollider2D cp2d;
    private Rigidbody2D r2d;
    private PlayerMovement pm;

    private enum State
    {
        ALIVE,
        RESPAWNING,
        DEAD
    }

    private State state;

    public PlayerGG GetInstance()
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
        c2d.enabled = false;
        cp2d.enabled = false;
        yield return new WaitForSeconds(2);
        r2d.isKinematic = true;
        r2d.velocity = Vector3.zero;
        transform.position = CameraMovement.CameraRect().center;
        for (int i = 0; i < 3; i++)
        {
            pm.sr.color = new Color(pm.sr.color.r, pm.sr.color.g, pm.sr.color.b, 0.5f);
            yield return new WaitForSeconds(0.3f);
            pm.sr.color = new Color(pm.sr.color.r, pm.sr.color.g, pm.sr.color.b, 1f);
            yield return new WaitForSeconds(0.3f);
        }
        r2d.isKinematic = false;
        c2d.enabled = true;
        cp2d.enabled = true;
        state = State.ALIVE;
    }
}
