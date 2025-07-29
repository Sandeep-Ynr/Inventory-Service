using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Infrastructure.Common.Approval.Handlers;

namespace MilkMatrix.Admin.Common.Handlers.Approval;

public class StatusPageHandler : DefaultHandler
{
    public override FactoryMapping HandlerKey => FactoryMapping.Status;

    public StatusPageHandler(ILogging logging, IRepositoryFactory repositoryFactory)
        : base(logging, repositoryFactory)
    {

    }
}
