using System;
using Unity.Entities;

/// <summary>
/// Movement and physics data for an entity
/// </summary>
[Serializable]
public struct Movement : IComponentData
{
	public const byte Grounded = 1;

	public float moveSpeed;
	public float jumpForce;
	public float jumpTimeMax;
	public float jumpDelta;
	public byte flags;
}
