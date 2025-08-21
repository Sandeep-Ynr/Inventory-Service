using System.Data;
using Azure.Core;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Admin.GlobleSetting;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.Sequance;
using MilkMatrix.Milk.Models.Response.Admin.GlobleSetting;
using MilkMatrix.Milk.Models.Response.Admin.GlobleSetting.Sequance;
using MilkMatrix.Milk.Models.Response.Bank;
using MilkMatrix.Milk.Models.Response.Geographical;
using MilkMatrix.Milk.Models.Response.MPP;
using static MilkMatrix.Milk.Models.Queries.BankQueries;
using static MilkMatrix.Milk.Models.Queries.SequenceQueries;
namespace MilkMatrix.Milk.Implementations
{
    public class SequenceService : ISequenceService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public SequenceService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(SequenceService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task Insertsequence(SequenceInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "HeadName", request.HeadName },
                    { "prefix", request.Prefix ?? (object)DBNull.Value },
                    { "StartValue", request.StartValue ?? (object)DBNull.Value },
                    { "StopValue", request.StopValue ?? (object)DBNull.Value },
                    { "IncrementValue", request.IncrementValue ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? 0 }
                };

                var message = await repository.AddAsync(SequenceQuery.InsupdSequence, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Sequence {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Insert/Update Sequence: {request.HeadName}", ex);
                throw;
            }

        }

        public Task DeleteSequance(int id, int userId)
        {
            throw new NotImplementedException();
        }

