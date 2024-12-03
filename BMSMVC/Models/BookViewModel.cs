using BMS.DAL.Models;

namespace BMS.WEBUI.Models
{
	public class BookViewModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
        public string AuthorFullName { get; set; }
        public string PublisherName { get; set; }
        public BookViewModel()
        {
             Categories = new List<Category>();
        }
    }
}
