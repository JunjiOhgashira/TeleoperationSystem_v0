using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class Vehicle_param : ScriptableObject
{
    /// <summary>
    /// オーバーオールステアリングギア比
    /// </summary>
    [Label("ステアリングギア比")]
    public double gear_rasio;

    /// <summary>
    /// ホイールベース
    /// </summary>
    [Label("ホイールベース[m]")]
    public  double W;

    /// <summary>
    /// トレッド
    /// </summary>
    [Label("トレッド[m]")]
    public double T;

    /// <summary>
    /// 重心から前輪までの長さm
    /// </summary>
    [Label("重心から前輪までの長さ[m]")]
    public double lf;

    /// <summary>
    /// 重心から後輪までの長さm
    /// </summary>
    [Label("重心から後輪までの長さ[m]")]
    public double lr;

    /// <summary>
    /// 車両幅の半分 m
    /// </summary>
    [Label("車両幅の半分[m]")]
    public double center_side;

    /// <summary>
    /// 車両長さの半分 m
    /// </summary>
    [Label("車両長さの半分[m]")]
    public double center_long;

    /// <summary>
    /// 車両高さ m
    /// </summary>
    [Label("車両高さ[m]")]
    public double center_up;

    /// <summary>
    /// 重心からのカメラ位置(高さ)
    /// </summary>
    [Label("重心からのカメラ位置（高さ）[m]")]
    public double camera_pos_Y;

    /// <summary>
    /// 重心からのカメラ位置(車軸方向)
    /// </summary>
    [Label("重心からのカメラ位置（車軸方向）[m]")]
    public double camera_pos_Z;
}