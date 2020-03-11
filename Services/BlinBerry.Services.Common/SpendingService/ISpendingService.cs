using System;
using System.Linq;
using System.Threading.Tasks;
using BlinBerry.Services.Common.SpendingService.Models;
using GlobalContants;

namespace BlinBerry.Services.Common.SpendingService
{
    public interface ISpendingService
    {
        IQueryable<SpendingDto> GetList();

        Task RemoveAsync(Guid id);

        Task<OperationResult> SaveAsync(SpendingDto model, Guid userId);

        Task<SpendingDto> PrepareForEditView(Guid? id);

        Task<SpendingDto> DetailsAsync(Guid userId);
    }
}
