﻿namespace StockService.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Company Company { get; set; }
    }
}
