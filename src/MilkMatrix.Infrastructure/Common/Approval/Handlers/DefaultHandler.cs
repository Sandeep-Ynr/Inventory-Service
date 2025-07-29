using MilkMatrix.Core.Abstractions.Approval.Handler;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using static MilkMatrix.Infrastructure.Common.Constants.Constants;
using System.Data;
using MilkMatrix.Core.Entities.Response.Approval;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Infrastructure.Common.Approval.Handlers;

public class DefaultHandler : IApprovalHandler
{
    private readonly IRepositoryFactory repositoryFactory;
    private readonly ILogging logging;

    public virtual FactoryMapping HandlerKey => FactoryMapping.Default;

    public DefaultHandler(ILogging logging, IRepositoryFactory repositoryFactory)
    {
        this.logging = logging.ForContext("ServiceName", nameof(DefaultHandler));
        this.repositoryFactory = repositoryFactory;
    }

    public virtual async Task<bool> CheckConditionsAsync(Dictionary<string, object> parameters)
    {
        if (!parameters.TryGetValue("PageId", out var pageIdObj) ||
            !parameters.TryGetValue("BusinessId", out var businessIdObj) ||
            !parameters.TryGetValue("recordId", out var recordObj))
        {
            logging.LogWarning("Missing required parameters for approval.");
            return false;
        }

        var pageId = Convert.ToInt32(pageIdObj);
        var businessId = Convert.ToInt32(businessIdObj);
        var record = recordObj?.ToString();
        var repo = repositoryFactory.ConnectDapper<object>(DbConstants.Main);

        var dbParams = new Dictionary<string, object>
        {
            ["PageId"] = pageId,
            ["BusinessId"] = businessId
        };

        var allowed = await repo.QueryAsync<ApprovalPages>(
            ApprovalSpName.GetAllowedTablesAndFields,
            dbParams,
            null,
            CommandType.StoredProcedure);

        // Use HashSet with case-insensitive comparer for fast, safe lookups
        var allowedTables = new HashSet<string>(
            allowed.Select(x => x.TableName?.ToString()?.Trim() ?? string.Empty),
            StringComparer.OrdinalIgnoreCase);

        var allowedFields = new HashSet<string>(
            allowed.Select(x => x.FieldName?.ToString()?.Trim() ?? string.Empty),
            StringComparer.OrdinalIgnoreCase);

        if (!allowedTables.Any())
        {
            logging.LogWarning($"Invalid table names");
            return false;
        }
        if (!allowedFields.Any())
        {
            logging.LogWarning($"Invalid field names");
            return false;
        }

        // Add more business rule checks as needed

        return true;
    }

    public virtual async Task<bool> ApproveAsync(Dictionary<string, object> parameters)
    {
        try
        {
            if (!parameters.TryGetValue("PageId", out var pageIdObj) ||
                !parameters.TryGetValue("BusinessId", out var businessIdObj) ||
                !parameters.TryGetValue("recordId", out var recordObj))
            {
                logging.LogWarning("Missing required parameters for approval.");
                return false;
            }

            var pageId = pageIdObj?.ToString()?.Trim();
            var businessId = businessIdObj?.ToString()?.Trim();
            var record = recordObj?.ToString()?.Trim();
            var user = parameters.TryGetValue("userId", out var userId) ? userId : null;

            if (string.IsNullOrEmpty(pageId) || string.IsNullOrEmpty(businessId) || string.IsNullOrEmpty(record))
            {
                logging.LogWarning("TableName, FieldName, or recordId is null or empty.");
                return false;
            }

            var repo = repositoryFactory.ConnectDapper<object>(DbConstants.Main);

            var dbParams = new Dictionary<string, object>
            {
                ["PageId"] = pageId,
                ["BusinessId"] = businessId,
                ["recordId"] = record,
                ["IsApproved"] = true,
                ["ModifyBy"] = user
            };

            var result = await repo.UpdateAsync(ApprovalSpName.SetIsApproved, dbParams, CommandType.StoredProcedure);

            logging.LogInfo($"Approval set for {pageId}.{businessId} = {record}");
            return !string.IsNullOrWhiteSpace(result) &&
                    string.Equals(result.Trim(), "Success", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            logging.LogError("Error during approval process.", ex);
            return false;
        }
    }
}
