using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class Other_param : ScriptableObject
{
    /// <summary>
    /// 予測表示有りモード
    /// </summary>
    [Label("予測表示つけるかどうか"), OnValueChanged("OnValueChanged")]
    public bool predict_mode;

    /// <summary>
    /// 遅延予測表示有りモード
    /// </summary>
    [Label("予測表示モード(Off:遅延補償、On：両方)"), EnableIf("predict_mode"), OnValueChanged("OnValueChanged1")]
    public bool compen_mode;

    /// <summary>
    /// 線の内輪差表示がアクティブがどうか
    /// </summary>
    [Label("提示モード（Off：線、On：2Dモデル）"), EnableIf(EConditionOperator.And, "predict_mode", "compen_mode")]
    public bool display_mode;

    /// <summary>
    /// HMD固定するかどうか
    /// </summary>
    [Label("HMDの位置を固定するかどうか")]
    public bool HMDfix_mode;

    /// <summary>
    /// ペダルを表示するかどうか
    /// </summary>
    [Label("ペダルUIを表示するか")]
    public bool pedal_mode;

    /// <summary>
    /// ステアリング表示するかどうか
    /// </summary>
    [Label("ステアリングUIを表示するか")]
    public bool Steering_mode;

    /// <summary>
    /// ステアリング表示するかどうか
    /// </summary>
    [Label("速度UIを表示するか")]
    public bool Velocity_mode;

    /// <summary>
    /// 速度推定モード
    /// </summary>
    [Label("速度推定モード")]
    public bool Est_vel_mode;
    private void OnValueChanged()
    {
        if(predict_mode == false)
        {
            compen_mode = false;
            display_mode = false;
        }
    }

    private void OnValueChanged1()
    {
        if (compen_mode == false)
        {

            display_mode = false;
        }
    }
}

