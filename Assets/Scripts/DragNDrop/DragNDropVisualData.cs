using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDropVisualData : ScriptableObject
{
    [field:SerializeField] public Vector2 ParentOffset { get; private set; }
}
