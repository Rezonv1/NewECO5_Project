using NewECO5.Models;
using NewECO5.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewECO5.ViewModel {
    /// <summary>
    /// 通訊協定設定
    /// </summary>
    public class Comm_UnitSettingViewModel {

        /// <summary>
        /// 通訊協定識別碼
        /// </summary>
        public short Id { get; set; }

        #region 通訊設定
        /// <summary>
        /// 通訊參數，依模式不同用途：
        /// - TCP: IP 地址 (例如 "192.168.1.1")
        /// - RS-485/RTU: 串列參數 (例如 "9600,8,1" 表示 9600 波特率, 8 數據位, 1 停止位)
        /// </summary>
        [Required(ErrorMessage = "IP 地址或串列參數為必填項")]
        [MaxLength(100, ErrorMessage = "通訊參數長度不能超過 100 個字元")]
        [Display(Name = "IP 地址或串列參數")]
        public string IP { get; set; } = "";

        /// <summary>
        /// 埠號或輔助參數，依模式不同用途：
        /// - TCP: 埠號 (0-65535)
        /// - RS-485/RTU: COM 埠號或次要參數 (例如奇偶校驗編碼)
        /// </summary>
        [MaxLength(50, ErrorMessage = "埠號或輔助參數長度不能超過 50 個字元")]
        [Display(Name = "埠號")]
        public string Port { get; set; }

        /// <summary>
        /// 通訊模式，RS485=0, TCP=1
        /// </summary>
        [Display(Name = "通訊方式")]
        public CommModeEnum IsTCPSetting { get; set; } = CommModeEnum.RS485; // 預設值 RS-485
        #endregion

        /// <summary>
        /// 說明
        /// </summary>
        [Display(Name = "描述")]
        public string? Description { get; set; }

    }
}
