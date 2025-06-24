using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.SubModule
{
    public class SubModuleUpdateRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? Order { get; set; } = 1;

        public CrudActionType ActionType { get; set; } = CrudActionType.Update;

        public int ModifyBy { get; set; }

        public bool IsAcive { get; set; }
    }
}
