using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectInteractStep : MonoBehaviour
{
    public GameObject objectInteract;
    private bool isStepped = false;
    private BoxCollider2D m_stepCollider; // 踏板的BoxCollider
    private BoxCollider2D m_boxCollider; // 方块的BoxCollider

    // 设置一个几乎重合的阈值
    [FormerlySerializedAs("m_positionThreshold")] [SerializeField]
    private float mPositionThreshold = 0.2f; // 位置差异阈值

    [FormerlySerializedAs("m_sizeThreshold")] [SerializeField]
    private float mSizeThreshold = 0.2f; // 尺寸差异阈值

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PuzzleCube"))
        {
            m_boxCollider = other.GetComponent<BoxCollider2D>();
            if (CheckOverlap())
            {
                isStepped = true;
                Debug.Log(gameObject.name + " is stepped");
                if (objectInteract)
                {
                    objectInteract.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PuzzleCube"))
        {
            isStepped = false;
            m_boxCollider = null;
            if (objectInteract)
                objectInteract.SetActive(true);
        }
    }


    void Start()
    {
        m_stepCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private bool CheckOverlap()
    {
        // 比较中心点和尺寸
        var distance = Vector2.Distance(m_stepCollider.gameObject.transform.position, m_boxCollider.gameObject.transform.position);
        if (distance < mPositionThreshold)
        {
            Debug.Log("两个BoxCollider2D几乎重合 " + distance);
            return true;
        }

        return false;
    }
}