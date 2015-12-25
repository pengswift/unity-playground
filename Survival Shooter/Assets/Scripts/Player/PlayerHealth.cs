using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;  // 开始生命值
    public int currentHealth; // 当前生命值
    public Slider healthSlider; // 生命值滑块
    public Image damageImage; // 闪屏图片
    public AudioClip deathClip;  // 死亡声音
    public float flashSpeed = 5f; // 闪动速度
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f); // 颜色


    Animator anim;  // 动画
    AudioSource playerAudio;  // 声音
    PlayerMovement playerMovement; // 玩家移动对象
    PlayerShooting playerShooting;
    bool isDead;  // 是否死亡
    bool damaged; // 是否闪屏


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;   // 如果闪屏，先设置闪屏颜色
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);  // 下次则执行动画
        }
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;  // 生命--

        healthSlider.value = currentHealth; // 设置滑块

        playerAudio.Play (); // 播放声音

        if(currentHealth <= 0 && !isDead)  // 如果死亡
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");  // 切换到死亡动画

        playerAudio.clip = deathClip;  // 切换声音片段
        playerAudio.Play ();  // 播放

        playerMovement.enabled = false;   // 不在移动
        playerShooting.enabled = false;
    }


    //public void RestartLevel ()
    //{
    //    SceneManager.LoadScene (0);  // 重启
    //}
}
