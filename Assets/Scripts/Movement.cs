using System;
using Unity.Entities;
using Unity.Physics;

/// <summary>
/// Movement and physics data for an entity
/// </summary>
[Serializable]
public struct Movement : IComponentData
{
	public float moveSpeed;
}

/// <summary>
/// Movement logic for player
/// </summary>
[AlwaysSynchronizeSystem]
public class PlayerMovementSystem : SystemBase
{
	/// <summary>
	/// Syncs player input with velocity
	/// </summary>
	protected override void OnUpdate()
	{
		var deltaTime = Time.DeltaTime;

		Entities.ForEach((ref PhysicsVelocity velocity, in Movement movement, in PlayerInput input) =>
		{
			var newVelocity = velocity.Linear.yz;
			newVelocity += input.movement * movement.moveSpeed * deltaTime;
			velocity.Linear.yz = newVelocity;
		}).Run();
	}
}

/// <summary>
/// Movement logic for non-player entities
/// </summary>
public class MovementSystem : SystemBase
{
	protected override void OnUpdate()
	{
		var deltaTime = Time.DeltaTime;
	}
}
