using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

/// <summary>
/// Disposes of entities marked for deletion
/// </summary>
[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(ItemCollisionSystem))]
public class DeletionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var buffer = new EntityCommandBuffer(Allocator.TempJob);

        Entities.WithAll<DeleteTag>().ForEach((Entity entity) => {
            buffer.DestroyEntity(entity);
        }).Run();

        buffer.Playback(EntityManager);
        buffer.Dispose();
    }
}
