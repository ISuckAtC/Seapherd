using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectArtifact : MonoBehaviour, IToolTip
{
    public string ToolTipText;
    public string ToolTip {get {return ToolTipText;}}
    
}
