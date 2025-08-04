using Microsoft.EntityFrameworkCore;
using NewECO5.Models;
using NewECO5.Models.Enum;
using Meter; // 引入 ECO.MeterHub4 的 Meter 套件
using System.Net.Sockets;
using static ModbusLib.Modbus;

namespace NewECO5.Services
{
    public class PowerMeterReaderService
    {
        private readonly NewECO5DBContext _context;

        public PowerMeterReaderService(NewECO5DBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 根據 SerialNr 立即讀取電表資料，回傳欄位名稱與對應值
        /// </summary>
        public async Task<Dictionary<string, float>> ReadNowAsync(int serialNr)
        {
            var meterSetting = await _context.MeterSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.SerialNr == serialNr);

            if (meterSetting == null)
                throw new Exception($"查無 SerialNr = {serialNr} 的儀表設定");

            var commSetting = await _context.Comm_UnitSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == meterSetting.MetersGroupSetting_Id);

            if (commSetting == null)
                throw new Exception($"查無通訊設定 Id = {meterSetting.MetersGroupSetting_Id}");

            // 🟡 DEBUG 輸出
            Console.WriteLine("📡 讀取 SerialNr: " + serialNr);
            Console.WriteLine("🔌 MeterID: " + meterSetting.MeterID);
            Console.WriteLine("🌐 IP: " + (commSetting.IP ?? "null"));
            Console.WriteLine("🌐 Port: " + (commSetting.Port ?? "null"));
            Console.WriteLine("🧭 ProtocolStr: " + (meterSetting.ProtocolStr ?? "[空白]"));

            var comm = new Comm_Unit
            {
                IP = commSetting.IP ?? "192.168.1.35",
                Port = ushort.TryParse(commSetting.Port, out ushort port) ? port : (ushort)502,
                TCP_Setting = $"{commSetting.IP},{commSetting.Port}",
                COM_Setting = commSetting.ConnectionStr ?? "COM1,9600,n,8,1",
                usingTcpString = (commSetting.IsTCPSetting == CommModeEnum.TCP),
                usingSerialString = (commSetting.IsTCPSetting == CommModeEnum.RS485)
            };

            var setting = new Setting
            {
                serialNr = (ushort)meterSetting.SerialNr,
                meterID = (byte)meterSetting.MeterID,
                isActived = meterSetting.IsActived,
                mode = meterSetting.ModbusMode == ModbusModeEnum.TCP ? Mode.TCP : Mode.RTU,
                protocalstr = meterSetting.ProtocolStr ?? ""
            };

            if (!setting.protocalReady)
                throw new Exception("ProtocolStr 格式錯誤：" + setting.settingErr);

            var meter = new Meter.Meter
            {
                commUnit = comm,
                setting = setting
            };

            int resultCode = meter.ReadMeter();

            if (resultCode != 0)
                throw new Exception($"讀取失敗，ErrType={meter.getting.Err_Type}：{meter.getting.Err}");

            var values = meter.getting.values;

            if (values == null || values.Length == 0)
                throw new Exception("讀取成功但回傳值為空");

            // ✅ 儲存資料到資料表 MeterRealtimeRecord
            var timestamp = DateTime.Now;

            var record = new MeterRealtimeRecord
            {
                SerialNr = serialNr,
                Timestamp = timestamp,
                TotalKW = values.Length > 0 ? values[0] : 0,
                VoltageA = values.Length > 1 ? values[1] : 0
            };

            _context.MeterRealtimeRecords.Add(record);
            await _context.SaveChangesAsync();

            // ✅ 回傳 Dictionary 結果
            var result = new Dictionary<string, float>();
            for (int i = 0; i < values.Length; i++)
            {
                result[$"Value{i + 1}"] = values[i];
            }

            return result;
        }
    }
}
