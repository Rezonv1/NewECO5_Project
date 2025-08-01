using Modbus.Device;
using Modbus.Utility;
using System.Net.Sockets;

namespace NewECO5.Services
{
    public class PowerMeterReaderService
    {
        private readonly ILogger<PowerMeterReaderService> _logger;

        public PowerMeterReaderService(ILogger<PowerMeterReaderService> logger)
        {
            _logger = logger;
        }

        public async Task<Dictionary<string, float>> ReadPA60Async(string ip, int port, byte slaveId)
        {
            var result = new Dictionary<string, float>();

            using var client = new TcpClient();
            await client.ConnectAsync(ip, port);

            var master = ModbusIpMaster.CreateIp(client);

            try
            {
                // 讀取從 40001 開始的兩組 float (共 4 個 ushort)
                ushort startAddress = 0;
                ushort numRegisters = 4;

                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                float value1 = ModbusUtility.GetSingle(registers[0], registers[1]);
                float value2 = ModbusUtility.GetSingle(registers[2], registers[3]);

                result["功率"] = value1;
                result["電壓"] = value2;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "讀取失敗");
            }

            return result;
        }
    }
}
