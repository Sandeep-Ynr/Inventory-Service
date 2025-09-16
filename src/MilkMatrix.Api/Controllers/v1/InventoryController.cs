using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Api.Models.Request.Inventory.Item;
using MilkMatrix.Api.Models.Request.Inventory.ItemCategory;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Inventory.Item;
using MilkMatrix.Milk.Contracts.Inventory.ItemCategory;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Inventory.Item;
using MilkMatrix.Milk.Models.Request.Inventory.ItemCategory;
using MilkMatrix.Milk.Models.Response.Inventory.Item;
using MilkMatrix.Milk.Models.Response.Inventory.ItemCategory;
using static MilkMatrix.Api.Common.Constants.Constants;
namespace MilkMatrix.Api.Controllers.v1
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogging logger;
        private readonly IMapper mapper;
        private readonly IItemCatgService itemCatgService;
        private readonly IItemService itemService;
        public InventoryController(
           IHttpContextAccessor httpContextAccessor,
           ILogging logging,
           IItemCatgService itemCatgService,
           IItemService itemService,
           IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logging.ForContext("ServiceName", nameof(InventoryController)) ?? throw new ArgumentNullException(nameof(logging));
            this.itemCatgService = itemCatgService ?? throw new ArgumentNullException(nameof(itemCatgService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        }

        #region item-category
        [HttpPost("list-item-category")]
        public async Task<IActionResult> GetList([FromBody] ListsRequest request)
        {
            var result = await itemCatgService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("get/-item-category{id}")]
        public async Task<ActionResult<ItemCatgResponse?>> GetById(int id)
        {
            try
            {
                logger.LogInfo($"GetById called for ItemCategory ID: {id}");
                var result = await itemCatgService.GetById(id);

                if (result == null)
                {
                    logger.LogInfo($"ItemCategory with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"ItemCategory with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving ItemCategory with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record." + ex.Message);
            }
        }

        [HttpPost("add-item-category")]
        public async Task<IActionResult> Add([FromBody] ItemCatgInsertRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = "Invalid request."
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                logger.LogInfo($"Add called for ItemCategory: {request.Name}");

                var mappedRequest = mapper.MapWithOptions<ItemCatgInsertRequest, ItemCatgInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                         { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await itemCatgService.AddItemCatg(mappedRequest);
                return Ok(new { message = "ItemCategory added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Add ItemCategory", ex);
                return StatusCode(500, $"An error occurred while adding the ItemCategory. {ex.Message}");
            }
        }

        [HttpPut("update-item-category")]
        public async Task<IActionResult> Update([FromBody] ItemCatgUpdateRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid || request.Id <= 0)
                    return BadRequest("Invalid request.");

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;

                var mappedRequest = mapper.MapWithOptions<ItemCatgUpdateRequest, ItemCatgUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                        { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }

                    });

                await itemCatgService.UpdateItemCatg(mappedRequest);
                logger.LogInfo($"ItemCategory with ID {request.Id} updated successfully.");
                return Ok(new { message = "ItemCategory updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError("Error in updating ItemCategory", ex);
                return StatusCode(500, $"An error occurred while updating the ItemCategory. {ex.Message}");
            }
        }

        [HttpDelete("delete/-item-category{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await itemCatgService.Delete(id, Convert.ToInt32(userId));
                logger.LogInfo($"ItemCategory with ID {id} deleted successfully.");
                return Ok(new { message = "ItemCategory deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting ItemCategory with ID: {id}", ex);
                return StatusCode(500, $"An error occurred while deleting the ItemCategory. {ex.Message}");
            }
        }
        #endregion

        #region item
        [HttpPost("list-item")]
        public async Task<IActionResult> GetItemList([FromBody] ListsRequest request)
        {
            var result = await itemService.GetAll(request);
            return Ok(result);
        }

        [HttpGet("Get/-item{id}")]
        public async Task<ActionResult<ItemResponse?>> GetItemById(long id)
        {
            try
            {
                logger.LogInfo($"GetById called for Item ID: {id}");
                var result = await itemService.GetById(id);

                if (result == null)
                {
                    logger.LogInfo($"Item with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInfo($"Item with ID {id} retrieved successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error retrieving Item with ID: {id}", ex);
                return StatusCode(500, "An error occurred while retrieving the record." + ex.Message);
            }
        }

        [HttpPost]
        [Route("item/add")]
        public async Task<IActionResult> AddItem([FromBody] ItemInsertRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = "Invalid request."
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                var requestParams = mapper.MapWithOptions<ItemInsertRequest, ItemInsertRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                { Constants.AutoMapper.CreatedBy, Convert.ToInt64(userId) }
                    });

                await itemService.AddItem(requestParams);

                logger.LogInfo($"Item {request.ItemName} added successfully.");
                return Ok(new { message = "Item added successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error adding Item: {request?.ItemName}", ex);
                return StatusCode(500, $"An error occurred while adding the Item. {ex.Message}");
            }
        }


        [HttpPut]
        [Route("item/update")]
        public async Task<IActionResult> UpdateItem([FromBody] ItemUpdateRequestModel request)
        {
            try
            {
                if (request == null || !ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = "Invalid request."
                    });
                }

                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                var requestParams = mapper.MapWithOptions<ItemUpdateRequest, ItemUpdateRequestModel>(
                    request,
                    new Dictionary<string, object>
                    {
                { Constants.AutoMapper.ModifiedBy, Convert.ToInt64(userId) }
                    });

                await itemService.UpdateItem(requestParams);

                logger.LogInfo($"Item {request.ItemName} updated successfully.");
                return Ok(new { message = "Item updated successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error updating Item: {request?.ItemName}", ex);
                return StatusCode(500, $"An error occurred while updating the Item. {ex.Message}");
            }
        }


        [HttpDelete("delete-item/{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            try
            {
                var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.UserData)?.Value;
                await itemService.Delete(id, Convert.ToInt32(userId));

                logger.LogInfo($"Item with ID {id} deleted successfully.");
                return Ok(new { message = "Item deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting Item with ID: {id}", ex);
                return StatusCode(500, $"An error occurred while deleting the Item. {ex.Message}");
            }
        }

        #endregion
    }
}
