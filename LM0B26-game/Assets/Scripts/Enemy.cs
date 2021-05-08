using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemytype;
    public int health = 30;
    float movespeed;
    int attackinterval;
    int damage;
    bool waiting = false;
    bool isgreg = false;
    public GameObject player1;
    public GameObject player2;
    GameObject target;
    Rigidbody2D rb;
    Transform tf;
    Vector3 initpos;
    Vector3 wanderpos;
    float idletime;
    public enum enemystate
    {
        idle,
        walkontoscreen,
        walknear,
        walkup,
        attack,
        wander
    }
    public enemystate currentstate;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        tf = gameObject.GetComponent<Transform>();
        initpos = findinitpos();

        switch (enemytype)
        {
            case "Normal" :
                health = 100;
                movespeed = 1f;
                attackinterval = 4;
                damage = 10;
                break;
            default:
                health = 100;
                break;
        }

        idle(enemystate.walkontoscreen, Random.Range(2, 4));
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentstate) {
            case enemystate.walkontoscreen:
                walkontoscreen();
                break;
            case enemystate.walknear:
                walknear();
                break;
            case enemystate.walkup:
                walkup();
                break;
            case enemystate.attack:
                attack();
                break;
            case enemystate.wander:
                wander();
                break;
            default:
                break;
        }
    }

    public void idle(enemystate nextstate, float idletime)
    {
        StartCoroutine(_idle(nextstate, idletime));
    }

    IEnumerator _idle(enemystate nextstate, float idletime)
    {
        waiting = true;
        yield return new WaitForSeconds(idletime);
        waiting = false;
        currentstate = nextstate;
    }

    public Vector2 findinitpos()
    {
        Vector3 randompos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(Screen.width/2, (Screen.width - Screen.height/8)), Random.Range(0, (Screen.height - Screen.height/8)), Camera.main.farClipPlane/2));
        return randompos;

    }

    Vector2 findclosesplayer()
    {
        Vector2 diff = tf.position - player1.transform.position;
        Vector2 diff2 = tf.position - player2.transform.position;
        if (diff.x > diff2.x)
        {
            return player2.transform.position;
        } else {
            return player1.transform.position;
        }
    }

    GameObject randomplayer()
    {
        if (Random.Range(1,2) == 1) {
            isgreg = true;
            return player1;
        } else {
            return player2;
        }
    }

    public void walkontoscreen()
    {
        tf.position = Vector2.MoveTowards(tf.position, initpos, 0.015f);
        if (tf.position == initpos && waiting == false){
            idle(enemystate.walknear, 1);    
        }
        
    }

    public void wander()
    {
        tf.position = Vector2.MoveTowards(tf.position, wanderpos, 0.015f);
        if (tf.position == wanderpos) {
            currentstate = enemystate.walknear;
        }
    }
    

    public void walknear()
    {
        target = randomplayer();
        if (isgreg == true) {
            if (target.GetComponent<GregController>().enemycount < 2) {
                target.GetComponent<GregController>().enemycount++;
                if (tf.position != (target.transform.position + new Vector3(3,0,0))) {
                    tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(3,0,0)), 0.01f);
                } else if (waiting == false) {
                    idle(enemystate.walkup, attackinterval);
                }
            } else {
                wanderpos = new Vector2(Random.Range(target.transform.position.x - 5, target.transform.position.x + 5), Random.Range(target.transform.position.y - 5, target.transform.position.y + 5));
                currentstate = enemystate.wander;
            
            }
        } else {
            if (target.GetComponent<JennController>().enemycount < 2) {
                target.GetComponent<JennController>().enemycount++;
                if (tf.position != (target.transform.position + new Vector3(3,0,0))) {
                    tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(3,0,0)), 0.01f);
                } else if (waiting == false) {
                    idle(enemystate.walkup, attackinterval);
                }
            } else {
                wanderpos = new Vector2(Random.Range(target.transform.position.x - 5, target.transform.position.x + 5), Random.Range(target.transform.position.y - 5, target.transform.position.y + 5));
                currentstate = enemystate.wander;
            }   
        }
    }

    public void walkup()
    {
        tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(1,0,0)), 0.025f);
        if (waiting == false && tf.position == (target.transform.position + new Vector3(1,0,0))) {
            currentstate = enemystate.attack;
        }
    }

    public void attack()
    {
        if (waiting == false) {
            idle(enemystate.walknear,Random.Range(2, 4));
        }//punch then have a 50% chance to punch after every punch then go back to walk near
        //two attacks a punch and a kick
    }

    public void takeDamage(int damage) {
        health -= damage;
        if (health <= 0)
            die();
    }

    void die() {
        Debug.Log("I died!");
        Destroy(gameObject);
    }

}

