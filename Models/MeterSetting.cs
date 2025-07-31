using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NewECO5.Models.Enum;


namespace NewECO5.Models.Enum
{
    /// <summary>
    /// 儀表(Meter類別)設定值
    /// </summary>
    public class MeterSetting
    {
        #region 基本屬性
        /// <summary>
        /// 主鍵，設備的唯一識別碼（流水號）    
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int SerialNr { get; set; }

        /// <summary>
        /// 儀表站號
        /// </summary>
        public byte MeterID { get; set; }

        /// <summary>
        /// 是否啟用通訊
        /// </summary>
        public bool IsActived { get; set; }

        /// <summary>
        /// Modbus 格式，RTU=0, TCP=1
        /// </summary>
        public ModbusModeEnum? ModbusMode { get; set; }
        #endregion


        #region 通訊設定
        /// <summary>
        /// 關聯 Protocol 資料表的 Id
        /// </summary>
        public short Protocol_Id { get; set; }

        /// <summary>
        /// 關聯 MetersGroupSetting_Id 資料表的 Id
        /// </summary>
        public short MetersGroupSetting_Id { get; set; }

        /// <summary>
        /// 通訊協定字串
        /// </summary>
        [MaxLength(500)]
        public string? ProtocolStr { get; set; } = "";
        #endregion
    }
}
