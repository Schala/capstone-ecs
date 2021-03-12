using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

/// <summary>
/// Collision logic for player
/// </summary>
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(ItemCollisionSystem))]
public class PlayerCollisionSystem : SystemBase
{
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;

	/// <summary>
	/// Handle player collision events
	/// </summary>
	struct PlayerCollisionEventsJob : ICollisionEventsJob
	{
		public ComponentDataFromEntity<PlayerMovement> movementGroup;
		public ComponentDataFromEntity<PlayerInput> inputGroup;
		[ReadOnly] public ComponentDataFromEntity<PlayerTag> playerGroup;
		[ReadOnly] public ComponentDataFromEntity<PlatformTag> platformGroup;

		/// <summary>
		/// Determine the entities involved in the collision event, and then perform the player landing logic.
		/// </summary>
		/// <param name="collisionEvent"></param>
		public void Execute(CollisionEvent collisionEvent)
		{
			var a = collisionEvent.EntityA;
			var b = collisionEvent.EntityB;

			var isPlayerA = playerGroup.HasComponent(a);
			var isPlayerB = playerGroup.HasComponent(b);

			var isPlatformA = platformGroup.HasComponent(a);
			var isPlatformB = platformGroup.HasComponent(b);

			if (isPlayerA && isPlatformB) Land(a);
			if (isPlayerB && isPlatformA) Land(b);
		}

		/// <summary>
		/// Player landed on a platform. Let them jump again.
		/// </summary>
		/// <param name="player">Entity designated as the player</param>
		void Land(Entity player)
		{
			var movement = movementGroup[player];
			var input = inputGroup[player];

			if (input.jump) input.jump = false;
			if (input.doubleJump) input.doubleJump = false;

			movement.jumpDelta = 0f;
			movement.grounded = true;

			movementGroup[player] = movement;
			inputGroup[player] = input;
		}
	}

	/// <summary>
	/// Set up our physics to handle player collision
	/// </summary>
	protected override void OnCreate()
	{
		buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
		stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
	}

	/// <summary>
	/// Iterate through our collision events.
	/// </summary>
	protected override void OnUpdate()
	{
		var job = new PlayerCollisionEventsJob
		{
			movementGroup = GetComponentDataFromEntity<PlayerMovement>(),
			inputGroup = GetComponentDataFromEntity<PlayerInput>(),
			playerGroup = GetComponentDataFromEntity<PlayerTag>(true),
			platformGroup = GetComponentDataFromEntity<PlatformTag>(true)
		};

		var handle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);
		handle.Complete();
	}
}
