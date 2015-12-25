using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;  // 攻击延时
    public int attackDamage = 10;  // 攻击减血量


    Animator anim;       // 动画
    GameObject player;   // 玩家对象
    PlayerHealth playerHealth;   // 玩家健康
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)  // trigger 触碰 
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)  // 如果在供给范围内，并且>= timeBetween
        {
            Attack ();  // 供给
        }

        if(playerHealth.currentHealth <= 0)  // 生命小与0 ，设置 死亡
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        // 攻击时，重置timer
        timer = 0f;

        // 设置当前生命 > 0,  减少血量 
        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
