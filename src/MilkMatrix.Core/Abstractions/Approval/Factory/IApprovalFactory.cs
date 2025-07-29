using MilkMatrix.Core.Abstractions.Approval.Handler;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Core.Abstractions.Approval.Factory;

public interface IApprovalFactory
{
    IApprovalHandler GetHandler(FactoryMapping key);
}
