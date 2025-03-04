using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ne3ma.Contracts.FoodItems;
using Ne3ma.Services;

namespace Ne3ma.Controllers;

[Authorize]
[Route("food-items")]
[ApiController]
public class FoodItemController(IFoodItemService foodItemService) : ControllerBase
{
    private readonly IFoodItemService _foodItemService = foodItemService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _foodItemService.GetAllAsync();
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _foodItemService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FoodItemRequest request)
    {
        var result = await _foodItemService.CreateAsync(request);

        if (!result.IsSuccess || result.Value is null)
            return result.ToProblem();

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FoodItemRequest request)
    {
        var result = await _foodItemService.UpdateAsync(id, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _foodItemService.DeleteAsync(id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
