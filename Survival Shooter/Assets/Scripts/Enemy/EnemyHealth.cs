using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;  // 下沉速度
    public int scoreValue = 10;  // 分数值
    public AudioClip deathClip; // 死亡声音片段


    Animator anim;              // 动画
    AudioSource enemyAudio;     // 声音源 
    ParticleSystem hitParticles;  // 粒子系统
    CapsuleCollider capsuleCollider;  // 胶囊碰撞器
    bool isDead;  // 是否死亡
    bool isSinking;  // 是否下沉


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();  // 获取子系统的粒子系统
        capsuleCollider = GetComponent <CapsuleCollider> (); // 胶囊系统

        currentHealth = startingHealth;   // 初始血量
    }


    void Update ()
    {
        if(isSinking)   // 如果是下沉状态
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);  // 朝着反方向移动
        }
    }


    // 收到伤害
    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();   // 播放受伤音乐

        currentHealth -= amount;  // 剪掉血量
            
        hitParticles.transform.position = hitPoint; // 设置受伤粒子位置
        hitParticles.Play();  // 播放

        if(currentHealth <= 0)   // 如果生命 <= 0 则死亡
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;             // 设置死亡

        capsuleCollider.isTrigger = true;  // 触碰器设置成true

        anim.SetTrigger ("Dead"); // 设置死亡状态

        enemyAudio.clip = deathClip; // 设置死亡片段
        enemyAudio.Play ();  // 播放音乐
    }


    // 开始下沉
    public void StartSinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false;   // 关闭navMeshAgent 
        GetComponent <Rigidbody> ().isKinematic = true;   // 启动Kinematic
        isSinking = true;   // 设置下沉状态
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);  // 2s后销毁该对象
    }
}
