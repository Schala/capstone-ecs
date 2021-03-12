
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player input
/// </summary>
[AlwaysUpdateSystem]
public class PlayerInputSystem : SystemBase, InputActions.IPlayerActions
{
	InputActions inputActions;
	Vector2 movement;
	bool fired = false;
	bool jumped = false;
	bool canDoubleJump = false;
	bool doubleJumped = false;
	bool grounded = false;

	/// <summary>
	/// Signal when the player has pressed the fire button.
	/// </summary>
	/// <param name="context">Context to retrieve when the player has fired</param>
	public void OnFire(InputAction.CallbackContext context) => fired = context.ReadValueAsButton();

	/// <summary>
	/// Signal when the player has pressed the jump button, or double jumped.
	/// </summary>
	/// <param name="context">Context to retrieve when the player has jumped</param>
	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			if (canDoubleJump)
			{
				doubleJumped = true;
				canDoubleJump = false;
			}
			if (grounded)
			{
				jumped = true;
				canDoubleJump = true;
			}
		}

		if (context.canceled) jumped = false;
	}

	/// <summary>
	/// Get player movement values upon input.
	/// </summary>
	/// <param name="context">Context to fetch the values from</param>
	public void OnMove(InputAction.CallbackContext context) => movement = context.ReadValue<Vector2>();

	/// <summary>
	/// Runs on game start, setting up input functions and fetch component data
	/// </summary>
	protected override void OnCreate()
	{
		inputActions = new InputActions();
		inputActions.Player.SetCallbacks(this);
	}

	/// <summary>
	/// Enable player input on game start.
	/// </summary>
	protected override void OnStartRunning() => inputActions.Enable();

	/// <summary>
	/// Disable player input on game stop.
	/// </summary>
	protected override void OnStopRunning() => inputActions.Disable();

	/// <summary>
	/// Process given inputs into our input component data every frame.
	/// </summary>
	protected override void OnUpdate()
	{
		var movement = this.movement;
		var fired = this.fired;
		var jumped = this.jumped;
		var doubleJumped = this.doubleJumped;
		var grounded = this.grounded;

		Entities.WithAll<PlayerTag>().ForEach((ref PlayerInput input, in PlayerMovement mvmt) =>
		{
			input.movement = -movement.x; // Negate the X-axis, otherwise our controls are inverted.

			grounded = mvmt.grounded;
			input.jump = jumped;
			input.doubleJump = doubleJumped;
		}).Run();

		this.grounded = grounded;
		this.fired = false;
		if (doubleJumped) this.doubleJumped = false;
	}
}
