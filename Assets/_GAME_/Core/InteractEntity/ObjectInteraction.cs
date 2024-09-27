using System;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject player;
    public float pickupRange = 2.0f; // 可拾取物体的最大距离
    public float doubleClickTime = 0.3f; // 双击的最大间隔时间

    private bool isAttached = false; // 物体是否已绑定
    private float lastClickTime = 0f; // 上次点击的时间
    private Transform originalParent; // 原始父对象
    public FixedJoint2D fixedJoint; // 用于连接物体和角色的关节
    void Start()
    {
        //Vector2 vector2 = gameObject.GetComponent<BoxCollider2D>().size;
        originalParent = transform.parent; // 记录物体的初始父对象
    }

    private void FixedUpdate()
    {
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            // 将鼠标屏幕坐标转换为世界坐标
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 使用射线追踪点击的物体
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject) // 检查点击是否在当前物体上
            {
                // 检测玩家是否在物体范围内
                if (Vector3.Distance(player.transform.position, transform.position) <= pickupRange)
                {
                    float timeSinceLastClick = Time.time - lastClickTime;
                    if (timeSinceLastClick <= doubleClickTime) // 检测是否为双击
                    {
                        if (isAttached)
                        {
                            DetachObject(); // 如果已绑定，解除绑定
                        }
                        else
                        {
                            AttachObject(); // 否则绑定物体
                        }
                    }

                    lastClickTime = Time.time; // 更新最后点击时间
                }
            }
        }
    }
    
    

    void AttachObject()
    {
        isAttached = true;
        
        // 取消箱子的物理模拟，使其与角色刚体合并
        Rigidbody2D boxRigidbody = GetComponent<Rigidbody2D>();
        if (boxRigidbody != null)
        {
            boxRigidbody.isKinematic = true; // 使箱子刚体不可移动
            //boxRigidbody.simulated = false;  // 取消模拟物理行为
        }
        // 打开 BoxCollider2D 的 isTrigger
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
        
        transform.SetParent(player.transform); // 将物体绑定到玩家
        transform.localPosition = new Vector2(0,0.5f); // 物体附加到玩家身上(可以根据需要调整位置)
        
        
        //boxRigidbody.isKinematic = false; // 使箱子刚体不可移动
    }

    void DetachObject()
    {
        isAttached = false;
        
        // 恢复物体的物理模拟
        Rigidbody2D boxRigidbody = GetComponent<Rigidbody2D>();
        if (boxRigidbody != null)
        {
            boxRigidbody.isKinematic = false; // 恢复箱子的物理行为
            //boxRigidbody.simulated = true;    // 重新模拟物理行为
        }
        
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = false;
        }
        
        transform.SetParent(originalParent); // 恢复物体的原始父对象
        // 你可以根据需求在此设置物体的位置，例如保持其当前的位置：
 
    }




}