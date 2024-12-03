using BMS.DAL.Models;

namespace BMS.WEBUI.Models
{
	public class AuthorViewModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Book>? Books { get; set; }
        public AuthorViewModel()
        {
            Books = new List<Book>();
        }
    }
}
