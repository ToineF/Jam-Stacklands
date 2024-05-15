using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualData")]
public class DragNDropVisualData : ScriptableObject
{
    [field:SerializeField] public Vector2 ParentOffset { get; private set; }
    [field:SerializeField, Range(0, 1)] public float CardFollowLerp { get; private set; }
}
