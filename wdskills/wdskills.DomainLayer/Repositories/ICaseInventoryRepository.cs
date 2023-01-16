namespace wdskills.DomainLayer.Repositories
{
    public interface ICaseInventoryRepository
    {
        public Task<bool> ChangeLoss(Guid id, decimal loss);
    }
}
