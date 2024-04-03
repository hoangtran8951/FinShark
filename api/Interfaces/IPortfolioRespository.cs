using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRespository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser appUser);
        Task<bool> AddPortfolioAsync(Portfolio portfolio);
        Task<Portfolio?> DeletePortfolio(AppUser appUser, string symbol);
    }
}