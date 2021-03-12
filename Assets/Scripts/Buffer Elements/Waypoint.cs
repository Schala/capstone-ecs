using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// A position we can insert into a dynamic buffer
/// </summary>
[InternalBufferCapacity(2)]
public struct Waypoint : IBufferElementData
{
	public float3 Value;

	public static implicit operator float3(Waypoint wp) => wp.Value;
	public static implicit operator Waypoint(float3 f3) => new Waypoint { Value = f3 };
	public static implicit operator Waypoint(Vector3 v3) => new Waypoint { Value = v3 };
}
