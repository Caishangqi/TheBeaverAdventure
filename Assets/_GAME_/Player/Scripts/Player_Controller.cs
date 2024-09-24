using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Add new line
// Add new line
[SelectionBase] // Select the top Hierarchy of the game object
public class Player_Controller : MonoBehaviour
{
    #region Enums

    private enum Directions
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    #endregion

    #region Editor Data

    [Header("Movement Attributes")] [SerializeField]
    float _moveSpeed = 50f;

    [Header("Dependencies")] [SerializeField]
    Rigidbody2D _rigidbody;

    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    #endregion

    #region Internal Data

    private Vector2 _deltaDisplacement = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;

    private readonly int _animMoveRight = Animator.StringToHash("Anim_Player_Move_Right");
    private readonly int _animIdleRight = Animator.StringToHash("Anim_Player_Idle_Right");

    private Vector2 _mouseClickPosition = Vector2.zero;
    private bool _isMoving = false;

    #endregion

    #region Tick

    // Update is called once per frame
    private void Update()
    {
        GetMousePosition();
        GatherMoveDirection();
        CalculateFacingDirection();
        UpdateAnimator();
    }

    private void GetMousePosition()
    {
        // Handle input for both mouse and touch
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector2 screenPosition;

            // If there is a touch, get the position of the first touch
            if (Input.touchCount > 0)
            {
                screenPosition = Input.GetTouch(0).position;
            }
            else
            {
                screenPosition = Input.mousePosition;
            }

            // Convert the screen position of the click or touch to world position
            if (Camera.main != null) _mouseClickPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            else
            {
                throw new Exception("No main camera found");
            }

            _isMoving = true;
        }
    }


    // FixedUpdate 关于一切的物理Update
    private void FixedUpdate()
    {
        MovementUpdate();
    }

    #endregion

    #region Input Logic

    private void GatherMoveDirection()
    {
        _deltaDisplacement.x = _mouseClickPosition.x - transform.position.x;
        _deltaDisplacement.y = _mouseClickPosition.y - transform.position.y;
    }

    #endregion

    #region Movement Logic

    private void MovementUpdate()
    {
        //_rigidbody.velocity = _moveDirection.normalized * _moveSpeed * Time.fixedDeltaTime;

        // Move towards the target position
        if (_isMoving)
        {
            // Smoothly move the player towards the target position
            transform.position =
                Vector2.MoveTowards(transform.position, _mouseClickPosition, _moveSpeed * Time.fixedDeltaTime);

            // If the player reaches the target position, stop moving
            if (Vector2.Distance(transform.position, _mouseClickPosition) < 0.1f)
            {
                _isMoving = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isMoving = false;
        }
    }

    #endregion

    #region Animation Logic

    private void CalculateFacingDirection()
    {
        if (_deltaDisplacement.x != 0)
        {
            if (_deltaDisplacement.x > 0) // 右侧移动
            {
                _facingDirection = Directions.RIGHT;
            }
            else if (_deltaDisplacement.x < 0)
            {
                _facingDirection = Directions.LEFT;
            }
        }

        //Debug.Log(_facingDirection);
    }

    private void UpdateAnimator()
    {
        if (_facingDirection == Directions.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_facingDirection == Directions.RIGHT)
        {
            _spriteRenderer.flipX = false;
        }

        if (_isMoving) // 意味着正在移动
        {
            _animator.CrossFade(_animMoveRight, 0);
        }

        if (!_isMoving)
        {
            _animator.CrossFade(_animIdleRight, 0);
        }

        Debug.Log(_deltaDisplacement);
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // lets the mouse position equal to rigidbody position
        _mouseClickPosition = _rigidbody.position;
    }
}