using System.ComponentModel.DataAnnotations.Schema;

namespace TheManXS.Model.Gameplay
{
    public enum VariableImpactedE { Cash, Debt, Expense, TheMan, Timeline, AllCommodityPrices, OneCommodityPrice, Total }
    public enum ApplicabilityE { ActivePlayer, AllPlayers, Operation, CommodityPricing, GeoLogical, Total }

    [Table("Clusters")]
    public class Cluster
    {
        public int ID { get; set; }
        public string Headline { get; set; }
        public VariableImpactedE VariableImpacted { get; set; }
        public ApplicabilityE Applicability { get; set; }
        public bool IsPositive { get; set; }
    }
}
