using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NewECO5.Models.Enum
{
    /// <summary>
    /// 通訊方式和協定格式
    /// </summary>
    public enum CommModeEnum {
        [Display(Name = "RS-485")]
        RS485 = 0,
        [Display(Name = "TCP")]
        TCP = 1
    }

    public enum ModbusModeEnum {
        [Display(Name = "RTU")]
        RTU = 0,
        [Display(Name = "TCP")]
        TCP = 1
    }

    /// <summary>
    /// 電壓型式
    /// </summary>
    public enum VotageType {
        [Display(Name = "線電壓")]
        Line = 0,  // 線電壓 (P-P)
        [Display(Name = "相電壓")]
        Phase = 1  // 相電壓 (P-N)
    }
    /// <summary>
    /// 需量預測模式枚舉
    /// </summary>
    public enum DmdMode {
        [Display(Name = "混合式")]
        Hybrid = 0,
        [Display(Name = "簡單線性回歸")]
        SimpleLinearRegression = 1,
        [Display(Name = "多項式")]
        Polynomial = 2,
        [Display(Name = "雙指數平滑法")]
        DoubleExponentialSmoothing = 3
    }

    public enum CategoryEnum {
        [Description("電表")]
        Meter = 1,
        [Description("熱量計")]
        Hot = 2,
        [Description("空壓流量計")]
        Air = 3,
        [Description("溫溼度計")]
        Temp = 4,
        [Description("壓力計")]
        Pressure = 5,
        [Description("瓦斯流量計")]
        Gas = 6,
        [Description("冰水機溫溼度計")]
        TempBTU = 7,
    }
}