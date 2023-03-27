namespace Test.Domain.Entities
{
    public class SiteStatitics : BaseEntity
    {
        public int Users { get; set; } = 0;
        public int Reviews { get; set; } = 0;
        public int OpenCases { get; set; } = 0;
        public int WithdrawnItems { get; set; } = 0;
        public int WithdrawnFunds { get; set; } = 0;
    }
}
