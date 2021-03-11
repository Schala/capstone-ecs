using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// Drives the movement logic behind items.
/// </summary>
public class ItemMovementSystem : SystemBase
{
    /// <summary>
    /// Bob our item up and down.
    /// </summary>
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var runTime = (float)Time.ElapsedTime;

        Entities.WithAll<ItemTag>().ForEach((ref Translation translation, in ItemMovement movement) =>
        {
            var deltaHeight = math.sin(runTime + deltaTime) - math.sin(runTime);
            translation.Value.y += deltaHeight * movement.heightFactor;
        }).Schedule();
    }
}
