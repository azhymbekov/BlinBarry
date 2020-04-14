using BlinBerry.Services.Common.SelesReport.Models;
using GlobalContants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinBerry.Services.Common.SelesReport
{
    public interface ISelesReportService
    {

        IQueryable<SeleTransactionDto> GetReportsList();

        Task RemoveAsync(Guid id);

        Task<OperationResult> SaveAsync(SeleTransactionDto model, Guid userId);

        Task<SeleTransactionDto> PrepareWordForEditView(Guid? id);
    }
}
