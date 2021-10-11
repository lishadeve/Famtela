﻿namespace Famtela.Application.Features.Chicks.Queries.GetById
{
    public class GetChickByIdResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Transport { get; set; }
    }
}