using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    //------------------------------
    [Header("컨트롤")]
    public float playerSpeed = 5.0f;
    public float sprintSpeed = 7.0f;
    public float jumpHeight = 0.8f;
    public float gravityMultiplier = 2f;
    public float rotationSpeed = 5f;

    [Header("애니메이션 스무딩")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;
    [Range(0, 1)]
    public float velocityDampTime = 0.1f;
    [Range(0, 1)]
    public float rotationDampTime = 0.2f;
    [Range(0, 1)]
    public float airControl = 0.5f;

    //------------------------------
    public StateMachine movementSM;
    public StandingState standing;
    public SprintState sprinting;
    public JumpingState jumping;
    public SprintJumpState sprintJumping;
    public DoubleJumpState doubleJumping;
    public LandingState landing;
    public CombatState combatting;
    public AttackState attacking;

    //------------------------------
    [HideInInspector]
    public float gravityValue = -9.81f;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform cameraTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 playerVelocity;

    //------------------------------
    public event Action<Mode> OnModeChange; // 모드 변경 이벤트

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();

        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM);
        sprinting = new SprintState(this, movementSM);
        jumping = new JumpingState(this, movementSM);
        sprintJumping = new SprintJumpState(this, movementSM);
        doubleJumping = new DoubleJumpState(this, movementSM);
        landing = new LandingState(this, movementSM);
        combatting = new CombatState(this, movementSM);
        attacking = new AttackState(this, movementSM);

        movementSM.Initialize(standing);

        gravityValue *= gravityMultiplier;
    }

    private void Update()
    {
        movementSM.currentState.HandleInput();

        movementSM.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }

    /// <summary>
    /// 모드 변경 이벤트 호출
    /// </summary>
    /// <param name="newMode"></param>
    public void InvokeModeChange(Mode newMode)
    {
        OnModeChange?.Invoke(newMode);
    }


    // --------------------------------------------------
    // 애니메이션 이벤트용 사운드 다이얼로그 함수
    // --------------------------------------------------

    /// <summary>
    /// 전투 모드 사운드 다이얼로그
    /// </summary>
    public void PlayDialogue_CombatMode()
    {
        SoundManager.Instance.PlaySFX("Dialogue_CombatMode");
    }

    /// <summary>
    /// 공격01 사운드 다이얼로그
    /// </summary>
    private void PlayDialogue_Attack01()
    {
        SoundManager.Instance.PlaySFX("Dialogue_Attack01");
    }

    /// <summary>
    /// 공격02 사운드 다이얼로그
    /// </summary>
    private void PlayDialogue_Attack02()
    {
        SoundManager.Instance.PlaySFX("Dialogue_Attack02");
    }

    /// <summary>
    /// 공격03 사운드 다이얼로그
    /// </summary>
    private void PlayDialogue_Attack03()
    {
        SoundManager.Instance.PlaySFX("Dialogue_Attack03");
    }
}
