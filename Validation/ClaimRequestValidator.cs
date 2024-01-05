using Custodian.Claims.Exceptions;
using System.Text.RegularExpressions;

namespace Custodian.Claims.Models
{
	public static class ClaimRequestValidator
	{
		private const string _numbersOnly = @"^[0-9]*$";

		public static void ValidateClaimRequest(ClaimRequest claimRequest)
		{
			claimRequest?.PolicyNumber!.IsValidPolicyNumber();
			claimRequest?.EmailAddress!.IsValidEmailAddress();
			claimRequest?.PhoneNumber!.IsValidPhoneNumber();
			claimRequest?.Description!.IsValidDescription();
		}

		public static void ValidateClaimRetrival(string claimNumber)
		{
			IsRequired(claimNumber, "Claim Number");
			Regex regex = new Regex(_numbersOnly);
			if (!regex.IsMatch(claimNumber))
			{
				throw new InputValidationException($"Invalid claim number.");
			};
		}

		private static void IsValidPolicyNumber(this string policyNumber)
		{
			IsRequired(policyNumber, "Policy Number");
			Regex regex = new Regex(_numbersOnly);
			if (!regex.IsMatch(policyNumber))
			{
				throw new InputValidationException($"Invalid Policy Number.");
			};
		}

		private static void IsValidEmailAddress(this string email)
		{
			IsRequired(email, "Email address");
			Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
			if (!regex.IsMatch(email))
			{
				throw new InputValidationException($"Invalid email address.");
			};
		}

		private static void IsValidPhoneNumber(this string phonenumber)
		{
			IsRequired(phonenumber, "Phone number");
			Regex regex = new Regex(_numbersOnly);
			if (!regex.IsMatch(phonenumber))
			{
				throw new InputValidationException($"Invalid phone number.");
			};
		}

		private static void IsValidDescription(this string? description)
		{
			Regex regex = new Regex(@"^[a-zA-Z0-9\s.,!?]*$");

			if (!string.IsNullOrEmpty(description) && !regex.IsMatch(description!))
			{
				throw new InputValidationException($"Description can only contain alphanumeric characters and basic punctuation.");
			}
		}

		private static void IsRequired(this string? value, string name)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new InputValidationException($"{name} cannot be null or empty.");
			}
		}
	}
}
