using UnityEngine;

public enum PowerupType
{
    Torque,
}

[CreateAssetMenu(fileName = "New PowerupSO", menuName = "Scriptable Objects/PowerupSO")]
public class PowerupSO : ScriptableObject
{
    [SerializeField] private PowerupType _powerupType; public PowerupType @PowerupType => _powerupType;
    [SerializeField] private float _value; public float Value => _value;
    [SerializeField] private float _duration; public float Duration => _duration;
    [SerializeField] private AudioClip _powerupActivatedSFX; public AudioClip PowerupActivatedSFX => _powerupActivatedSFX;
    [SerializeField] private AudioClip _powerupEndedSFX; public AudioClip PowerupEndedSFX => _powerupEndedSFX;
}
