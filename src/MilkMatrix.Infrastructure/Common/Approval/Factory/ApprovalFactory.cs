using Microsoft.Extensions.DependencyInjection;
using MilkMatrix.Core.Abstractions.Approval.Factory;
using MilkMatrix.Core.Abstractions.Approval.Handler;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Infrastructure.Common.Approval.Handlers;

namespace MilkMatrix.Infrastructure.Common.Approval.Factory
{
    public class ApprovalFactory : IApprovalFactory
    {
        private readonly IEnumerable<IApprovalHandler> handlers;

        public ApprovalFactory(IEnumerable<IApprovalHandler> handlers)
        {
            this.handlers = handlers;
        }

        public IApprovalHandler GetHandler(FactoryMapping key)
        {
            return handlers.FirstOrDefault(h => h.HandlerKey == key)
                ?? handlers.First(h => h.HandlerKey == FactoryMapping.Default);
        }
    }
}
