using System;
using Unity.Entities;

/// <summary>
/// Holds data from the player's input
/// </summary>
[Serializable]
public struct PlayerInput : IComponentData
{
	public float movement;
	public bool jump;
	public bool doubleJump;
}
