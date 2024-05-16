using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "AudioData")]
public class AudioData : ScriptableObject
{
    [field:SerializeField] public AudioClip CardDragBegin { get; private set; }
    [field:SerializeField] public AudioClip CardDragEnd { get; private set; }
    [field:SerializeField] public AudioClip CardsCraft { get; private set; }
    [field:SerializeField] public AudioClip CardSpawn { get; private set; }
    [field:SerializeField] public AudioClip CardSell { get; private set; }
    [field:SerializeField] public AudioClip BoosterBuy { get; private set; }
    [field:SerializeField] public AudioClip UIClick { get; private set; }
}
