using System;
using Unity.Entities;

/// <summary>
/// Movement and physics data for the player
/// </summary>
[Serializable]
public struct PlayerMovement : IComponentData
{
	public float moveSpeed;
	public float jumpForce;
	public float doubleJumpForceMultiplier;
	public float jumpTimeMax;
	public float jumpDelta;
	public bool grounded;
}
