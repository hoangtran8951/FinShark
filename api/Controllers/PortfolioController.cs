using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRespository _stockRepo;
        private readonly IPortfolioRespository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRespository stockRepo, IPortfolioRespository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetuserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetuserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var stockModel = await _stockRepo.GetBySymbol(symbol);
            if(appUser == null || stockModel == null)
                return BadRequest("This User/Stock does not exist");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Can not Add the same Stock to portfolio");
            
            var portfolio = new Portfolio{
                AppUserId = appUser.Id,
                StockId = stockModel.Id
            };
            await _portfolioRepo.AddPortfolioAsync(portfolio);
            return Ok();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string symbol)
        {
            var username = User.GetuserName();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            
            var filteredStock = userPortfolio.Where(e => e.Symbol.ToLower() == symbol.ToLower()).ToList();
            if(filteredStock.Count == 1)
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            else   
                return BadRequest("Stock not in your portfolio");
            
            return Ok();
        }
    }
}