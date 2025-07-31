using NewECO5.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace NewECO5.Models
{
    /// <summary>
    /// 儀表通訊協定總表
    /// </summary>
    public class Protocol
    {
        /// <summary>
        /// 通訊協定識別碼(流水號)
        /// </summary>
        [Key]
        public short Id { get; set; }

        /// <summary>
        /// 儀表類型1=電表2=熱量計3=空壓流量計4=溫溼度計5=壓力計6=瓦斯流量計
        /// </summary>
        public CategoryEnum Category { get; set; }

        /// <summary>
        /// 廠牌
        /// </summary>
        [StringLength(20)]
        public string? Brand { get; set; }

        /// <summary>
        /// 型號
        /// </summary>
        [StringLength(50)]
        public string? Model { get; set; }

        /// <summary>
        /// 接線方式
        /// </summary>
        [StringLength(50)]
        public string? WiringMode { get; set; }

        /// <summary>
        /// 迴路
        /// </summary>
        public short Loop { get; set; }

        /// <summary>
        /// Protocol內容
        /// </summary>
        [StringLength(500)]
        public string? Content { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 建立使用者
        /// </summary>
        [StringLength(50)]
        public string? CreateUser { get; set; } = string.Empty;

        /// <summary>
        /// 更新使用者
        /// </summary>
        [StringLength(50)]
        public string? UpdateUser { get; set; } = string.Empty;
    }
}
