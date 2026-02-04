using CareerSphere.Models.UserTableModel;

namespace CareerSphere.Models.ConnectionTableModel
{
   
    public class Connection
    {
        public Guid followingId { get; set; }
        public Guid followerId { get; set; }

        public User following { get; set; }
        public User follower { get; set; }
    }
}
