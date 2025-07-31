using NewECO5.Models;
using NewECO5.Models.Enum;
using NewECO5.ViewModel;

namespace NewECO5.ViewModel
{
    public static class Comm_UnitSettingMapper
    {
        public static Comm_UnitSettingViewModel ToViewModel(Comm_UnitSetting entity)
        {
            var viewModel = new Comm_UnitSettingViewModel
            {
                Id = entity.Id,
                Description = entity.Description,
                IsTCPSetting = entity.IsTCPSetting,
                IP = "",
                Port = ""
            };

            if (!string.IsNullOrWhiteSpace(entity.ConnectionStr))
            {
                if (entity.IsTCPSetting == CommModeEnum.TCP)
                {
                    var parts = entity.ConnectionStr.Split(':');
                    if (parts.Length == 2)
                    {
                        viewModel.IP = parts[0];
                        viewModel.Port = parts[1];
                    }
                }
                else if (entity.IsTCPSetting == CommModeEnum.RS485)
                {
                    // RS485 預設整串丟 IP 欄位就好（例如 "9600,8,1,N"）
                    viewModel.IP = entity.ConnectionStr;
                }
            }

            return viewModel;
        }

        public static Comm_UnitSetting ToEntity(Comm_UnitSettingViewModel vm)
        {
            return new Comm_UnitSetting
            {
                Id = vm.Id,
                Description = vm.Description,
                IsTCPSetting = vm.IsTCPSetting,
                ConnectionStr = BuildConnectionStr(vm)
            };
        }

        public static string BuildConnectionStr(Comm_UnitSettingViewModel vm)
        {
            if (vm.IsTCPSetting == CommModeEnum.TCP)
                return $"{vm.IP}:{vm.Port}";
            else
                return vm.IP; // RS485: IP 欄位存整串參數
        }
    }
}
