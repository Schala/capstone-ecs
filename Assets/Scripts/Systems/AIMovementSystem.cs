using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

/// <summary>
/// Movement logic for non-player, AI-controlled entities
/// </summary>
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class AIMovementSystem : SystemBase
{
	/// <summary>
	/// Have the AI move between set waypoints indefinitely.
	/// </summary>
	protected override void OnUpdate()
	{
		var deltaTime = Time.DeltaTime;

		Entities.WithAll<EnemyTag>().ForEach((DynamicBuffer<Waypoint> waypoints, ref PhysicsVelocity velocity, ref AIMovement movement, ref PhysicsMass mass, ref Rotation rotation, 
			in Translation translation) =>
		{
			var hasArrived = math.distance(translation.Value.xz, waypoints[movement.nextWaypoint].Value.xz) < movement.waypointEpsilon;

			if (hasArrived)
			{
				movement.nextWaypoint = (movement.nextWaypoint + 1) % waypoints.Length;
				rotation.Value = quaternion.LookRotationSafe(waypoints[movement.nextWaypoint], new float3(0f, 1f, 0f));
			}

			PhysicsComponentExtensions.ApplyLinearImpulse(ref velocity, in mass, new float3(0f, 0f, movement.moveSpeed * deltaTime));
			mass.InverseInertia = float3.zero; // freeze rotation workaround
		}).Schedule();
	}
}
