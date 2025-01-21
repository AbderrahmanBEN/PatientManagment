using System.ComponentModel.DataAnnotations;

namespace MyApi.Models
{
    public class Patient
    {
        public required int Id { get; set; }
        public required string firstName { get; set; }
        public required string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        
    	[DataType(DataType.Date)]
    	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    	public DateTime dob { get; set; }
    }
}

