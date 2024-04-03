using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRespository _commentRepo;
        private readonly IStockRespository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRespository commentRepo, IStockRespository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllComment()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync();
            var commentsDto = comments.Select(item => item.ToCommentDto());
            return Ok(commentsDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetById(id);
            if(comment == null)
                return NotFound("No comment");
            return Ok(comment.ToCommentDto());
        }
        [HttpPost("{StockId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int StockId, CreateCommentRequest commentDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.GetByIdAsync(StockId);
            if(stock == null)
                return NotFound("This Stock no more existed");

            var username = User.GetuserName();
            var appUser = await _userManager.FindByNameAsync(username);

            Comment comment = commentDto.FromCreatedDto(StockId);
            comment.AppUserId = appUser.Id;

            await _commentRepo.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new {id = comment.Id}, comment.ToCommentDto());
            // return Ok(comment);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateCommentRequestDto commentDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Comment commentModel = commentDto.FromUpdatedDto();
            var comment = await _commentRepo.UpdateAsync(id, commentModel);
            if(comment == null)
                return NotFound("This comment not more existed");
            return Ok(comment.ToCommentDto());
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) 
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                
            bool isDeleted = await _commentRepo.DeleteAsync(id);
            if(isDeleted == false)
                return NotFound("The comment does not exist.");
            return NoContent();
        }
    }
}