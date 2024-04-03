using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRespository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<bool>CreateAsync(Stock stock);
        Task<bool> DeleteAsync(int id);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDtos updateStockDot);
        Task<Stock?> GetBySymbol(string Symbol);
    }
}