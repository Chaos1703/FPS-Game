using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void walk(bool walk)
    {
        anim.SetBool("Walk", walk);
    }

    public void run(bool run)
    {
        anim.SetBool("Run", run);
    }

    public void attack()
    {
        anim.SetTrigger("Attack");
    }

    public void Dead()
    {
        anim.SetTrigger("Dead");
    }
}
