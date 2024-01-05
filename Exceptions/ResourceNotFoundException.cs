namespace Custodian.Claims.Exceptions
{
	public class ResourceNotFoundException : Exception
	{
        public ResourceNotFoundException(): base($"No record found with this claim number")
        {
            
        }
    }
}
