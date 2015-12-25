using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;  //  tank 层
    public ParticleSystem m_ExplosionParticles; // 爆炸粒子      
    public AudioSource m_ExplosionAudio;  // 爆炸音效            
    public float m_MaxDamage = 100f;      // 最大伤害            
    public float m_ExplosionForce = 1000f;   // 破坏力         
    public float m_MaxLifeTime = 2f;      // 最大生命时间            
    public float m_ExplosionRadius = 5f;    // 爆炸范围          


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);  // 2 后销毁
    }


    // 碰撞检测 
    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        // 查找半径内的所有碰撞物
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++) {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if(!targetRigidbody)
                continue;

            // 添加爆破力
            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            if(!targetHealth)
                continue;

            // 计算爆炸伤害
            float damage = CalculateDamage(targetRigidbody.position);
            targetHealth.TakeDamage(damage);
        }

        // 粒子从父节点脱离
        m_ExplosionParticles.transform.parent = null;
        // 播放粒子
        m_ExplosionParticles.Play();
        // 播放声音
        m_ExplosionAudio.Play();

        // 存活周期之后删除
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);

        // 删除自身
        Destroy(gameObject);
    }


    // 计算爆炸伤害
    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        // // 计算目标对象 和 当前对象的距离
        Vector3 explosionToTarget = targetPosition - transform.position; 
        // 计算向量长度
        float explosionDistance = explosionToTarget.magnitude;

        // 计算相对距离
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
        // 计算相对伤害, 离爆炸中心越远，伤害越小
        float damage = relativeDistance * m_MaxDamage;
        damage = Mathf.Max(0f, damage);
        return damage;
    }
}
