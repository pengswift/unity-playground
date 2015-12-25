using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;    // 初始血量      
    public Slider m_Slider;   // 滑动块                     
    public Image m_FillImage;   // 填充图片                   
    public Color m_FullHealthColor = Color.green;  // 满血显示绿色
    public Color m_ZeroHealthColor = Color.red;    // 空血显示红色 
    public GameObject m_ExplosionPrefab; // 爆炸粒子
    
    
    private AudioSource m_ExplosionAudio;          // 爆炸音效
    private ParticleSystem m_ExplosionParticles;   // 粒子系统 
    private float m_CurrentHealth;  // 当前血量 
    private bool m_Dead;          // 是否死亡  


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>(); // 初始化例子预设，并获得粒子系统
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>(); // 获得爆炸音效 

        m_ExplosionParticles.gameObject.SetActive(false);  // 设置关闭
    }


    // 启用时
    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;  // 设置血量
        m_Dead = false;  //  

        // 设置血量UI
        SetHealthUI();
    }
   

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount; 

        SetHealthUI();

        if (m_CurrentHealth <= 0f && !m_Dead) {
            OnDeath();
        }
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentHealth; 
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true; 

        m_ExplosionParticles.transform.position= transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();

        m_ExplosionAudio.Play();

        gameObject.SetActive(false);

    }
}
