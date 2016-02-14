using System;
using System.Net;
namespace D2L.Services.Core.Exceptions.TestUtilities {
	internal sealed class StubServiceException : ServiceException {
		public StubServiceException(
			ServiceErrorType errorType,
			string message = null,
			Exception innerException = null,
			HttpStatusCode serviceStatusCode = default( HttpStatusCode )
		) : base( errorType, message, innerException, serviceStatusCode ) {}

		public override string ServiceName {
			get { return "foobar"; }
		}
	}
}
