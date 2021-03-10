using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; } = null;

	[SerializeField] float offset = -10f;
	[SerializeField] float followSpeed = 0.15f;

    public Entity target = Entity.Null;

	EntityManager entityManager;
	Vector3 velocity = Vector3.zero;

	/// <summary>
	/// Set up our singleton instance and variables
	/// </summary>
	private void Awake()
	{
		if (Instance != null)
			Destroy(gameObject);

		Instance = this;
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	/// <summary>
	/// Update the camera to follow the player consistently
	/// </summary>
	void LateUpdate()
    {
		if (target == Entity.Null) return;

		var trans = entityManager.GetComponentData<Translation>(target);
		trans.Value.x += offset;
		transform.position = Vector3.SmoothDamp(transform.position, trans.Value, ref velocity, Time.deltaTime * followSpeed);
    }
}
