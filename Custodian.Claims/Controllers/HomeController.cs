using Custodian.Claims.ClaimsProcessing;
using Custodian.Claims.Models;
using Microsoft.AspNetCore.Mvc;

namespace Custodian.Claims.Controllers
{
	public class HomeController : Controller
	{
		private readonly IClaimsRequestManager _claimsRequestManager;
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, IClaimsRequestManager claimsRequestManager)
		{
			_logger = logger;
			_claimsRequestManager = claimsRequestManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost("api/ClaimRequest")]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> ClaimRequest([FromBody] ClaimRequest request)
		{
			await Task.Delay(5000);
			var response = _claimsRequestManager.SaveClaimRequest(request);
			return StatusCode(response.StatusCode, response);
		}


		[HttpGet("api/CheckClaimStatus")]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> CheckClaimStatus([FromQuery] string claimNumber)
		{
			await Task.Delay(5000);
			var response = _claimsRequestManager.ClaimRetrievalByClaimNumber(claimNumber);
			return StatusCode(response.StatusCode, response);
		}

	}
}