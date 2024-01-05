using Custodian.Claims.Dto.Response;
using Custodian.Claims.Models;

namespace Custodian.Claims.ClaimsProcessing
{
	public interface IClaimsRequestManager
	{
		ResponseBase<ClaimStatusResponse> ClaimRetrievalByClaimNumber(string claimNumber);
		ResponseBase<string> SaveClaimRequest(ClaimRequest claimRequest);
	}
}