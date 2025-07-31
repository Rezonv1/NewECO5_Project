using Microsoft.EntityFrameworkCore;
using NewECO5.Models;
using NewECO5.ViewModel;
using NewECO5.Models.Enum;
using System.Text.RegularExpressions;

namespace NewECO5.Services
{
    public class Comm_UnitSettingService
    {
        private readonly NewECO5DBContext _context;

        public Comm_UnitSettingService(NewECO5DBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得全部通訊設定
        /// </summary>
        public async Task<List<Comm_UnitSettingViewModel>> GetAllAsync()
        {
            var settings = await _context.Comm_UnitSettings.ToListAsync();
            return settings.Select(Comm_UnitSettingMapper.ToViewModel).ToList();
        }

        /// <summary>
        /// 以 ID 取得單筆設定
        /// </summary>
        public async Task<Comm_UnitSettingViewModel?> GetByIdAsync(short id)
        {
            var setting = await _context.Comm_UnitSettings.FindAsync(id);
            return setting == null ? null : Comm_UnitSettingMapper.ToViewModel(setting);
        }

        /// <summary>
        /// 建立通訊設定
        /// </summary>
        public async Task<(bool Success, string? ErrorMessage)> CreateAsync(Comm_UnitSettingViewModel vm)
        {
            try
            {
                var entity = Comm_UnitSettingMapper.ToEntity(vm);
                _context.Comm_UnitSettings.Add(entity);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// 更新通訊設定
        /// </summary>
        public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(short id, Comm_UnitSettingViewModel vm)
        {
            try
            {
                var setting = await _context.Comm_UnitSettings.FindAsync(id);
                if (setting == null)
                    return (false, "設定不存在");

                setting.ConnectionStr = Comm_UnitSettingMapper.BuildConnectionStr(vm);
                setting.IsTCPSetting = vm.IsTCPSetting;
                setting.Description = vm.Description;

                _context.Comm_UnitSettings.Update(setting);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// 刪除設定
        /// </summary>
        public async Task DeleteAsync(short id)
        {
            var setting = await _context.Comm_UnitSettings.FindAsync(id);
            if (setting != null)
            {
                _context.Comm_UnitSettings.Remove(setting);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 查詢通訊設定
        /// </summary>
        public async Task<List<Comm_UnitSettingViewModel>> SearchAsync(string? query, string? field)
        {
            var meterQuery = _context.Comm_UnitSettings.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query) && !string.IsNullOrWhiteSpace(field))
            {
                query = query.ToLower();
                switch (field.ToLower())
                {
                    case "description":
                        meterQuery = meterQuery.Where(m => m.Description != null && m.Description.ToLower().Contains(query));
                        break;
                    case "istcpsetting":
                        meterQuery = meterQuery.Where(m =>
                            m.IsTCPSetting.ToString().ToLower().Contains(query)
                            || (m.IsTCPSetting == CommModeEnum.RS485 && "rs-485".Contains(query))
                            || (m.IsTCPSetting == CommModeEnum.TCP && "tcp/ip".Contains(query))
                        );
                        break;
                    case "ip":
                        meterQuery = meterQuery.Where(m => m.ConnectionStr != null && m.ConnectionStr.ToLower().Contains(query));
                        break;
                }
            }

            var result = await meterQuery.ToListAsync();
            return result.Select(Comm_UnitSettingMapper.ToViewModel).OrderBy(x => x.Id).ToList();
        }
    }
}
