using System;
using Unity.Entities;

/// <summary>
/// Holds data from the player's input
/// </summary>
[Serializable]
public struct PlayerInput : IComponentData
{
	public const byte Fire = 1;
	public const byte Jump = 2;
	public const byte DoubleJump = 4;

	public float movement;
	public byte flags;
}
