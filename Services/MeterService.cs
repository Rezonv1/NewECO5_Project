using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewECO5.Models;
using NewECO5.Models.Enum;
using NewECO5.ViewModel;

namespace NewECO5.Services;


public class MeterService : IMeterService
{
    private readonly NewECO5DBContext _context;

    public MeterService(NewECO5DBContext context)
    {
        _context = context;
    }

    public async Task<List<MeterListItemViewModel>> GetMeterListAsync(string? searchQuery, string? searchField)
    {
        var meterQuery = _context.MeterSettings
            .GroupJoin(_context.PowerMeterSettings,
                ms => ms.SerialNr,
                pms => pms.SerialNr,
                (ms, pms) => new { MeterSetting = ms, PowerMeterSettings = pms })
            .SelectMany(x => x.PowerMeterSettings.DefaultIfEmpty(),
                (ms, pms) => new MeterListItemViewModel
                {
                    SerialNr = ms.MeterSetting.SerialNr,
                    DeviceName = pms != null ? pms.DeviceName : null,
                    Description = pms != null ? pms.Description : null,
                });

        if (!string.IsNullOrEmpty(searchQuery) && !string.IsNullOrEmpty(searchField))
        {
            searchQuery = searchQuery.ToLower();
            switch (searchField.ToLower())
            {
                case "serialnr":
                    if (int.TryParse(searchQuery, out int serialNrVal))
                        meterQuery = meterQuery.Where(m => m.SerialNr == serialNrVal);
                    break;
                case "devicename":
                    meterQuery = meterQuery.Where(m => m.DeviceName != null && m.DeviceName.ToLower().Contains(searchQuery));
                    break;
                case "description":
                    meterQuery = meterQuery.Where(m => m.Description != null && m.Description.ToLower().Contains(searchQuery));
                    break;
            }
        }

        return await meterQuery.OrderBy(x => x.SerialNr).ToListAsync();
    }

