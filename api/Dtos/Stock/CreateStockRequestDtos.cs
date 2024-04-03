using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDtos
    {
        [Required]
        [MaxLength(10, ErrorMessage = "The Symbol cannot be over 10 characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(50, ErrorMessage = "The Company Name cannot be over 50 characters")] // current longest company name in the stock market has 46 characters
        public string CompanyName { get; set; }= string.Empty;
        [Required]
        [Range(1, 1000000000000)]
        public decimal Purchase { get; set; }       
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "The Industry Name cannot be over 30 characters")]
        public string Industry { get; set; } = string.Empty;
        [Range(1, 5000000000000)]
        public long MarketCap {get; set; }
    }
}