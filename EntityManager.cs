using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public GameObject[] entity;

    public GameObject Spawn(EntityCode entityCode, Vector2 position)
    {
        GameObject spawnedEntity = Instantiate(entity[(int)entityCode], position, Quaternion.identity);
        return spawnedEntity;
    }
}