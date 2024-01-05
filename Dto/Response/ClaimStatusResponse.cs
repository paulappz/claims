namespace Custodian.Claims.Dto.Response
{
	public class ClaimStatusResponse
	{
		public string? ClaimNumber { get; set; }
		public string? PolicyNumber { get; set; }
		public string? Status { get; set; }
		public string? CreatedAt { get; set; } = DateTime.Now.ToString();
	}
}
