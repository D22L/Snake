using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSkinView : MonoBehaviour
{
    [field: SerializeField] public Renderer[] StartRenderer { get; private set; }
    [field: SerializeField] public Renderer HeadRender { get; private set; }
    [field: SerializeField] public int HeadBaseVaterialIndex { get; private set; }
    
}
