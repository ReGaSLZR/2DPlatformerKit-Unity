using UnityEngine;
	
public class SpawnUtil {

	private static int MAX_SPAWN_CHANCE = 10;

	public static void Spawn(SpawnableObject spawnObject, Instantiator instantiator, GameObject parent) {
		if(spawnObject != null) {
			int randomChance = Random.Range(0, MAX_SPAWN_CHANCE);

			if(randomChance <= spawnObject.spawnChance) {
				instantiator.InjectPrefab(spawnObject.prefabSpawn, parent);
			}
		}
		else {
			LogUtil.PrintWarning(parent.GetType(), "Spawn() single param is NULL");
		}
	}

	public static void Spawn(SpawnableObject[] spawnObjects, Instantiator instantiator, GameObject parent) {
		if((spawnObjects != null) && (spawnObjects.Length > 0)) {
			for(int x=0; x<spawnObjects.Length; x++) {
				Spawn(spawnObjects[x], instantiator, parent);
			}
		}
		else {
			LogUtil.PrintWarning(parent.GetType(), "Spawn() array param is NULL or empty.");
		}
	}

}


/* ---------------------------------------------------------------------------------------- */

[System.Serializable]
public class SpawnableObject {

	[Tooltip("Chance to Spawn (full chance at 10)")]
	[Range(0, 10)]
	public int spawnChance = 5;
	public GameObject prefabSpawn;

}
