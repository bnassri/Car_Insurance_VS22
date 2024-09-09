namespace Car_Insurance_VS22.Models
{
	public class Insuree
	{
		public Guid Id { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required string EmailAddress { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public required string CarYear { get; set; }
		public required string CarMake { get; set; }
		public required string CarModel { get; set; }
		public required string DUI { get; set; }
		public required string SpeedingTickets { get; set; }
		public required string CoverageType { get; set; }

		public required double Quote { get; set; }
		
	}
}
