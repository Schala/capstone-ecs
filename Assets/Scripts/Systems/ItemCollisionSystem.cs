using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

/// <summary>
/// Handle item collision logic.
/// </summary>
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class ItemCollisionSystem : SystemBase
{
	BuildPhysicsWorld buildPhysicsWorld;
	StepPhysicsWorld stepPhysicsWorld;
	BeginFixedStepSimulationEntityCommandBufferSystem bufferSystem;

	/// <summary>
	/// Handle item trigger events.
	/// </summary>
	struct ItemTriggerEventsJob : ITriggerEventsJob
	{
		[ReadOnly] public ComponentDataFromEntity<PlayerTag> playerGroup;
		[ReadOnly] public ComponentDataFromEntity<ItemTag> itemGroup;
		[ReadOnly] public ComponentDataFromEntity<DeleteTag> deletionGroup;
		public EntityCommandBuffer buffer;

		/// <summary>
		/// Determine the entities involved in the trigger event, and then perform the item consume logic.
		/// </summary>
		/// <param name="triggerEvent"></param>
		public void Execute(TriggerEvent triggerEvent)
		{
			var a = triggerEvent.EntityA;
			var b = triggerEvent.EntityB;

			var isPlayerA = playerGroup.HasComponent(a);
			var isPlayerB = playerGroup.HasComponent(b);

			var isItemA = itemGroup.HasComponent(a);
			var isItemB = itemGroup.HasComponent(b);

			if (isPlayerA && isItemB) Consume(a, b);
			if (isPlayerB && isItemA) Consume(b, a);
		}

		/// <summary>
		/// Consume the item, adding its effect to the player.
		/// </summary>
		/// <param name="player">Entity designated as the player</param>
		/// <param name="item">Entity designated as the item</param>
		void Consume(Entity player, Entity item)
		{
			if (deletionGroup.HasComponent(item)) return;

			buffer.AddComponent(item, typeof(DeleteTag));
		}
	}

	/// <summary>
	/// Set up our physics to handle item collision
	/// </summary>
	protected override void OnCreate()
	{
		buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
		stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
		bufferSystem = World.GetOrCreateSystem<BeginFixedStepSimulationEntityCommandBufferSystem>();
	}

	protected override void OnUpdate()
	{
		var job = new ItemTriggerEventsJob
		{
			playerGroup = GetComponentDataFromEntity<PlayerTag>(true),
			itemGroup = GetComponentDataFromEntity<ItemTag>(true),
			deletionGroup = GetComponentDataFromEntity<DeleteTag>(true),
			buffer = bufferSystem.CreateCommandBuffer()
		};

		var handle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);
		handle.Complete();
	}
}