    public async Task<MeterDetailViewModel> GetMeterDetailAsync(int? serialNr, string? searchQuery, string? searchField)
    {
        var viewModel = new MeterDetailViewModel
        {
            SearchQuery = searchQuery,
            SearchField = searchField,
            MeterList = await GetMeterListAsync(searchQuery, searchField),
            Brands = await GetBrandsAsync(),
            Description = await _context.Comm_UnitSettings
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Description
                }).ToListAsync()
        };

        if (serialNr.HasValue)
        {
            var setting = await _context.MeterSettings.FirstOrDefaultAsync(x => x.SerialNr == serialNr.Value);
            if (setting != null)
            {
                viewModel.MeterSetting = new MeterSettingViewModel
                {
                    SerialNr = setting.SerialNr,
                    MeterID = setting.MeterID,
                    IsActived = setting.IsActived,
                    Mode = setting.ModbusMode,
                    Protocol_Id = setting.Protocol_Id,
                    MetersGroupSetting_Id = setting.MetersGroupSetting_Id
                };

                if (setting.Protocol_Id > 0)
                {
                    var protocol = await _context.Protocols.FirstOrDefaultAsync(x => x.Id == setting.Protocol_Id);
                    if (protocol != null)
                    {
                        viewModel.MeterSetting.Brand = protocol.Brand;
                        viewModel.MeterSetting.Model = protocol.Model;
                        viewModel.MeterSetting.WiringMode = protocol.WiringMode;
                        viewModel.MeterSetting.Loop = protocol.Loop;
                    }
                }

                if (setting.MetersGroupSetting_Id > 0)
                {
                    var group = await _context.Comm_UnitSettings.FirstOrDefaultAsync(x => x.Id == setting.MetersGroupSetting_Id);
                    if (group != null)
                    {
                        viewModel.MeterSetting.ConnectionStr = group.ConnectionStr;
                        viewModel.MeterSetting.IsTCPSetting = group.IsTCPSetting;
                    }
                }

                var powerSetting = await _context.PowerMeterSettings.FirstOrDefaultAsync(x => x.SerialNr == serialNr.Value);
                if (powerSetting != null)
                {
                    viewModel.PowerMeterSetting = new PowerMeterSettingViewModel
                    {
                        SerialNr = powerSetting.SerialNr,
                        DeviceName = powerSetting.DeviceName,
                        Description = powerSetting.Description,
                        KVA_Diff = powerSetting.KVA_Diff,
                        KW_Check_limit = powerSetting.KW_Check_limit,
                        BufferToKeep = powerSetting.BufferToKeep,
                        DiagramNr = powerSetting.DiagramNr,
                        IsLowVoltage = powerSetting.IsLowVoltage,
                        AutoReset = powerSetting.AutoReset,
                        Dmd_Interval = powerSetting.Dmd_Interval,
                        Dmd_Capacity = powerSetting.Dmd_Capacity,
                        Dmd_CapacityHalf = powerSetting.Dmd_CapacityHalf,
                        Dmd_CapacitySatHalf = powerSetting.Dmd_CapacitySatHalf,
                        Dmd_CapacityOff = powerSetting.Dmd_CapacityOff,
                        Dmd_UpperLimit = powerSetting.Dmd_UpperLimit,
                        Dmd_LowerLimit = powerSetting.Dmd_LowerLimit,
                        Dmd_Mode = powerSetting.Dmd_Mode,
                        Alarm_I_UpperLimit = powerSetting.Alarm_I_UpperLimit,
                        Alarm_I_LowerLimit = powerSetting.Alarm_I_LowerLimit,
                        Alarm_V_Type = powerSetting.Alarm_V_Type,
                        Alarm_V_UpperLimit = powerSetting.Alarm_V_UpperLimit,
                        Alarm_V_LowerLimit = powerSetting.Alarm_V_LowerLimit,
                        Alarm_HzUpperLimit = powerSetting.Alarm_HzUpperLimit,
                        Alarm_HzLowerLimit = powerSetting.Alarm_HzLowerLimit,
                        Alarm_PfUpperLimit = powerSetting.Alarm_PfUpperLimit,
                        Alarm_PfLowerLimit = powerSetting.Alarm_PfLowerLimit,
                        Alarm_KWh_UpperLimit = powerSetting.Alarm_KWh_UpperLimit,
                        Alarm_kW_UpperLimit = powerSetting.Alarm_kW_UpperLimit,
                        Alarm_kW_LowerLimit = powerSetting.Alarm_kW_LowerLimit,
                        Alarm_kW_IdelValue = powerSetting.Alarm_kW_IdelValue,
                        Alarm_notPowerOff_kW = powerSetting.Alarm_notPowerOff_kW,
                        Alarm_notPowerOffAlarmStartTime = powerSetting.Alarm_notPowerOffAlarmStartTime,
                        Alarm_notPowerOffAlarmEndTime = powerSetting.Alarm_notPowerOffAlarmEndTime,
                        Area = powerSetting.Area,
                        Persons = powerSetting.Persons,
                        CustomName = powerSetting.CustomName,
                        CustomValue = powerSetting.CustomValue
                    };
                }


                if (!string.IsNullOrEmpty(viewModel.MeterSetting.Brand))
                {
                    viewModel.Models = await GetModelsAsync(viewModel.MeterSetting.Brand);
                    viewModel.WiringMode = await GetWiringModesAsync(viewModel.MeterSetting.Brand, viewModel.MeterSetting.Model);
                    viewModel.Loop = await GetLoopsAsync(viewModel.MeterSetting.Brand, viewModel.MeterSetting.Model);
                }
            }
        }

        return viewModel;
    }

    public async Task<bool> SaveMeterDetailAsync(MeterDetailViewModel viewModel)
    {
        if (viewModel.MeterSetting == null)
            return false;

        var meter = await _context.MeterSettings
            .FirstOrDefaultAsync(x => x.SerialNr == viewModel.MeterSetting.SerialNr)
            ?? new MeterSetting { SerialNr = viewModel.MeterSetting.SerialNr };

        meter.MeterID = viewModel.MeterSetting.MeterID;
        meter.IsActived = viewModel.MeterSetting.IsActived;
        meter.ModbusMode = viewModel.MeterSetting.Mode;
        meter.MetersGroupSetting_Id = viewModel.MeterSetting.MetersGroupSetting_Id;

        // Protocol 查找與組裝 ProtocolStr
        var protocol = await _context.Protocols.FirstOrDefaultAsync(p =>
            p.Brand == viewModel.MeterSetting.Brand &&
            p.Model == viewModel.MeterSetting.Model &&
            (viewModel.MeterSetting.WiringMode == "無" || p.WiringMode == viewModel.MeterSetting.WiringMode) &&
            (viewModel.MeterSetting.Loop == 0 || p.Loop == viewModel.MeterSetting.Loop));

        if (protocol != null)
        {
            meter.Protocol_Id = protocol.Id;

            var prefix = $"{protocol.Brand}_{protocol.Model}";
            if (!string.IsNullOrWhiteSpace(protocol.WiringMode) && protocol.WiringMode != "無")
                prefix += $"_{protocol.WiringMode}";
            if (protocol.Loop > 0)
                prefix += $"_{protocol.Loop}";

            meter.ProtocolStr = $"<{prefix}>{protocol.Content}</{prefix}>";
        }

        // MetersGroupSetting 內容帶入
        var comm = await _context.Comm_UnitSettings.FirstOrDefaultAsync(x => x.Id == meter.MetersGroupSetting_Id);
        if (comm != null)
        {
            viewModel.MeterSetting.ConnectionStr = comm.ConnectionStr;
            viewModel.MeterSetting.IsTCPSetting = comm.IsTCPSetting;
        }

        if (_context.Entry(meter).State == EntityState.Detached)
            _context.MeterSettings.Add(meter);

        // PowerMeterSetting 更新
        if (viewModel.PowerMeterSetting != null)
        {
            var power = await _context.PowerMeterSettings
                .FirstOrDefaultAsync(x => x.SerialNr == meter.SerialNr)
                ?? new PowerMeterSetting { SerialNr = meter.SerialNr };

            power.DeviceName = viewModel.PowerMeterSetting.DeviceName;
            power.Description = viewModel.PowerMeterSetting.Description;
            power.KVA_Diff = viewModel.PowerMeterSetting.KVA_Diff ?? 0.2f;
            power.KW_Check_limit = viewModel.PowerMeterSetting.KW_Check_limit ?? 100000f;
            power.BufferToKeep = viewModel.PowerMeterSetting.BufferToKeep ?? 31;
            power.DiagramNr = viewModel.PowerMeterSetting.DiagramNr;
            power.IsLowVoltage = viewModel.PowerMeterSetting.IsLowVoltage;
            power.AutoReset = viewModel.PowerMeterSetting.AutoReset;
            power.Dmd_Interval = viewModel.PowerMeterSetting.Dmd_Interval ?? 1.0f;
            power.Dmd_Capacity = viewModel.PowerMeterSetting.Dmd_Capacity ?? 0.0f;
            power.Dmd_CapacityHalf = viewModel.PowerMeterSetting.Dmd_CapacityHalf ?? 0.0f;
            power.Dmd_CapacitySatHalf = viewModel.PowerMeterSetting.Dmd_CapacitySatHalf ?? 0.0f;
            power.Dmd_CapacityOff = viewModel.PowerMeterSetting.Dmd_CapacityOff ?? 0.0f;
            power.Dmd_UpperLimit = viewModel.PowerMeterSetting.Dmd_UpperLimit ?? 0.0f;
            power.Dmd_LowerLimit = viewModel.PowerMeterSetting.Dmd_LowerLimit ?? 0.0f;
            power.Dmd_Mode = viewModel.PowerMeterSetting.Dmd_Mode;
            power.Alarm_I_UpperLimit = viewModel.PowerMeterSetting.Alarm_I_UpperLimit ?? 0.0f;
            power.Alarm_I_LowerLimit = viewModel.PowerMeterSetting.Alarm_I_LowerLimit ?? 0.0f;
            power.Alarm_V_Type = viewModel.PowerMeterSetting.Alarm_V_Type;
            power.Alarm_V_UpperLimit = viewModel.PowerMeterSetting.Alarm_V_UpperLimit ?? 0.0f;
            power.Alarm_V_LowerLimit = viewModel.PowerMeterSetting.Alarm_V_LowerLimit ?? 0.0f;
            power.Alarm_HzUpperLimit = viewModel.PowerMeterSetting.Alarm_HzUpperLimit ?? 0.0f;
            power.Alarm_HzLowerLimit = viewModel.PowerMeterSetting.Alarm_HzLowerLimit ?? 0.0f;
            power.Alarm_PfUpperLimit = viewModel.PowerMeterSetting.Alarm_PfUpperLimit ?? 0.0f;
            power.Alarm_PfLowerLimit = viewModel.PowerMeterSetting.Alarm_PfLowerLimit ?? 0.0f;
            power.Alarm_KWh_UpperLimit = viewModel.PowerMeterSetting.Alarm_KWh_UpperLimit ?? 0.0f;
            power.Alarm_kW_UpperLimit = viewModel.PowerMeterSetting.Alarm_kW_UpperLimit ?? 0.0f;
            power.Alarm_kW_LowerLimit = viewModel.PowerMeterSetting.Alarm_kW_LowerLimit ?? 0.0f;
            power.Alarm_kW_IdelValue = viewModel.PowerMeterSetting.Alarm_kW_IdelValue ?? 0.5f;
            power.Alarm_notPowerOff_kW = viewModel.PowerMeterSetting.Alarm_notPowerOff_kW ?? 0.0f;
            power.Alarm_notPowerOffAlarmStartTime = viewModel.PowerMeterSetting.Alarm_notPowerOffAlarmStartTime?.ToUniversalTime() ?? DateTime.UtcNow;
            power.Alarm_notPowerOffAlarmEndTime = viewModel.PowerMeterSetting.Alarm_notPowerOffAlarmEndTime?.ToUniversalTime() ?? DateTime.UtcNow;
            power.Area = viewModel.PowerMeterSetting.Area ?? 0.0f;
            power.Persons = viewModel.PowerMeterSetting.Persons ?? 0;
            power.CustomName = viewModel.PowerMeterSetting.CustomName ?? "無";
            power.CustomValue = viewModel.PowerMeterSetting.CustomValue ?? 0.0f;

            if (_context.Entry(power).State == EntityState.Detached)
                _context.PowerMeterSettings.Add(power);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<SelectListItem>> GetBrandsAsync()
    {
        return await _context.Protocols
            .Select(x => x.Brand)
            .Distinct()
            .Select(b => new SelectListItem { Value = b, Text = b })
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetModelsAsync(string brand)
    {
        return await _context.Protocols
            .Where(p => p.Brand == brand)
            .Select(p => new SelectListItem { Value = p.Model, Text = p.Model })
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetWiringModesAsync(string brand, string model)
    {
        return await _context.Protocols
            .Where(p => p.Brand == brand && p.Model == model)
            .Select(p => new SelectListItem
            {
                Value = p.WiringMode,
                Text = string.IsNullOrEmpty(p.WiringMode) ? "無" : p.WiringMode
            }).Distinct().ToListAsync();
    }

    public async Task<List<SelectListItem>> GetLoopsAsync(string brand, string model)
    {
        return await _context.Protocols
            .Where(p => p.Brand == brand && p.Model == model)
            .Select(p => new SelectListItem
            {
                Value = p.Loop.ToString(),
                Text = p.Loop == 0 ? "無" : p.Loop.ToString()
            }).Distinct().ToListAsync();
    }
    public async Task<bool> SaveMeterSettingAsync(MeterDetailViewModel viewModel, ModelStateDictionary modelState)
    {
        if (viewModel.MeterSetting == null || viewModel.MeterSetting.MeterID == 0)
        {
            modelState.AddModelError("MeterSetting.MeterID", "MeterID 為必填欄位。");
            return false;
        }

        return await SaveMeterDetailAsync(viewModel);
    }

    public async Task<List<MeterListItemViewModel>> SearchMetersAsync(string searchQuery, string searchField)
    {
        return await GetMeterListAsync(searchQuery, searchField);
    }

}