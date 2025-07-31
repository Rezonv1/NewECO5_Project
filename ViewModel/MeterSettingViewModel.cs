using NewECO5.Models;
using NewECO5.Models.Enum;

using System.ComponentModel.DataAnnotations;

namespace NewECO5.ViewModel {
    public class MeterSettingViewModel {
        #region 基本屬性
        /// <summary>
        /// 主鍵，設備的唯一識別碼（流水號），唯讀（由資料庫生成）
        /// </summary>
        [Display(Name = "設備流水號")]
        public int SerialNr { get; set; }

        /// <summary>
        /// 儀表站號
        /// </summary>
        [Required(ErrorMessage = "站號為必填項")]
        [Range(0, 255, ErrorMessage = "站號必須在 0 到 255 之間")]
        [Display(Name = "站號")]
        public byte MeterID { get; set; }

        /// <summary>
        /// 是否啟用通訊
        /// </summary>
        [Display(Name = "是否啟用")]
        public bool IsActived { get; set; }

        /// <summary>
        /// Modbus 格式，RTU=0, TCP=1
        /// </summary>
        [Display(Name = "Modbus 格式")]
        public ModbusModeEnum? Mode { get; set; }
        #endregion

        #region 廠牌設定
        [Required(ErrorMessage = "請選擇廠牌")]
        [Display(Name = "廠牌")]
        public string? Brand { get; set; }
        [Required(ErrorMessage = "請選擇型號")]
        [Display(Name = "型號")]
        public string? Model { get; set; }
        [Display(Name = "接線方式")]
        public string? WiringMode { get; set; }
        [Display(Name = "迴圈")]
        public short? Loop { get; set; }
        /// <summary>
        /// 關聯 Protocol 資料表的 Id
        /// </summary>
        public long Protocol_Id { get; set; }


        #endregion

        #region 通訊設定
        [Display(Name = "通訊字串")]
        public string? ConnectionStr { get; set; }
        [Display(Name = "是否為TCP")]
        public CommModeEnum? IsTCPSetting { get; set; }
        /// <summary>
        /// 關聯 MetersGroupSetting_Id 資料表的 Id
        /// </summary>
        public short MetersGroupSetting_Id { get; set; }




        /// <summary>
        /// 通訊協定字串
        /// </summary>
        [MaxLength(500, ErrorMessage = "通訊協定字串長度不能超過 500 個字元")]
        [Display(Name = "通訊協定字串")]
        public string? ProtocolStr { get; set; } = "";
        #endregion
    }
}

