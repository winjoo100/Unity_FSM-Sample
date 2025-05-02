using UnityEngine;

public class SprintJumpState : State
{
    bool grounded;
    bool doubleJump;

    float gravityValue;
    float jumpHeight;
    float playerSpeed;

    Vector3 airVelocity;

    public SprintJumpState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
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
        character.animator.SetTrigger("sprintJump");

        SprintJump();
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
            velocity = character.playerVelocity;
            airVelocity = new Vector3(input.x, 0, input.y);

            velocity = velocity.x * character.cameraTransform.right.normalized
                + velocity.z * character.cameraTransform.forward.normalized;
            velocity.y = 0f;

            airVelocity = airVelocity.x * character.cameraTransform.right.normalized
                + airVelocity.z * character.cameraTransform.forward.normalized;
            airVelocity.y = 0f;

            character.controller.Move(gravityVelocity * Time.deltaTime
                + (airVelocity * character.airControl + velocity * (1 - character.airControl))
                * playerSpeed * 1.5f * Time.deltaTime);
        }

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
    }

    void SprintJump()
    {
        gravityVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
    }
}
