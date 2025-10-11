using UnityEngine;

public abstract class BaseEffect : MonoBehaviour
{
    [SerializeField] protected AnimationCurve _curve;
    [SerializeField] protected float _duration = 1.0f;
}
