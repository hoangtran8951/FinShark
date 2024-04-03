using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRespository _stockRepo;
        public StockController(IStockRespository stockRepo)
        {
            _stockRepo = stockRepo;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stockDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.GetByIdAsync(id);

            if(stock == null)
                return NotFound("Stock not found");
            
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDtos stockDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = stockDto.FromCreatedDto();
            await _stockRepo.CreateAsync(stock);
            return CreatedAtAction(nameof(GetById), new {id = stock.Id}, stock.ToStockDto());
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDtos updateStockDot){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = await _stockRepo.UpdateAsync(id, updateStockDot);
            if(stock == null)
                return NotFound("Stock not found");
            return Ok(stock.ToStockDto());
        }
        [HttpDelete("{id::int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            bool check = await _stockRepo.DeleteAsync(id);
            if(check == false)
                return NotFound("Stock not found");
            return NoContent();
        }
    }
}