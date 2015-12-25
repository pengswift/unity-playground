using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;          // 玩家个数 
    public float m_Speed = 12f;             // 速度
    public float m_TurnSpeed = 180f;        // 旋转速度
    public AudioSource m_MovementAudio;     // 移动声音源
    public AudioClip m_EngineIdling;        // 待机声音片段 
    public AudioClip m_EngineDriving;       // 移动声音片段
    public float m_PitchRange = 0.2f;       // 间距范围

    
    private string m_MovementAxisName;      // 移动的axis 名称 
    private string m_TurnAxisName;          // 循转的axis 名称
    private Rigidbody m_Rigidbody;          // 刚体
    private float m_MovementInputValue;     // 移动值
    private float m_TurnInputValue;         // 旋转值
    private float m_OriginalPitch;          // 原始间距


    private void Awake()
    {
        // 获得刚体组件
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    // 开启
    private void OnEnable ()
    {
        // 动力学关闭
        // 速度和 移动 重置为 0
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    // 关闭
    private void OnDisable ()
    {
        // 开启动力学
        m_Rigidbody.isKinematic = true;
    }


    // 开始
    private void Start()
    {
        // 设置 axis 名称
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        // 获得声音 间距
        m_OriginalPitch = m_MovementAudio.pitch;
    }
  

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName); 
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        // 如果尚未移动
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f) {
            // 如果之前是 移动状态
            if (m_MovementAudio.clip == m_EngineDriving) {
                // 改成待机状态
                m_MovementAudio.clip = m_EngineIdling;
                // 播放声音
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }

        // 如果处于移动状态    
        } else {
            // 如果之前是待机状态
            if (m_MovementAudio.clip == m_EngineIdling) {
                // 改成移动状态
                m_MovementAudio.clip = m_EngineDriving;
                // 播放声音
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        } 
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime; 

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime; 
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}
