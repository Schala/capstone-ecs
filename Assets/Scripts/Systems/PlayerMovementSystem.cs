using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;

/// <summary>
/// Movement logic for player
/// </summary>
[AlwaysSynchronizeSystem]
public class PlayerMovementSystem : SystemBase
{
	/// <summary>
	/// Syncs player input with movement
	/// </summary>
	protected override void OnUpdate()
	{
		var deltaTime = Time.DeltaTime;

		Entities.WithAll<PlayerTag>().ForEach((ref PhysicsVelocity velocity, ref Movement movement, ref PhysicsMass mass, ref PlayerInput input) =>
		{
			if ((movement.flags & Movement.Grounded) != 0 && movement.jumpDelta < movement.jumpTimeMax)
				if ((input.flags & PlayerInput.Jump) != 0)
				{
					PhysicsComponentExtensions.ApplyLinearImpulse(ref velocity, in mass, new float3(0f, 1f, 0f) * movement.jumpForce);
					movement.flags ^= Movement.Grounded;
				}

			if ((input.flags & PlayerInput.DoubleJump) != 0)
			{
				velocity.Linear = float3.zero; // Zero out any velocity to counter gravity.
				PhysicsComponentExtensions.ApplyLinearImpulse(ref velocity, in mass, new float3(0f, 1f, 0f) * movement.jumpForce);
			}

			PhysicsComponentExtensions.ApplyLinearImpulse(ref velocity, in mass, new float3(0f, 0f, input.movement * movement.moveSpeed * deltaTime));
			mass.InverseInertia = float3.zero; // freeze rotation workaround

			movement.jumpDelta += deltaTime;
		}).Run();
	}
}
