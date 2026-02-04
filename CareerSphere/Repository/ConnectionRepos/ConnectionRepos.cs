using CareerSphere.Data;
using CareerSphere.Models.ConnectionTableModel;
using Microsoft.EntityFrameworkCore;

namespace CareerSphere.Repository.ConnectionRepos
{
    public class ConnectionRepos : IConnectionRepo
    {
        private readonly AppDbContext _dbContext;

        public ConnectionRepos(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SendConnectionRequestAsync(Guid senderId, Guid receiverId)
        {
            if (senderId == receiverId)
            {
                return false;
            }

            bool exists = await _dbContext.Connections.AnyAsync(c =>
            c.followerId == senderId &&
            c.followingId == receiverId
            );

            if (exists)
            {
                return false;
            }
            Connection connection = new Connection();
            connection.followerId = senderId;
            connection.followingId = receiverId;
            _dbContext.Connections.Add(connection);
            await _dbContext.SaveChangesAsync();



            return true;

        }

        public async Task<bool> RemoveConnectionAsync(Guid senderId, Guid receiverID)
        {
            if (senderId == receiverID)
            {
                return false;
            }

            var connection = await _dbContext.Connections.FirstOrDefaultAsync(c =>
            c.followerId == senderId &&
            c.followingId == receiverID
            );

           
            if (connection == null)
                return false;

          
            _dbContext.Connections.Remove(connection);
            await _dbContext.SaveChangesAsync();
            return true;


        }
    }
}
