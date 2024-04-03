using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRespository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetById(int id);
        Task<bool> CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(int id, Comment comment);
        Task<bool> DeleteAsync(int id);
    }
}