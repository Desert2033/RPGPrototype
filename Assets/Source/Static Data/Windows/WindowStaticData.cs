using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WindowStaticData", menuName = "StaticData/WindowStaticData")]
public class WindowStaticData : ScriptableObject
{
    public List<WindowConfig> Configs;
}
