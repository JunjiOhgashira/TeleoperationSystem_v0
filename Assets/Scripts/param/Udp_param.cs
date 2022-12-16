using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class Udp_param : ScriptableObject
{
    /// <summary>
    /// ポート番号
    /// </summary>
    [Label("ポート番号")]
    public int LOCA_LPORT;

}
