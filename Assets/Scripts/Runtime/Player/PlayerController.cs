/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using Runtime.InputSystem;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private float jumpForce;
        [SerializeField] private float attackForce;
        [SerializeField] private float slideForce;
        [SerializeField] private int totalLives = 3;
        [SerializeField] private float maxAttackHeight;
        [SerializeField] private LayerMask floorLayer;
        
        // ============ COMPONENTS ============
        private Animator _movementAnimator;
        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider2D;

        // ============ VARIABLES ============
        private int CurrentLives { get; set;  }
        [HideInInspector] public bool isInAir;
        [HideInInspector] public bool isAttacking;
        private bool _gameActive;
        private bool _isSliding;
        
        // ================== STATE ==================
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking"); 
        private static readonly int IsJumping = Animator.StringToHash("isJumping");
        private static readonly int Land = Animator.StringToHash("land");
        private static readonly int IsSliding = Animator.StringToHash("isSliding");
        private static readonly int Started = Animator.StringToHash("startGame");
        private static readonly int Life = Animator.StringToHash("looseLife");
        private static readonly int GetUp = Animator.StringToHash("getUp");
        private static readonly int SlideFinish = Animator.StringToHash("slideFinish");
        
        // ================== SLIDING ==================
        private readonly Vector2 _runOffset = new(0.095f, -0.85f);
        private readonly Vector2 _runSize = new(1.65f, 4.75f);
        private readonly Vector2 _slideOffset = new(1.15f, -2.25f);
        private readonly Vector2 _slideSize = new(3.95f, 1.95f);
        private static readonly int Die = Animator.StringToHash("die");

        private void LateUpdate()
        {
            Debug.DrawRay(transform.position, Vector2.down * maxAttackHeight, Color.red);
        }

        private void OnEnable()
        {
            inputReader.SlideEvent += HandleSlide;
            inputReader.JumpEvent += HandleJump;
            inputReader.LeftClickEvent += HandleAttack;
        }

        private void OnDisable()
        {
            inputReader.SlideEvent -= HandleSlide;
            inputReader.JumpEvent -= HandleJump;
            inputReader.LeftClickEvent -= HandleAttack;
        }
       
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _movementAnimator = GetComponent<Animator>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Floor"))
            {
                if(!_gameActive) return;
                
                if(!isAttacking && isInAir)
                {
                    TriggerLand();
                }
            }
        }
        
        private void TriggerLand()
        {
            _movementAnimator.SetTrigger(Land);
            isInAir = false;
        }

        private void ResetVulnerable()
        {
            isAttacking = false;
        }
        
        private void ResetSlide()
        {
            _isSliding = false;
        }

        private void LoseLives(int lives)
        { 
            CurrentLives -= lives;
        }
        
        private void HandleAttack()
        {
            if (isInAir)
            {
                var hit = Physics2D.Raycast(transform.position, Vector2.down, maxAttackHeight, floorLayer);
                if(hit.collider != null) return;
                
                _rigidbody2D.AddForce(Vector2.down * attackForce, ForceMode2D.Impulse);
                _movementAnimator.SetTrigger(IsAttacking);
                isAttacking = true;
            }
        }

        private void HandleJump()
        {
            if(!isInAir && !_isSliding)
            {
                _movementAnimator.SetTrigger(IsJumping);
                _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isInAir = true;
            }
        }

        private void HandleSlide()
        {
            if(isInAir || _isSliding) return;
            _movementAnimator.SetTrigger(IsSliding);
            GameManager.Instance.LevelManager.DistanceTraveled += slideForce;
            _isSliding = true;
            
            _boxCollider2D.offset = _slideOffset;
            _boxCollider2D.size = _slideSize;
        }
        
        private void FinishSlide()
        {
            _movementAnimator.SetTrigger(SlideFinish);
            _boxCollider2D.offset = _runOffset;
            _boxCollider2D.size = _runSize;
        }
       
        public void LooseLife()
        {
            LoseLives(1);
            GameManager.Instance.HUD.UpdateLife(CurrentLives);
            
            if(CurrentLives <= 0)
            {
                GameManager.Instance.GameOver();
                _movementAnimator.SetTrigger(Die);
                _gameActive = false;
            }
            else
            {
                _movementAnimator.SetTrigger(Life);
            }
        }

        public void Continue()
        {
            _movementAnimator.SetTrigger(GetUp);
        }

        public void StartGame()
        {
            CurrentLives = totalLives;
            _movementAnimator.SetBool(Started, true);
            _gameActive = true;
        }
    }
}