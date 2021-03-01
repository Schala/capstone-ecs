using Unity.Entities;
using UnityEngine;

/// <summary>
/// Handles startup conversion to Unity's new data oriented tech stack, and tracks game-wide variables
/// </summary>
public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; } = null;

	EntityManager entityManager;
	World world = null;

	/// <summary>
	/// Runs upon game creation, ensuring only one game manager instance and setting up data oriented tech stack
	/// </summary>
	private void Awake()
	{
		if (Instance != null)
			Destroy(gameObject);

		Instance = this;
		DontDestroyOnLoad(gameObject);

		world = World.DefaultGameObjectInjectionWorld;
		entityManager = world.EntityManager;
	}
}
