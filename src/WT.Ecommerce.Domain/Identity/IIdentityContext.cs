using System.Collections.Generic;
using System.Security.Claims;

namespace WT.Ecommerce.Domain.Identity
{
    /// <summary>
	/// Represents the identity in context.
	/// </summary>
    public interface IIdentityContext
    {
		/// <summary>
		/// Gets the identifier of the Principal
		/// </summary>
		string UserCode { get; }

		/// <summary>
		/// Gets the full name of the user
		/// </summary>
		string UserFullName { get; }

		/// <summary>
		/// Gets the collection of <see cref="Claim"/> associated with the Principal
		/// </summary>
		IEnumerable<Claim> Claims { get; }

	}
}
