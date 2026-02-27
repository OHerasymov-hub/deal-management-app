using DealService.Api.Filters;
using DealService.Application.Command;
using DealService.Application.Dto;
using DealService.Application.Queries;
using DealService.Domain.Entities;
using DealService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;

        public DealsController(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [TypeFilter<LoggingFilter>(Arguments = ["Get Deals"])]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDeals(CancellationToken cancellationToken)
        {
             
            var deals = await _mediator.Send(new GetDealsQuery(), cancellationToken);
            //Task.Delay(1000);// Simulate some processing delay
            return Ok(deals);
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var surname = User.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value;
            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.GivenName)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            return Ok(new { UserId = userId, UserName = userName, Surname = surname, Email = email });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateDeal(Guid id, [FromBody] UpdateDealCommand updateDealCommand)
        {
            if (id != updateDealCommand.Id) return BadRequest("IDs do not match");
            var result = await _mediator.Send(updateDealCommand);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<ActionResult> ChangeDealStatus(Guid id, [FromBody] DealStatus newStatus)
        {
            var result = await _mediator.Send(new UpdateDealStatus(id, newStatus));
            if (!result) return NotFound();
            return NoContent();
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateDeal([FromBody] CreateDealCommand createDealCommand)
        {
            var dealId = await _mediator.Send(createDealCommand);
            return Ok(new { Id = dealId });
            
        }
    }
}