        //public Task SeqTransCloneforAllDocs(string clonefromfy, string newfy)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<IListsResponse<SequenceResponse>> GetSequanceList(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<SequenceResponse, int, FiltersMeta>(SequenceQuery.GetSequenceList,
                    DbConstants.Main,
                    parameters,
                    null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<SequenceResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public  async Task<SequenceResponse?> GetSequanceById(string HeadName)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Get Sequance id: {HeadName}");
                var repo = repositoryFactory
                           .ConnectDapper<SequenceResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<SequenceResponse>(SequenceQuery.GetSequenceList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "HeadName", HeadName }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new SequenceResponse();
                logging.LogInfo(result != null
                    ? $"Sequance with id {HeadName} retrieved successfully."
                    : $"Sequance with id {HeadName} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Sequance id: {HeadName}", ex);
                throw;
            }
        }

        public async Task Updatesequence(SequenceUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "HeadName", request.HeadName },
                    { "prefix", request.Prefix ?? (object)DBNull.Value },
                    { "StartValue", request.StartValue ?? (object)DBNull.Value },
                    { "StopValue", request.StopValue ?? (object)DBNull.Value },
                    { "IncrementValue", request.IncrementValue ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy ?? 0 }
                };

                var message = await repository.UpdateAsync(SequenceQuery.InsupdSequence, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Sequence {message} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Update Sequence: {request.HeadName}", ex);
                throw;
            }

        }

        public async Task<NextNumberResponse> GetNextNumberforSeq(string HeadName)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called Next No generation for: {HeadName}");
                var repo = repositoryFactory
                           .ConnectDapper<NextNumberResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<NextNumberResponse>(SequenceQuery.GenNextSeqNo, new Dictionary<string, object>
                {
                    
                    { "HeadName", HeadName }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new NextNumberResponse();
                logging.LogInfo(result != null
                    ? $"Next No against {HeadName} retrieved successfully."
                    : $"Head Name with  {HeadName} not found.");
                return result ;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Next Sequence No for HeadName = {HeadName}. Exception: {ex}");
                throw new Exception(ex.Message);
            }
        }

        public async Task InsertsequenceTrans(SequenceTransInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "HeadName", request.HeadName },
                    { "financial_year", request.fy_year },
                    { "delimiter", request.delimiter ?? (object)DBNull.Value },
                    { "prefix", request.Prefix ?? (object)DBNull.Value },
                    { "suffix", request.suffix ?? (object)DBNull.Value},
                    { "StartValue", request.StartValue ?? (object)DBNull.Value },
                    { "StopValue", request.StopValue ?? (object)DBNull.Value },
                    { "IncrementValue", request.IncrementValue ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? 0 }
                };

                var message = await repository.AddAsync(SequenceQuery.InsupdSequenceTrans, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Sequence {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Insert/Update Sequence: {request.HeadName}", ex);
                throw;
            }

        }

        public async Task UpdatesequenceTrans(SequenceTransUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "HeadName", request.HeadName },
                    { "financial_year", request.fy_year },
                    { "delimiter", request.delimiter ?? (object)DBNull.Value },
                    { "prefix", request.Prefix ?? (object)DBNull.Value },
                    { "suffix", request.suffix ?? (object)DBNull.Value},
                    { "StartValue", request.StartValue ?? (object)DBNull.Value },
                    { "StopValue", request.StopValue ?? (object)DBNull.Value },
                    { "IncrementValue", request.IncrementValue ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy ?? 0 }
                };

                var message = await repository.UpdateAsync(SequenceQuery.InsupdSequenceTrans, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Sequence {message} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Update Sequence: {request.HeadName}", ex);
                throw;
            }

        }

        public async  Task<SequenceTransResponse?> GetSequanceTransById(string HeadName, string FY)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Get Sequance id: {HeadName}");
                var repo = repositoryFactory
                           .ConnectDapper<SequenceTransResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<SequenceTransResponse>(SequenceQuery.GetSequenceTransList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "HeadName", HeadName },
                    { "financialyear", FY }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new SequenceTransResponse();
                logging.LogInfo(result != null
                    ? $"Sequance with id {HeadName} retrieved successfully."
                    : $"Sequance with id {HeadName} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Sequance id: {HeadName}", ex);
                throw;
            }
        }

        public async Task<SequenceTransResponse?> SeqTransCloneforAllDocs(string clonefromfy, string newfy,int userId)
        {
            try
            {
                logging.LogInfo($"SeqTransCloneforAllDocs from: {clonefromfy} to {newfy}");

                var repo = repositoryFactory
                           .ConnectDapper<SequenceTransResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<SequenceTransResponse>(
                    SequenceQuery.GenClonefornextfyfromold,
                    new Dictionary<string, object>
                    {
                { "FromFinancialYear", clonefromfy },
                { "NewFinancialYear", newfy },
                { "CreatedBy", userId }
                    },
                    null);

                var result = data.FirstOrDefault(); // return null if empty

                if (result == null)
                {
                    logging.LogWarning($"No clone created. Kindly check if FY {clonefromfy} exists.");
                    throw new Exception($"No clone created. Kindly check if FY {clonefromfy} exists.");

                }
                else
                {
                    logging.LogInfo($"New Clone {newfy} against {clonefromfy} retrieved successfully.");
                }

                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error SeqTransCloneforAllDocs from: {clonefromfy} to {newfy}. Exception:", ex);
                throw new Exception(ex.Message); // don’t wrap ex.Message only, preserve stack trace
            }
        }

        public async Task<SequenceTransResponse?> SeqTransCloneforSelectiveHead(string clonefromfy, string fromhead, string newfy, int userId)
        {
            try
            {
                logging.LogInfo($"SeqTransCloneforAllDocs from: {clonefromfy} to {newfy}");

                var repo = repositoryFactory
                           .ConnectDapper<SequenceTransResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<SequenceTransResponse>(
                    SequenceQuery.sequence_clone_by_head_fy,
                    new Dictionary<string, object>
                    {
                 {"HeadName",fromhead },
                { "FromFinancialYear", clonefromfy },
                { "NewFinancialYear", newfy },
                { "CreatedBy", userId }
                    },
                    null);

                var result = data.FirstOrDefault(); // return null if empty

                if (result == null)
                {
                    logging.LogWarning($"No clone created. Kindly check if FY {clonefromfy} and {fromhead} exists.");
                    throw new Exception($"No clone created. Kindly check if FY {clonefromfy}  and {fromhead}    exists.");

                }
                else
                {
                    logging.LogInfo($"New Clone {newfy} against {clonefromfy} and {fromhead}  retrieved successfully.");
                }

                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error SeqTransCloneforAllDocs from: {clonefromfy} and {fromhead}  to {newfy} and {fromhead} . Exception:", ex);
                throw new Exception(ex.Message); // don’t wrap ex.Message only, preserve stack trace
            }
        }

        public async Task<SeqTransNextNumberResponse> GetNextNumberforSeqTrans(string HeadName, string FY)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called Next No generation for: {HeadName}");
                var repo = repositoryFactory
                           .ConnectDapper<SeqTransNextNumberResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<SeqTransNextNumberResponse>(SequenceQuery.GenNextSeqNoforTrans, new Dictionary<string, object>
                {

                    { "HeadName", HeadName },
                    { "financial_year", FY }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new SeqTransNextNumberResponse();
                logging.LogInfo(result != null
                    ? $"Next No against {HeadName} retrieved successfully."
                    : $"Head Name with  {HeadName} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Next Sequence No for HeadName = {HeadName}. Exception: {ex}");
                throw new Exception(ex.Message);
            }
        }
     
        public async Task<IListsResponse<SequenceTransResponse>> GetSequanceTransList(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<SequenceTransResponse, int, FiltersMeta>(SequenceQuery.GetSequenceTransList,
                    DbConstants.Main,
                    parameters,
                    null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<SequenceTransResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
