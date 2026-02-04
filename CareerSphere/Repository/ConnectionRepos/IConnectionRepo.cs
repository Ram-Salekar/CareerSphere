namespace CareerSphere.Repository.ConnectionRepos
{
    public interface IConnectionRepo
    {
        public Task<bool> SendConnectionRequestAsync(Guid senderId , Guid receiverID);

        public Task<bool> RemoveConnectionAsync(Guid senderId , Guid receiverID);
    }
}
