namespace DRS.Data.BusinessEntities
{
    public partial class ProductGroupWeightingHelpingTableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ProductGroupWeightingTable { get; set; }
    }
}
