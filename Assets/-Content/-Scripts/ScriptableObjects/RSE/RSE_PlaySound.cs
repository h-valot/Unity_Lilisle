using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "RSE_Sound", menuName = "Data/RSE/Sound")]
public class RSE_Sound : RuntimeScriptableEvent<TypeSound, AudioClip, bool> { }