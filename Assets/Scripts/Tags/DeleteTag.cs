using System;
using Unity.Entities;

/// <summary>
/// Indicates the attached entity is marked for deletion
/// </summary>
[Serializable]
public struct DeleteTag : IComponentData
{
}
