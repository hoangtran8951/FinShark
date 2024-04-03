using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace api.Respository
{
    public class CommentRespository: ICommentRespository
    {
        public readonly ApplicationDBContext _context;
        public CommentRespository(ApplicationDBContext context){
            _context = context;
        }

        public async Task<bool> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(item => item.Id == id);
            if(comment == null) 
                return false;
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(a => a.AppUser).ToListAsync();
        }

        public Task<Comment?> GetById(int id)
        {
            return _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var existingComment = await _context.Comments.FindAsync(id);
            if(existingComment == null)
                return null;
            existingComment.Title = comment.Title;
            existingComment.content = comment.content;
            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}