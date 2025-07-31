using Microsoft.AspNetCore.Mvc.Rendering;
using NewECO5.Models;
using NewECO5.Models.Enum;


namespace NewECO5.ViewModel {
    public class MeterDetailViewModel {
        /// <summary>
        /// 當前 MeterSetting 詳細資訊
        /// </summary>
        public MeterSettingViewModel MeterSetting { get; set; } = new MeterSettingViewModel();


        /// <summary>
        /// 相關的 PowerMeterSetting 清單
        /// </summary>
        public PowerMeterSettingViewModel? PowerMeterSetting { get; set; }

        /// <summary>
        /// 電表清單，用於右邊顯示
        /// </summary> 
        public List<MeterListItemViewModel> MeterList { get; set; } = new List<MeterListItemViewModel>();
        /// <summary>
        /// 廠牌下拉選單
        /// </summary>
        public List<SelectListItem> Brands { get; set; } = new List<SelectListItem>();
        /// <summary>
        /// 型號下拉選單
        /// </summary>
        public List<SelectListItem> Models { get; set; } = new List<SelectListItem>();
        /// <summary>
        /// 接線方式下拉選單
        /// </summary>
        public List<SelectListItem> WiringMode { get; set; } = new List<SelectListItem>();
        /// <summary>
        /// 迴圈下拉選單
        /// </summary>
        public List<SelectListItem> Loop { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Description { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> IsTCPSetting { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> ConnectionStr { get; set; } = new List<SelectListItem>();


        public string? SearchQuery { get; set; }
        public string? SearchField { get; set; }
    }
}
