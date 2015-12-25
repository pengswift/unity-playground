using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;   // 伤害
    public float timeBetweenBullets = 0.15f;  // 攻击间隔
    public float range = 100f;  // 范围


    float timer;   
    Ray shootRay;   // 设置攻击ray
    RaycastHit shootHit;  // 攻击到的点
    int shootableMask;  // 检测的layer
    ParticleSystem gunParticles;  // 枪粒子
    LineRenderer gunLine;      // 线 renderer
    AudioSource gunAudio;  // 声音
    Light gunLight;  // 灯光
    float effectsDisplayTime = 0.2f;  // 效果显示时间


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");  // 获取shootable 层
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;  

        // 如果点击左键，并且 时间 > 每次间隔， 并且时间缩放不为0, 则开火
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        // 如果超过effects 时间，则取消显示效果
        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    // 取消光线， 取消灯光
    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    // 发射
    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play (); // 播放声音

        gunLight.enabled = true; // 启动光线

        gunParticles.Stop (); // 停止粒子
        gunParticles.Play (); // 开启粒子

        gunLine.enabled = true;  // 启动光线
        gunLine.SetPosition (0, transform.position); // 设置光线位置, 设置第一个点

        // 设置光线朝向
        shootRay.origin = transform.position;   // 起始位置 
        shootRay.direction = transform.forward; // 目标位置

        // 检测
        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            // 判断是否碰到带有EnemyHealth 的组件
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                // 如果有，则减血
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            // 设置光线的第二个点
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            // 如果没有碰到可射击的物体，显示最大射程
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
