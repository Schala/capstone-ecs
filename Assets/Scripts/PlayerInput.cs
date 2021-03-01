using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Various combinable input states (ie. firing, jumping)
/// </summary>
[Flags]
public enum PlayerInputFlags : byte
{
	None = 0,
	Fire = 1,
	Jump = 2
}

/// <summary>
/// Holds data from the player's input
/// </summary>
[Serializable]
public struct PlayerInput : IComponentData
{
	public float2 movement;
	public PlayerInputFlags flags;
}

/// <summary>
/// Handles player input
/// </summary>
[AlwaysUpdateSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class PlayerInputSystem : SystemBase, InputActions.IPlayerActions
{
	InputActions inputActions;
	Vector2 movement;
	EntityQuery playerInputQuery;
	bool fired = false;
	bool jumped = false;
	bool doubleJumped = false;

	/// <summary>
	/// Signal when the player has pressed the fire button.
	/// </summary>
	/// <param name="context">Context to retrieve when the player has fired</param>
	public void OnFire(InputAction.CallbackContext context) => fired = context.ReadValueAsButton();

	/// <summary>
	/// Signal when the player has pressed the jump button, or double jumped
	/// </summary>
	/// <param name="context">Context to retrieve when the player has jumped</param>
	public void OnJump(InputAction.CallbackContext context)
	{
		if (jumped && !doubleJumped)
			doubleJumped = true;
		jumped = true;
	}

	/// <summary>
	/// Get player movement values upon input
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

		playerInputQuery = GetEntityQuery(typeof(PlayerInput));
	}

	/// <summary>
	/// Enable player input on game start
	/// </summary>
	protected override void OnStartRunning() => inputActions.Enable();

	/// <summary>
	/// Disable player input on game stop
	/// </summary>
	protected override void OnStopRunning() => inputActions.Disable();

	/// <summary>
	/// Process given inputs into our input component data every frame
	/// </summary>
	protected override void OnUpdate()
	{
		if (playerInputQuery.CalculateEntityCount() == 0)
			EntityManager.CreateEntity(typeof(PlayerInput));

		var flags = PlayerInputFlags.None;
		if (fired) flags |= PlayerInputFlags.Fire;
		if (jumped) flags |= PlayerInputFlags.Jump;

		playerInputQuery.SetSingleton(new PlayerInput
		{
			movement = movement,
			flags = flags
		});
	}
}
