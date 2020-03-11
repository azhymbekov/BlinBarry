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

        IQueryable<SelesReportDto> GetReportsList();

        Task RemoveAsync(Guid id);

        Task<OperationResult> SaveAsync(SelesReportDto model, Guid userId);

        Task<SelesReportDto> PrepeareWordForEditView(Guid? id);
    }
}
