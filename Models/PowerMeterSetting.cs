using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NewECO5.Models.Enum;

namespace NewECO5.Models
{
    /// <summary>
    /// 電表特定設定
    /// </summary>
    public class PowerMeterSetting
    {
        #region 基本屬性
        /// <summary>
        /// 主鍵，同時作為外鍵，關聯到 MeterSetting 的 SerialNr
        /// </summary>
        [Key]
        [ForeignKey("MeterSetting")]
        [Required]
        public int SerialNr { get; set; }

        /// <summary>
        /// 數值檢查誤差，預設值：0.2
        /// </summary>
        public float KVA_Diff { get; set; } = 0.2f;

        /// <summary>
        /// kW 值檢查上限，預設值：100000
        /// </summary>
        public float KW_Check_limit { get; set; } = 100000;

        /// <summary>
        /// 允許暫存的儀表數值天數，預設值：31
        /// </summary>
        public int BufferToKeep { get; set; } = 31;

        /// <summary>
        /// 裝置名稱
        /// </summary>
        [MaxLength(50)]
        public string? DeviceName { get; set; }

        /// <summary>
        /// 裝置描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 單線圖編號，格式：AA-BB-CC，例如 01-01-03
        /// </summary>
        [MaxLength(20)]
        public string? DiagramNr { get; set; }

        /// <summary>
        /// 是否為台電低壓供電
        /// </summary>
        public bool IsLowVoltage { get; set; }

        /// <summary>
        /// 每月電表累計值自動歸零
        /// </summary>
        public bool AutoReset { get; set; }
        #endregion

        #region 需量參數 (dmdParameter)
        /// <summary>
        /// 計算頻率，單位：分鐘，例如 1、3、5
        /// </summary>
        public float Dmd_Interval { get; set; }

        /// <summary>
        /// 經常契約容量
        /// </summary>
        public float Dmd_Capacity { get; set; }

        /// <summary>
        /// 半尖峰（非夏月）契約容量
        /// </summary>
        public float Dmd_CapacityHalf { get; set; }

        /// <summary>
        /// 週六半尖峰契約容量
        /// </summary>
        public float Dmd_CapacitySatHalf { get; set; }

        /// <summary>
        /// 離峰契約容量
        /// </summary>
        public float Dmd_CapacityOff { get; set; }

        /// <summary>
        /// 上限百分比
        /// </summary>
        public float Dmd_UpperLimit { get; set; }

        /// <summary>
        /// 下限百分比
        /// </summary>
        public float Dmd_LowerLimit { get; set; }

        /// <summary>
        /// 需量預測模式，0=混合式, 1=簡單線性回歸, 2=多項式, 3=雙指數平滑法
        /// </summary>
        public DmdMode Dmd_Mode { get; set; }
        #endregion

        #region 警報參數 (alarmParameter)
        /// <summary>
        /// 電流上限
        /// </summary>
        public float Alarm_I_UpperLimit { get; set; }

        /// <summary>
        /// 電流下限
        /// </summary>
        public float Alarm_I_LowerLimit { get; set; }

        /// <summary>
        /// 電壓型式，0=線電壓(P-P), 1=相電壓(P-N)
        /// </summary>
        public VotageType Alarm_V_Type { get; set; }

        /// <summary>
        /// 電壓上限
        /// </summary>
        public float Alarm_V_UpperLimit { get; set; }

        /// <summary>
        /// 電壓下限
        /// </summary>
        public float Alarm_V_LowerLimit { get; set; }

        /// <summary>
        /// 頻率上限
        /// </summary>
        public float Alarm_HzUpperLimit { get; set; }

        /// <summary>
        /// 頻率下限
        /// </summary>
        public float Alarm_HzLowerLimit { get; set; }

        /// <summary>
        /// 功因上限
        /// </summary>
        public float Alarm_PfUpperLimit { get; set; }

        /// <summary>
        /// 功因下限
        /// </summary>
        public float Alarm_PfLowerLimit { get; set; }

        /// <summary>
        /// 電量上限
        /// </summary>
        public float Alarm_KWh_UpperLimit { get; set; }

        /// <summary>
        /// 功率上限
        /// </summary>
        public float Alarm_kW_UpperLimit { get; set; }

        /// <summary>
        /// 功率下限
        /// </summary>
        public float Alarm_kW_LowerLimit { get; set; }

        /// <summary>
        /// 功率待機值，預設值：0.5kW
        /// </summary>
        public float Alarm_kW_IdelValue { get; set; } = 0.5f;

        /// <summary>
        /// 用電未關閉功率上限值
        /// </summary>
        public float Alarm_notPowerOff_kW { get; set; }

        /// <summary>
        /// 檢查用電未關閉警報開始時間
        /// </summary>
        public DateTime Alarm_notPowerOffAlarmStartTime { get; set; }

        /// <summary>
        /// 檢查用電未關閉警報結束時間
        /// </summary>
        public DateTime Alarm_notPowerOffAlarmEndTime { get; set; }
        #endregion

        #region 效率 (efficiencyParameter)
        /// <summary>
        /// 面積，單位:平方公尺
        /// </summary>
        public float? Area { get; set; }

        /// <summary>
        /// 人數
        /// </summary>
        public int? Persons { get; set; }

        /// <summary>
        /// 自訂名稱
        /// </summary>
        [MaxLength(50)]
        public string? CustomName { get; set; }

        /// <summary>
        /// 自訂數值
        /// </summary>
        public float? CustomValue { get; set; }
        #endregion



        #region 導航屬性
        /// <summary>
        /// 關聯的通訊設定
        /// </summary>
        public virtual MeterSetting MeterSetting { get; set; } = null!;
        #endregion
    }
}