using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_UseRelativeRotation = true;  


    private Quaternion m_RelativeRotation;     


    private void Start()
    {
        // 获取父节点原来旋转
        m_RelativeRotation = transform.parent.localRotation;
    }


    private void Update()
    {
        // 设置循转, 保持自身旋转角度
        if (m_UseRelativeRotation)
            transform.rotation = m_RelativeRotation;
    }
}
