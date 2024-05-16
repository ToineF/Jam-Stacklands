using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualData")]
public class DragNDropVisualData : ScriptableObject
{
    [field:Header("Follow")]
    [field:SerializeField] public Vector2 ParentOffset { get; private set; }
    [field:SerializeField, Range(0, 1)] public float CardFollowLerp { get; private set; }
    [field:Header("Out of Bounds")]
    [field:SerializeField] public float CardOutOfBoundsLerpTime { get; private set; }
    [field:Header("Repel")]
    [field:SerializeField] public float CardRepelDistance { get; private set; }
    [field:SerializeField] public float CardRepelTime { get; private set; }
    [field:Header("Drag")]
    [field:SerializeField] public float CardDraggedScaleAmount{ get; private set; }
    [field:SerializeField] public float CardDraggedScaleTime{ get; private set; }
    [field:Header("Drop")]
    [field:SerializeField] public float CardDroppedScaleTime{ get; private set; }
    [field:Header("Focus")]
    [field:SerializeField] public float CardFocusShakeForce { get; private set; }
    [field:SerializeField] public float CardFocusShakeTime { get; private set; }
    [field:Header("Spawn")]
    [field:SerializeField] public float CardSpawnDistance { get; private set; }
    [field:SerializeField] public float CardSpawnTime { get; private set; }
    [field:Header("Booster Buy")]
    [field:SerializeField] public float BoosterEjectionTime { get; private set; }
    [field:SerializeField] public float BoosterEjectionSpeed { get; private set; }

    [field:Header("Shop")]
    [field:SerializeField] public Vector2 OverShopParentOffset { get; private set; }
}
