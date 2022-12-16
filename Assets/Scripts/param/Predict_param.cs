using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class Predict_param : ScriptableObject
{
    /// <summary>
    /// 予測表示の配列の個数
    /// </summary>
    [Label("予測表示の計算の配列の大きさ")]
    public int BUF_NUMBER;

    /// <summary>
    /// アクセルゲイン
    /// </summary>
    [Label("アクセルゲイン")]
    public double K_pdl;

    /// <summary>
    /// ブレーキゲイン
    /// </summary>
    [Label("ブレーキゲイン")]
    public double K_brk;

    /// <summary>
    /// 予測表示の更新頻度
    /// </summary>
    [Label("予測表示の更新頻度[s]")]
    public double timeOut;

    /// <summary>
    /// ブレーキのふかんたい
    /// </summary>
    [Label("ブレーキの不感帯")]
    public double dead_band;

    /// <summary>
    /// 最高加速度m/s
    /// </summary>
    [Label("最高加速度m/s")]
    public double acc_max;

    /// <summary>
    /// 最小加速度m/s
    /// </summary>
    [Label("最小加速度m/s")]
    public double acc_min;

    /// <summary>
    /// ステアリングの最大
    /// </summary>
    [Label("最大ステアリング角")]
    public double steer_max;

    /// <summary>
    /// ステアリングの最小
    /// </summary>
    [Label("最小ステアリング角")]
    public double steer_min;

    /// <summary>
    /// 軌跡の長さ
    /// </summary>
    [Label("軌跡の長さ[m]")]
    public double traj_length;

    /// <summary>
    /// 軌跡の細かさ
    /// </summary>
    [Label("軌跡の細かさ")]
    public double traj_res;

    /// <summary>
    /// 車両モデルの位置
    /// </summary>
    [Label("車両モデルの位置[m]")]
    public double vehicle_pos;
}