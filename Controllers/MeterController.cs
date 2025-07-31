using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewECO5.Services;
using NewECO5.ViewModel;

namespace NewECO5.Controllers
{
    public class MeterController : Controller
    {
        private readonly IMeterService _service;

        public MeterController(IMeterService service)
        {
            _service = service;
        }

        // 讀取電表詳細資料
        public async Task<IActionResult> Detail(int? serialNr, string? searchQuery = null, string? searchField = null)
        {
            var viewModel = await _service.GetMeterDetailAsync(serialNr, searchQuery, searchField);
            return View(viewModel);
        }

        // 儲存電表設定
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(MeterDetailViewModel viewModel)
        {
            if (!await _service.SaveMeterSettingAsync(viewModel, ModelState))
            {
                // ⚠️ 如果驗證失敗，要明確指定回 Detail.cshtml，避免回到錯誤的 View（例如 Index）
                return View("Detail", viewModel);
            }

            TempData["SuccessMessage"] = "電表設定已成功儲存。";
            return RedirectToAction(nameof(Detail), new { serialNr = viewModel.MeterSetting.SerialNr });
        }

        // 搜尋用
        [HttpPost]
        public async Task<IActionResult> DetailSearch(string searchQuery, string searchField)
        {
            var results = await _service.SearchMetersAsync(searchQuery, searchField);
            return Json(results);
        }

        // Ajax：依品牌取得型號
        public async Task<JsonResult> GetModels(string brand) =>
            Json(await _service.GetModelsAsync(brand));

        // Ajax：依品牌與型號取得接線模式
        public async Task<JsonResult> GetWiringModes(string brand, string model) =>
            Json(await _service.GetWiringModesAsync(brand, model));

        // Ajax：依品牌與型號取得迴路數
        public async Task<JsonResult> GetLoops(string brand, string model) =>
            Json(await _service.GetLoopsAsync(brand, model));
    }
}
