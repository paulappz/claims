using Custodian.Claims.Dto.Response;
using Custodian.Claims.Exceptions;
using Custodian.Claims.Models;
using System.Net;

namespace Custodian.Claims.ClaimsProcessing
{
	public class ClaimsRequestManager : IClaimsRequestManager
	{
		private readonly ILogger<ClaimsRequestManager>? _logger;

		public ClaimsRequestManager(ILogger<ClaimsRequestManager>? logger)
		{
			_logger = logger;
		}

		private List<ClaimStatusResponse> _data = new List<ClaimStatusResponse>() 
		{
			new ClaimStatusResponse
			{
				ClaimNumber = "123456",
				Status = "Pending",
				PolicyNumber = "675432"
			},
			new ClaimStatusResponse
			{
				ClaimNumber = "678900",
				Status = "Approved",
				PolicyNumber = "098765"
			}

		};
		public ResponseBase<string> SaveClaimRequest(ClaimRequest claimRequest)
		{
			try
			{
				ClaimRequestValidator.ValidateClaimRequest(claimRequest);
				return ResponseBase<string>.Success(string.Empty);
			}
			catch (InputValidationException ex)
			{
				return ResponseBase<string>.Fail(ex.Message);
			}
		}

		public ResponseBase<ClaimStatusResponse> ClaimRetrievalByClaimNumber(string claimNumber)
		{
			try
			{
				ClaimRequestValidator.ValidateClaimRetrival(claimNumber);
				var response = _data.FirstOrDefault(c => c.ClaimNumber == claimNumber);
				if(response != null)
				{
					return ResponseBase<ClaimStatusResponse>.Success(response);
				}
				throw new ResourceNotFoundException();
			}
			catch (InputValidationException ex)
			{
				return ResponseBase<ClaimStatusResponse>.Fail(ex.Message);
			}
			catch(ResourceNotFoundException ex)
			{
				return ResponseBase<ClaimStatusResponse>.Fail(ex.Message,HttpStatusCode.NotFound);
			}
			catch(Exception ex)
			{
				_logger?.LogError(ex, ex.Source, ex.InnerException, ex.Message);
				return ResponseBase<ClaimStatusResponse>.Fail("Your request cannot be processed at the moment, Please try again later", HttpStatusCode.InternalServerError);
			}
		}
	}
}
