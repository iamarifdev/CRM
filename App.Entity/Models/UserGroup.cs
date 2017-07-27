using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.Design;

namespace App.Entity.Models
{
    public class UserGroup
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public virtual User User { get; set; }
    }
}