using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemytype;
    int health;
    float movespeed;
    int attackinterval;
    int damage;
    bool waiting = false;
    public Animator animator;
    public bool isgreg = false;
    public GameObject player1;
    public GameObject player2;
    GameObject target;
    Rigidbody2D rb;
    Transform tf;
    Vector3 initpos;
    Vector3 wanderpos;
    Vector2 currentpos;
    Vector2 lastpos;
    private bool facingRight = true;
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
        target = randomplayer();
        currentpos = tf.position;

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
        flip();
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
        if (Random.Range(1,3) == 1) {
            isgreg = true;
            return player1;
        } else {
            Debug.Log("jen");
            return player2;
        }
    }

    public void walkontoscreen()
    {
        tf.position = Vector2.MoveTowards(tf.position, initpos, 0.015f);
        if (tf.position == initpos && waiting == false){
            if (isgreg == true) {
                if (target.GetComponent<GregController>().enemycount < 2) {
                    target.GetComponent<GregController>().enemycount++;
                    idle(enemystate.walknear, 1);
                } else {
                    wanderpos = new Vector2(Random.Range(target.transform.position.x - 4, target.transform.position.x + 4), Random.Range(target.transform.position.y - 2, target.transform.position.y + 2));
                    idle(enemystate.wander, 1);
                }
            } else {
                if (target.GetComponent<JennController>().enemycount < 2) {
                    target.GetComponent<JennController>().enemycount++;
                    idle(enemystate.walknear, 1);
                } else {
                    wanderpos = new Vector2(Random.Range(target.transform.position.x - 4, target.transform.position.x + 4), Random.Range(target.transform.position.y - 2, target.transform.position.y + 2));
                    idle(enemystate.wander, 1);
                }
            }
        }
    }

    public void wander()
    {
        tf.position = Vector2.MoveTowards(tf.position, wanderpos, 0.008f);
        if (tf.position == wanderpos) {
            if (isgreg == true) {
                if (target.GetComponent<GregController>().enemycount < 2) {
                    target.GetComponent<GregController>().enemycount++;
                    idle(enemystate.walknear, 1);
                } else {
                    wanderpos = new Vector2(Random.Range(target.transform.position.x - 4, target.transform.position.x + 4), Random.Range(target.transform.position.y - 2, target.transform.position.y + 2));
                }
            } else {
                if (target.GetComponent<JennController>().enemycount < 2) {
                    target.GetComponent<JennController>().enemycount++;
                    idle(enemystate.walknear, 1);
                } else {
                    wanderpos = new Vector2(Random.Range(target.transform.position.x - 4, target.transform.position.x + 4), Random.Range(target.transform.position.y - 2, target.transform.position.y + 2));
                }
            }
        }
    }
    
    public void flip()
    {
        lastpos = currentpos;
        currentpos = tf.position;
        if (currentpos.x < lastpos.x && facingRight == true) {
            // Switch the way the player is labelled as facing.
		    facingRight = false;

		    // Multiply the player's x local scale by -1.
		    Vector3 theScale = transform.localScale;
		    theScale.x *= -1;
		    transform.localScale = theScale;
        } else if (currentpos.x > lastpos.x && facingRight == false) {
            // Switch the way the player is labelled as facing.
		    facingRight = true;

		    // Multiply the player's x local scale by -1.
		    Vector3 theScale = transform.localScale;
		    theScale.x *= -1;
		    transform.localScale = theScale;
        }
    }

    public void walknear()
    {
        if (tf.position != (target.transform.position + new Vector3(3,0,0))) {
            if (isgreg == true) {
                if (target.GetComponent<GregController>().enemycount == 1) {
                    tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(3,0,0)), 0.01f);
                } else if (target.GetComponent<GregController>().enemycount == 2) {
                    tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(-3,0,0)), 0.01f);
                }
            } else {
                if (target.GetComponent<JennController>().enemycount == 1) {
                    tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(3,0,0)), 0.01f);
                } else if (target.GetComponent<GregController>().enemycount == 2) {
                    tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(-3,0,0)), 0.01f);
                }
            }
        } else if (waiting == false) {
            idle(enemystate.walkup, attackinterval);
        }
    }

    public void walktowards() 
    {
        if (tf.position != (target.transform.position + new Vector3(3,0,0))) {
            tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(3,0,0)), 0.01f);
        } else if (waiting == false) {
            idle(enemystate.walkup, attackinterval);
        }
    }

    public void walkup()
    {
        tf.position = Vector2.MoveTowards(tf.position, (target.transform.position + new Vector3(1,0,0)), 0.025f);
        if (waiting == false && tf.position == (target.transform.position + new Vector3(1,0,0))) {
            animator.SetTrigger("Attack");
            currentstate = enemystate.attack;
        }
    }

    public void attack()
    {
        if (waiting == false) {
            idle(enemystate.walknear, 1);
        }//punch then have a 50% chance to punch after every punch then go back to walk near
        //two attacks a punch and a kick
    }

}

