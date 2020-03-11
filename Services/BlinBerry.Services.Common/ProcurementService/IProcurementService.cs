using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlinBerry.Services.Common.ProcurementService.Models;
using GlobalContants;

namespace BlinBerry.Services.Common.ProcurementService
{
    public interface IProcurementService
    {
        Task<ProcurementDto> GetAsync(Guid id);

        IQueryable<ProcurementDto> GetList();

        Task RemoveAsync(Guid id);

        Task<OperationResult> SaveAsync(ProcurementDto model, Guid userId);

        Task<ProcurementDto> PrepareForEditView(Guid? id);

        Task<ProcurementDto> DetailsAsync(Guid userId);
    }
}
