using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;     // 玩家 id     
    public Rigidbody m_Shell;  // 子弹刚体          
    public Transform m_FireTransform;   // 发射器 
    public Slider m_AimSlider;          // 能量slider 
    public AudioSource m_ShootingAudio; // 射击声音 
    public AudioClip m_ChargingClip;    // 充能声音片段 
    public AudioClip m_FireClip;        // 发射声音片段 
    public float m_MinLaunchForce = 15f;  // 最小发射力
    public float m_MaxLaunchForce = 30f;  // 最大发射力
    public float m_MaxChargeTime = 0.75f; // 最大充能时间

    
    private string m_FireButton;          // 发射按钮 
    private float m_CurrentLaunchForce;   // 当前发射力 
    private float m_ChargeSpeed;          // 充能速度 
    private bool m_Fired;                 // 是否发射 


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;  // 设置初始力
        m_AimSlider.value = m_MinLaunchForce;     // 设置滑块值
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime; // 计算充能速度
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        // 设置最小充能值 
        m_AimSlider.value = m_MinLaunchForce; 

        // 如果当前发射力 >= 最大发射力，如果没有发射，则发射
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        // 如果鼠标第一次按下状态
        } else if (Input.GetButtonDown(m_FireButton)) {
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            // 设置充能状态，并切播放
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        } else if (Input.GetButton(m_FireButton) && !m_Fired) {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            // 充能
            m_AimSlider.value = m_CurrentLaunchForce;
        } else if (Input.GetButtonUp(m_FireButton) && !m_Fired) {
            Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; // 设置初始速度

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}
