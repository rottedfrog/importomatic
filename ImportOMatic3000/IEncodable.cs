using System.Text;

namespace ImportOMatic3000
{
    public interface IEncodable
    {
        Encoding Encoding { get; set; }
    }
}
