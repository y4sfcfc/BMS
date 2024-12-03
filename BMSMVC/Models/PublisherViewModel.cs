using BMS.DAL.Models;

namespace BMS.WEBUI.Models
{
	public class PublisherViewModel
	{
		public int Id { get; set; }
		public List<Book> Books { get; set; }
		public string Name { get; set; }
	}
}
