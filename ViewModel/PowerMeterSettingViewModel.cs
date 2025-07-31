using NewECO5.Models;
using NewECO5.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace NewECO5.ViewModel {
    /// <summary>
    /// 電表特定設定 ViewModel
    /// </summary>
    public class PowerMeterSettingViewModel {
        #region 基本屬性
        /// <summary>
        /// 主鍵，同時作為外鍵，關聯到 MeterSetting 的 SerialNr，僅顯示
        /// </summary>
        [Display(Name = "設備流水號")]
        public int SerialNr { get; set; }


        /// <summary>
        /// 數值檢查誤差，預設值：0.2
        /// </summary>
        [Display(Name = "數值檢查誤差")]
        public float? KVA_Diff { get; set; } = 0.2f;

        /// <summary>
        /// kW 值檢查上限，預設值：100000
        /// </summary>
        [Display(Name = "kW 檢查上限")]
        public float? KW_Check_limit { get; set; } = 100000;

        /// <summary>
        /// 允許暫存的儀表數值天數，預設值：31
        /// </summary>
        [Display(Name = "暫存天數")]
        public int? BufferToKeep { get; set; } = 31;

        /// <summary>
        /// 裝置名稱
        /// </summary>
        [MaxLength(50, ErrorMessage = "裝置名稱長度不能超過 50 個字元")]
        [Display(Name = "裝置名稱")]
        public string? DeviceName { get; set; }

        /// <summary>
        /// 裝置描述
        /// </summary>
        [Display(Name = "裝置描述")]
        public string? Description { get; set; }

        /// <summary>
        /// 單線圖編號，格式：AA-BB-CC，例如 01-01-03
        /// </summary>
        [MaxLength(20, ErrorMessage = "單線圖編號長度不能超過 20 個字元")]
        [RegularExpression(@"^\d{2}-\d{2}-\d{2}$", ErrorMessage = "單線圖編號格式必須為 AA-BB-CC，例如 01-01-03")]
        [Display(Name = "單線圖編號")]
        public string? DiagramNr { get; set; }

        /// <summary>
        /// 是否為台電低壓供電
        /// </summary>
        [Display(Name = "台電低壓供電")]
        public bool IsLowVoltage { get; set; }

        /// <summary>
        /// 每月電表累計值自動歸零
        /// </summary>
        [Display(Name = "自動歸零")]
        public bool AutoReset { get; set; }
        #endregion

        #region 需量參數 (dmdParameter)
        /// <summary>
        /// 計算頻率，單位：分鐘，例如 1、3、5
        /// </summary>
        [Display(Name = "計算頻率 (分鐘)")]
        public float? Dmd_Interval { get; set; } = 1.0f;

        /// <summary>
        /// 經常契約容量
        /// </summary>
        [Display(Name = "經常(尖峰)")]
        public float? Dmd_Capacity { get; set; }

        /// <summary>
        /// 半尖峰（非夏月）契約容量
        /// </summary>
        [Display(Name = "半尖峰(非夏月)")]
        public float? Dmd_CapacityHalf { get; set; }

        /// <summary>
        /// 週六半尖峰契約容量
        /// </summary>
        [Display(Name = "週六半尖峰")]
        public float? Dmd_CapacitySatHalf { get; set; }

        /// <summary>
        /// 離峰契約容量
        /// </summary>
        [Display(Name = "離峰")]
        public float? Dmd_CapacityOff { get; set; }

        /// <summary>
        /// 上限百分比
        /// </summary>
        [Display(Name = "上限")]
        public float? Dmd_UpperLimit { get; set; }

        /// <summary>
        /// 下限百分比
        /// </summary>
        [Display(Name = "下限")]
        public float? Dmd_LowerLimit { get; set; }

        /// <summary>
        /// 需量預測模式，0=混合式, 1=簡單線性回歸, 2=多項式, 3=雙指數平滑法
        /// </summary>
        [Required(ErrorMessage = "請選擇需量預測模式")]
        [Display(Name = "需量預測模式")]
        public DmdMode Dmd_Mode { get; set; }
        #endregion

        #region 警報參數 (alarmParameter)
        /// <summary>
        /// 電流上限
        /// </summary>
        [Display(Name = "電流上限")]
        public float? Alarm_I_UpperLimit { get; set; }

        /// <summary>
        /// 電流下限
        /// </summary>
        [Display(Name = "電流下限")]
        public float? Alarm_I_LowerLimit { get; set; }

        /// <summary>
        /// 電壓型式，0=線電壓(P-P), 1=相電壓(P-N)
        /// </summary>
        [Required(ErrorMessage = "請選擇電壓型式")]
        [Display(Name = "電壓型式")]
        public VotageType Alarm_V_Type { get; set; }

        /// <summary>
        /// 電壓上限
        /// </summary>
        [Display(Name = "電壓上限")]
        public float? Alarm_V_UpperLimit { get; set; }

        /// <summary>
        /// 電壓下限
        /// </summary>
        [Display(Name = "電壓下限")]
        public float? Alarm_V_LowerLimit { get; set; }

        /// <summary>
        /// 頻率上限
        /// </summary>
        [Display(Name = "頻率上限")]
        public float? Alarm_HzUpperLimit { get; set; }

        /// <summary>
        /// 頻率下限
        /// </summary>
        [Display(Name = "頻率下限")]
        public float? Alarm_HzLowerLimit { get; set; }

        /// <summary>
        /// 功因上限
        /// </summary>
        [Display(Name = "功因上限")]
        public float? Alarm_PfUpperLimit { get; set; }

        /// <summary>
        /// 功因下限
        /// </summary>
        [Display(Name = "功因下限")]
        public float? Alarm_PfLowerLimit { get; set; }

        /// <summary>
        /// 電量上限
        /// </summary>
        [Display(Name = "電量上限")]
        public float? Alarm_KWh_UpperLimit { get; set; }

        /// <summary>
        /// 功率上限
        /// </summary>
        [Display(Name = "功率上限")]
        public float? Alarm_kW_UpperLimit { get; set; }

        /// <summary>
        /// 功率下限
        /// </summary>
        [Display(Name = "功率下限")]
        public float? Alarm_kW_LowerLimit { get; set; }

        /// <summary>
        /// 功率待機值，預設值：0.5kW
        /// </summary>
        [Display(Name = "功率待機值")]
        public float? Alarm_kW_IdelValue { get; set; } = 0.5f;

        /// <summary>
        /// 用電未關閉功率上限值
        /// </summary>
        [Display(Name = "功率上限")]
        public float? Alarm_notPowerOff_kW { get; set; }

        /// <summary>
        /// 檢查用電未關閉警報開始時間
        /// </summary>
        [Display(Name = "開始時間")]
        public DateTime? Alarm_notPowerOffAlarmStartTime { get; set; }

        /// <summary>
        /// 檢查用電未關閉警報結束時間
        /// </summary>
        [Display(Name = "結束時間")]
        public DateTime? Alarm_notPowerOffAlarmEndTime { get; set; }
        #endregion

        #region 效率 (efficiencyParameter)
        /// <summary>
        /// 面積，單位:平方公尺
        /// </summary>
        public float? Area { get; set; } = 0.0f;

        /// <summary>
        /// 人數
        /// </summary>
        public int? Persons { get; set; } = 0;

        /// <summary>
        /// 自訂名稱
        /// </summary>
        [MaxLength(50)]
        public string? CustomName { get; set; } = "無";

        /// <summary>
        /// 自訂數值
        /// </summary>
        public float? CustomValue { get; set; } = 0.0f;
        #endregion
    }
}
