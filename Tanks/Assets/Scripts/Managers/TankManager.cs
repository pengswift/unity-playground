using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    // 坦克管理
    public Color m_PlayerColor;  //  玩家颜色          
    public Transform m_SpawnPoint;   //  出生点      
    [HideInInspector] public int m_PlayerNumber;    // 玩家id         
    [HideInInspector] public string m_ColoredPlayerText; // 玩家字体颜色
    [HideInInspector] public GameObject m_Instance;  // 实例        
    [HideInInspector] public int m_Wins;     // win次数                


    private TankMovement m_Movement;      // 坦克移动  
    private TankShooting m_Shooting;      // 坦克射击
    private GameObject m_CanvasGameObject;  // 


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject; // 获取canvas 组件

        m_Movement.m_PlayerNumber = m_PlayerNumber;   // 设置玩家id
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        // 玩家颜色字体
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        // 获取mesh renders 器
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        // 设置渲染颜色
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    // 放弃控制
    public void DisableControl()
    {
        // 设置不可用
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        // 背景canvas 设置不激活状态
        m_CanvasGameObject.SetActive(false);
    }


    // 启用控制
    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        // 设置canvas激活
        m_CanvasGameObject.SetActive(true);
    }


    // 重置
    public void Reset()
    {
        // 设置实例的位置和旋转角度
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        // 关闭 & 显示
        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
