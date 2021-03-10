using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;

/// <summary>
/// Movement logic for player
/// </summary>
[UpdateAfter(typeof(PlayerInputSystem))]
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
			if (movement.grounded && movement.jumpDelta < movement.jumpTimeMax)
				if (input.jump)
				{
					PhysicsComponentExtensions.ApplyLinearImpulse(ref velocity, in mass, new float3(0f, 1f, 0f) * movement.jumpForce);
					movement.grounded = false;
				}

			if (input.doubleJump)
			{
				velocity.Linear = float3.zero; // Zero out any velocity to counter gravity.
				PhysicsComponentExtensions.ApplyLinearImpulse(ref velocity, in mass, new float3(0f, 1f, 0f) * movement.jumpForce * movement.doubleJumpForceMultiplier);
			}

			PhysicsComponentExtensions.ApplyLinearImpulse(ref velocity, in mass, new float3(0f, 0f, input.movement * movement.moveSpeed * deltaTime));
			mass.InverseInertia = float3.zero; // freeze rotation workaround

			if (!movement.grounded)
				movement.jumpDelta += deltaTime;
		}).Run();
	}
}
