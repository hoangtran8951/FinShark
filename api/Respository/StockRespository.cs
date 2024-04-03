using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Respository
{
    public class StockRespository : IStockRespository
    {
        private readonly ApplicationDBContext _context;
         public StockRespository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var stock = await GetByIdAsync(id);
            if(stock == null)
                return false;
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();
            if(!string.IsNullOrWhiteSpace(query.CompanyName))
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            
            if(!string.IsNullOrWhiteSpace(query.Symbol))
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            
            if(!string.IsNullOrWhiteSpace(query.SortBy)){
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }
            var skipNumber = (query.PageNumber -1) * query.PageSize;


            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public Task<Stock?> GetByIdAsync(int id)
        {
            return _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).FirstOrDefaultAsync(item => item.Id == id);
        }

        public Task<Stock?> GetBySymbol(string Symbol)
        {
            return _context.Stocks.FirstOrDefaultAsync(item => item.Symbol == Symbol);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDtos updateStockDot)
        {
            var stock = await GetByIdAsync(id);
            if(stock == null)
                return null;

            stock.CompanyName = updateStockDot.CompanyName;
            stock.Symbol = updateStockDot.Symbol;
            stock.Purchase = updateStockDot.Purchase;
            stock.LastDiv = updateStockDot.LastDiv;
            stock.Industry = updateStockDot.Industry;
            stock.MarketCap = updateStockDot.MarketCap;
            await _context.SaveChangesAsync();
            return stock;
        }
    }
}