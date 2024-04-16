using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject effectObject;
    public RuntimeAnimatorController[] animations;

    public void Create(EffectCode effect, float duration, Vector2 position, float size = 3, int layer = 0)
    {
        GameObject createdEffect = Instantiate(effectObject, position, Quaternion.identity);
        createdEffect.transform.localScale = new Vector3(size, size, 0);

        createdEffect.GetComponent<Animator>().runtimeAnimatorController = animations[(int)effect];
        createdEffect.GetComponent<SpriteRenderer>().sortingOrder = layer;
        createdEffect.GetComponent<DestroyObject>().timer = duration;
    }
}
