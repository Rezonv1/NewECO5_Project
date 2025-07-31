using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewECO5.Models.Enum;
using NewECO5.Services;
using NewECO5.ViewModel;
using System.Text.RegularExpressions;

namespace NewECO5.Controllers;

public class CommUnitSettingController : Controller
{
    private readonly Comm_UnitSettingService _service;

    public CommUnitSettingController(Comm_UnitSettingService service)
    {
        _service = service;
    }

    // GET: 通訊設定列表
    public async Task<IActionResult> Index()
    {
        var viewModels = await _service.GetAllAsync();
        return View(viewModels);
    }

    // GET: 建立通訊設定
    public IActionResult Create()
    {
        return View(new Comm_UnitSettingViewModel());
    }

    // POST: 建立通訊設定
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Comm_UnitSettingViewModel viewModel)
    {
        if (viewModel == null)
        {
            ModelState.AddModelError("", "提交資料無效。");
            return View();
        }

        ValidateViewModel(viewModel);

        if (!ModelState.IsValid)
            return View(viewModel);

        var result = await _service.CreateAsync(viewModel);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.ErrorMessage ?? "無法建立通訊設定。");
            return View(viewModel);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: 編輯設定
    public async Task<IActionResult> Edit(short id)
    {
        var viewModel = await _service.GetByIdAsync(id);
        if (viewModel == null)
            return NotFound();

        return View(viewModel);
    }

    // POST: 更新設定
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(short id, Comm_UnitSettingViewModel viewModel)
    {
        if (id != viewModel.Id)
            return BadRequest();

        ValidateViewModel(viewModel);

        if (!ModelState.IsValid)
            return View(viewModel);

        var result = await _service.UpdateAsync(id, viewModel); // ✅ 修正錯誤：補上 id

        if (!result.Success)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return View(viewModel);
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: 刪除確認頁
    public async Task<IActionResult> Delete(short id)
    {
        var viewModel = await _service.GetByIdAsync(id);
        if (viewModel == null)
            return NotFound();

        return View(viewModel);
    }

    // POST: 確認刪除
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(short id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // AJAX 搜尋功能
    [HttpGet]
    public async Task<IActionResult> SearchMeters(string? searchQuery, string? searchField)
    {
        var results = await _service.SearchAsync(searchQuery, searchField);
        return Json(results);
    }

    /// <summary>
    /// 通訊參數驗證：根據模式檢查 IP/Port 格式是否正確
    /// </summary>
    private void ValidateViewModel(Comm_UnitSettingViewModel viewModel)
    {
        if (viewModel.IsTCPSetting == CommModeEnum.TCP)
        {
            if (string.IsNullOrEmpty(viewModel.Port))
            {
                ModelState.AddModelError("Port", "TCP 模式下埠號為必填項");
            }
            else if (!Regex.IsMatch(viewModel.Port, @"^\d+$") || !int.TryParse(viewModel.Port, out int port) || port < 0 || port > 65535)
            {
                ModelState.AddModelError("Port", "埠號必須是 0 到 65535 之間的數字");
            }

            if (!Regex.IsMatch(viewModel.IP, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
            {
                ModelState.AddModelError("IP", "請輸入有效的 IP 地址 (例如 192.168.1.1)");
            }
        }
        else
        {
            if (!Regex.IsMatch(viewModel.IP, @"^\d+,\d+,[0-2]$"))
            {
                ModelState.AddModelError("IP", "請輸入有效的串列參數 (例如 9600,8,1)");
            }

            // RS485 模式不需要 Port，清空
            viewModel.Port = string.Empty;
        }
    }
}
