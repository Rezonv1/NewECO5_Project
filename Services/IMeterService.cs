using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewECO5.ViewModel;

namespace NewECO5.Services
{
    public interface IMeterService
    {
        Task<List<MeterListItemViewModel>> GetMeterListAsync(string? searchQuery, string? searchField);
        Task<MeterDetailViewModel> GetMeterDetailAsync(int? serialNr, string? searchQuery, string? searchField);
        Task<bool> SaveMeterDetailAsync(MeterDetailViewModel viewModel);
        Task<bool> SaveMeterSettingAsync(MeterDetailViewModel viewModel, ModelStateDictionary modelState); // ✅ 加回
        Task<List<MeterListItemViewModel>> SearchMetersAsync(string searchQuery, string searchField);       // ✅ 加回

        Task<List<SelectListItem>> GetBrandsAsync();
        Task<List<SelectListItem>> GetModelsAsync(string brand);
        Task<List<SelectListItem>> GetWiringModesAsync(string brand, string model);
        Task<List<SelectListItem>> GetLoopsAsync(string brand, string model);

    }
}
