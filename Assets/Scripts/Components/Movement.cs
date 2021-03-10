using System;
using Unity.Entities;

/// <summary>
/// Movement and physics data for an entity
/// </summary>
[Serializable]
public struct Movement : IComponentData
{
	public float moveSpeed;
	public float jumpForce;
	public float doubleJumpForceMultiplier;
	public float jumpTimeMax;
	public float jumpDelta;
	public bool grounded;
}
