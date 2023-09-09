using System.Collections.Generic;
using System.Runtime.InteropServices;
using RicModel.RoomRent;

namespace RicModel.CostMonitoring.Dtos
{
    public class GraphDataOutput
    {
        public List<TransactionCostSummaryOutput> Categories { get; set; }
        public List<ItemOutput> Items { get; set; }
    }

    public class TransactionCostSummaryOutput
    {

        public int CategoryId { get; set; }
        public string Category { get; set; }
        public decimal Total { get; set; }
        public int Count { get; set; }
        
        
    }

    public class ItemOutput
    {
        public string Name { get; set; }
        public List<decimal> Total { get; set; }
        public string BackgroundColor { get; set; }

    }

    public class TotalItem
    {
        public int ItemId { get; set; }
        public decimal Total { get; set; }
    }

    //output
    public class TransactionCostSummaryDto
    {
        public string CategoryAndTotal
        {
            get { return Category + " - " + Total.ToString("C"); }
        }

        public int CategoryId { get; set; }
        public string Category { get; set; }
        
        public decimal Total { get; set; }
        public int Count { get; set; }

        public List<Item> Items { get; set; }
    }
    
    public class TransactionCostGraphDto
    {
        public TransactionCostGraphDto()
        {
            Datasets = new DataSets { BackgroundColor = "rgba(53, 162, 235, 0.5)" };
        }

        public string Labels { get; set; }
        public DataSets Datasets { get; set; }
    }

    public class DataSets
    {
        public string Label { get; set; }
        public string Data { get; set; }
        public string BackgroundColor { get; set; }
    }

    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Total { get; set; }
        public int Count { get; set; }
        public string BackgroundColor { get; set; }
    }

}
