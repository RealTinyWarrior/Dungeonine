using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject effectObject;
    public RuntimeAnimatorController[] animations;

    [HideInInspector]
    public enum Effects
    {
        Blast_Effect
    }

    public void Create(Effects effect, float duration, Vector2 position)
    {
        GameObject createdEffect = Instantiate(effectObject, position, Quaternion.identity);
        createdEffect.GetComponent<Animator>().runtimeAnimatorController = animations[(int)effect];
        createdEffect.GetComponent<DestroyObject>().timer = duration;
    }
}
