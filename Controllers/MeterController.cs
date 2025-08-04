using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewECO5.Models;
using NewECO5.Services;
using NewECO5.ViewModel;
using NModbus;
using System.Net.Sockets;

namespace NewECO5.Controllers
{
    public class MeterController : Controller
    {
        private readonly IMeterService _service;
        private readonly PowerMeterReaderService _readerService;
        private readonly NewECO5DBContext _context;
        private readonly PowerMeterReaderService _powerMeterReaderService;

        public MeterController(IMeterService service, NewECO5DBContext context, PowerMeterReaderService powerMeterReaderService)
        {
            _service = service;
            _context = context;
            _powerMeterReaderService = powerMeterReaderService;
        }

        // 讀取電表詳細資料（支援網址導覽與搜尋參數）
        [HttpGet]
        [Route("Meter/Detail/{serialNr:int?}")]
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
                return View("Detail", viewModel);
            }

            TempData["SuccessMessage"] = "電表設定已成功儲存。";
            return RedirectToAction(nameof(Detail), new { serialNr = viewModel.MeterSetting.SerialNr });
        }

        // 搜尋結果（左側清單）
        [HttpPost]
        public async Task<IActionResult> DetailSearch(string searchQuery, string searchField)
        {
            var results = await _service.SearchMetersAsync(searchQuery, searchField);
            return Json(results);
        }

        // SelectList：設備選單
        private List<SelectListItem> GetSerialSelectList()
        {
            return _context.MeterSettings
                .Select(m => new SelectListItem
                {
                    Value = m.SerialNr.ToString(),
                    Text = $"{m.SerialNr} - {m.DeviceName}"
                }).ToList();
        }

        // 通訊測試頁面 GET
        [HttpGet]
        public IActionResult TestConnection()
        {
            return View();
        }

        // 通訊測試 POST
        [HttpPost]
        public async Task<IActionResult> TestConnection(string ip, int port, byte meterId)
        {
            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(ip, port);

                var factory = new ModbusFactory();
                var master = factory.CreateMaster(client);

                ushort[] registers = await master.ReadHoldingRegistersAsync(meterId, 0, 4);
                ViewBag.Result = $"✅ 成功連接至 {ip}:{port}，讀取值：{string.Join(", ", registers)}";
            }
            catch (Exception ex)
            {
                ViewBag.Result = $"❌ 通訊失敗：{ex.Message}";
            }

            return View();
        }

        // 暫存器掃描頁 GET
        [HttpGet]
        public IActionResult ScanRegisters() => View();

        // 暫存器掃描 POST
        [HttpPost]
        public async Task<IActionResult> ScanRegisters(string ip, int port, byte slaveId, ushort startAddress, ushort numRegisters)
        {
            try
            {
                using var client = new TcpClient(ip, port);
                var factory = new ModbusFactory();
                var master = factory.CreateMaster(client);

                ushort[] data = await master.ReadHoldingRegistersAsync(slaveId, startAddress, numRegisters);
                var result = new Dictionary<ushort, ushort>();
                for (int i = 0; i < data.Length; i++)
                {
                    result[(ushort)(startAddress + i)] = data[i];
                }

                ViewBag.Results = result;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "❌ 掃描失敗：" + ex.Message;
            }

            return View();
        }

        // 即時讀取電表資料
        [HttpGet]
        public async Task<IActionResult> ReadNow(int serialNr)
        {
            try
            {
                var result = await _powerMeterReaderService.ReadNowAsync(serialNr);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> ReadNowPage(int serialNr)
        {
            if (serialNr <= 0)
                return BadRequest("未提供正確的 SerialNr");

            var result = await _powerMeterReaderService.ReadNowAsync(serialNr);

            if (result == null || result.Count == 0)
            {
                ViewBag.Message = "❌ 通訊失敗或未讀取到資料";
                return View("ReadNowPage");
            }

            ViewBag.Message = "✅ 通訊成功，資料如下：";
            ViewBag.Data = result;

            return View("ReadNowPage");
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
