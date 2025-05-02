using UnityEngine;

public class JumpingState : State
{
    bool grounded;
    bool doubleJump;

    float gravityValue;
    float jumpHeight;
    float playerSpeed;

    Vector3 airVelocity;

    public JumpingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        grounded = false;
        doubleJump = false;
        gravityValue = character.gravityValue;
        jumpHeight = character.jumpHeight;
        playerSpeed = character.playerSpeed;
        gravityVelocity.y = 0;

        character.animator.SetFloat("speed", 0);
        character.animator.SetTrigger("jump");

        Jump();
    }

    public override void HandleInput()
    {
        base.HandleInput();

        input = moveAction.ReadValue<Vector2>();

        if (jumpAction.triggered)
        {
            doubleJump = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (grounded)
        {
            stateMachine.ChangeState(character.landing);
        }

        if (doubleJump)
        {
            stateMachine.ChangeState(character.doubleJumping);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!grounded)
        {
            velocity = character.playerVelocity;                // 이전 프레임에서 저장한 플레이어 속도 복원
            airVelocity = new Vector3(input.x, 0, input.y);     // 입력값을 기준으로 공중에서의 방향벡터 계산

            // 카메라 기준으로 월드 방향 변환
            // {
            velocity = velocity.x * character.cameraTransform.right.normalized
                + velocity.z * character.cameraTransform.forward.normalized;
            velocity.y = 0f;

            airVelocity = airVelocity.x * character.cameraTransform.right.normalized
                + airVelocity.z * character.cameraTransform.forward.normalized;
            airVelocity.y = 0f;
            // }

            // 최종 이동 벡터 계산 및 이동
            character.controller.Move(gravityVelocity * Time.deltaTime
                + (airVelocity * character.airControl + velocity * (1 - character.airControl))
                * playerSpeed * Time.deltaTime);
        }

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
    }

    void Jump()
    {
        gravityVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
}