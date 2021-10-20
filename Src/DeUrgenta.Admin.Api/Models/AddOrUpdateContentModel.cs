namespace DeUrgenta.Admin.Api.Models
{
    public sealed record AddOrUpdateContentModel
    {
        public string Culture { get; set; }

        public string Key { get; set; }
        
        public string Value { get; set; }
        
        
        
        
    }
}