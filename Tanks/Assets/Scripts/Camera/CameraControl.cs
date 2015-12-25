using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;   // 动画时间                
    public float m_ScreenEdgeBuffer = 4f;   // 屏幕边缘距离        
    public float m_MinSize = 6.5f;   // 最小缩放尺寸               
    [HideInInspector] public Transform[] m_Targets; // 目标对象 


    private Camera m_Camera;  // 相机                       
    private float m_ZoomSpeed; // 缩放速度                      
    private Vector3 m_MoveVelocity; // 移动速度                
    private Vector3 m_DesiredPosition;  // 预定位置             


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move(); // 移动
        Zoom(); // 缩放
    }


    private void Move()
    {
        // 寻找平均位置
        FindAveragePosition();

        // 平滑阻尼运动
        // m_MoveVelocity 当前速度，每次更改, 
        // m_DampTime 最大速度
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    // 计算平均距离
    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        // 遍历每个目标对象
        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // 距离累加
            averagePos += m_Targets[i].position;
            // 数量++
            numTargets++;
        }

        // 除以平均数
        if (numTargets > 0)
            averagePos /= numTargets;

        // 保证y坐标不动
        averagePos.y = transform.position.y;

        // 设置期望值
        m_DesiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        // 设置照相机缩放大小 
        //orthographicSize 当前位置
        //requiredSize 目标位置
        //m_ZoomSpeed 当前速度 
        //m_DampTime 最大速度 
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        // 变换到自身坐标 以相机为原点，计算  m_DesiredPosition 坐标
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        // 遍历目标 
        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            // 以相机为原点， 计算每个目标的位置 
            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            // 计算 目标对象 到 中心点的距离, 得到 (x, y) 坐标, z坐标为深度，不考虑
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // 拿当前 size 和   desiredPosToTarget.y 做比较，取最大值
            // 计算出最大的离平均点的距离
            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

            // 拿当前size 和  desiredPosToTarget.x / 宽高比 做比较，取最大值
            // 2次比较
            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
        }
        
        // 缩放尺寸＋ edge
        size += m_ScreenEdgeBuffer;

        // 和最小缩放比较
        size = Mathf.Max(size, m_MinSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}
