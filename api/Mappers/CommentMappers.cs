using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers 
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto 
            {
                Id = comment.Id,
                Title = comment.Title,
                content = comment.content,
                Createdon = comment.Createdon,
                CreatedBy = comment.AppUser.UserName,
                StockId = comment.StockId,
            };
        }
        public static Comment FromCreatedDto(this CreateCommentRequest commentDto, int stockId)
        {
            return new Comment{
                Title = commentDto.Title,
                content = commentDto.content,
                Createdon = DateTime.Now,
                StockId = stockId,
            };
        }
        public static Comment FromUpdatedDto(this UpdateCommentRequestDto commentDto)
        {
            return new Comment{
                Title = commentDto.Title,
                content = commentDto.content,
                Createdon = DateTime.Now,
            };
        }
    }
}