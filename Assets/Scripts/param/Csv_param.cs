using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class Csv_param : ScriptableObject
{
    /// <summary>
    /// ファイルの名前
    /// </summary>
    [Label("ファイル名")]
    public string fileName;

    /// <summary>
    /// HMDの角度だけをを保存するかどうか
    /// </summary>
    [Label("HMDの回転を保存するかどうか")]
    public bool HMD_rot_mode;

    /// <summary>
    /// 推定速度だけをを保存するかどうか
    /// </summary>
    [Label("推定速度保存するかどうか")]
    public bool est_velocity_mode;

    /// <summary>
    /// ステアリング角度だけをを保存するかどうか
    /// </summary>
    [Label("ステアリング角保存するかどうか")]
    public bool steering_rot_mode;

    /// <summary>
    /// アイトラッキング保存
    /// </summary>
    [Label("アイトラキング保存")]
    public bool eye_mode;
}
