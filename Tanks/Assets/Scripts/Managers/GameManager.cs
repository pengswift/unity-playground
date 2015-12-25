using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 游戏管理
public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;     // 每回合赢5次     
    public float m_StartDelay = 3f;      // 启动延时   
    public float m_EndDelay = 3f;        // 结束延时   
    public CameraControl m_CameraControl;  // 相机控制 
    public Text m_MessageText;             // 消息 
    public GameObject m_TankPrefab;        // Tank 预设 
    public TankManager[] m_Tanks;          // 坦克数组 


    private int m_RoundNumber;             // 回合数 
    private WaitForSeconds m_StartWait;    // 等待启动  
    private WaitForSeconds m_EndWait;      // 等待结束 
    private TankManager m_RoundWinner;     // 等待回合结束
    private TankManager m_GameWinner;      // 等待游戏结束 


    private void Start()
    {
        // 创建routinue
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        // 生成坦克
        SpawnAllTanks();

        // 设置相机
        SetCameraTargets();

        // 启动游戏主循环
        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
        // 遍历坦克数量 
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // 生成坦克
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            // 设置id
            m_Tanks[i].m_PlayerNumber = i + 1;
            // 启动
            m_Tanks[i].Setup();
        }
    }


    // 设置相机
    private void SetCameraTargets()
    {
        //  创建targets
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            // 存放transform
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        // 设置m_Targets
        m_CameraControl.m_Targets = targets;
    }


    // 游戏主循环
    private IEnumerator GameLoop()
    {
        // 启动 回合ing
        yield return StartCoroutine(RoundStarting());
        // 启动 游戏ing
        yield return StartCoroutine(RoundPlaying());
        // 启动 结束ing
        yield return StartCoroutine(RoundEnding());

        // 如果已经有赢家
        if (m_GameWinner != null)
        {
            // 重新加载场景
            SceneManager.LoadScene(0);
        }
        else
        {
            // 否则，重新执行游戏循环
            StartCoroutine(GameLoop());
        }
    }


    // 回合开始
    private IEnumerator RoundStarting()
    {
        // 重置所有坦克
        ResetAllTanks();

        // 设置成不可用状态
        DisableTankControl();

        // 设置照相机的初始位置 
        m_CameraControl.SetStartPositionAndSize();

        // 回合数++
        m_RoundNumber++;

        // 显示回合信息
        m_MessageText.text = "ROUND " + m_RoundNumber;

        // 切换协程
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        // 游戏开始，启动坦克 
        EnableTankControl();

        // 文本清空
        m_MessageText.text = string.Empty;

        // 如果不是一个玩家 yield return null;
        while(!OneTankLeft()) {
            // 继续执行自身
            yield return null;
        }
    }


    // 回合结束
    private IEnumerator RoundEnding()
    {
        // 坦克停止
        DisableTankControl();
        m_RoundWinner = null;

        // 获取回合赢家
        m_RoundWinner = GetRoundWinner();

        // 胜利次数++
        if (m_RoundWinner != null) 
            m_RoundWinner.m_Wins++;

        // 获取游戏玩家
        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        // 切换协程
        yield return m_EndWait;
    }


    // 判断是否小于等于1个玩家
    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        // 如果只有一个玩家
        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}
