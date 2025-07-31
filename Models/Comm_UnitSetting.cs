
using NewECO5.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewECO5.Models
{
    /// <summary>
    /// 硄癟﹚砞﹚
    /// </summary>
    public class Comm_UnitSetting
    {

        /// <summary>
        /// 硄癟﹚醚絏
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        #region 硄癟砞﹚
        /// <summary>
        /// 硄癟把计ㄌ硄癟家Αぃノ硚
        /// - TCP: IP  (ㄒ "192.168.1.1:502")
        /// - RS-485: ﹃把计 (ㄒ "9600,8,1" ボ 9600 猧疭瞯, 8 计沮, 1 氨ゎ)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ConnectionStr { get; set; } = "";

        /// <summary>
        /// 硄癟家ΑRS485=0, TCP=1
        /// </summary>
        public CommModeEnum IsTCPSetting { get; set; } = CommModeEnum.RS485; // 箇砞 RS-485
        #endregion

        /// <summary>
        /// 弧
        /// </summary>
        public string? Description { get; set; }

    }
}
