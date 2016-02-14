using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using D2L.Services.Core.Exceptions.TestUtilities;
using NUnit.Framework;

namespace D2L.Services.Core.Exceptions {
	[TestFixture]
    public class ServiceExceptionTests {
		[Test]
		public void ServiceException_UnknownErrorType_Proposes500() {
			ServiceException exception = new StubServiceException(
				errorType: ( ServiceErrorType )(-123)
			);

			Assert.AreEqual( HttpStatusCode.InternalServerError, exception.ProposedStatusCode );
		}

		[Test]
		public void ServiceException_HttpGreaterThan500Response_ProposesSame() {
			ServiceException exception = new StubServiceException(
				errorType: ServiceErrorType.ErrorResponse,
				serviceStatusCode: HttpStatusCode.GatewayTimeout
			);

			Assert.AreEqual( HttpStatusCode.GatewayTimeout, exception.ProposedStatusCode );
		}

		[Test]
		public void ServiceException_Http500Response_Proposes502() {
			ServiceException exception = new StubServiceException(
				errorType: ServiceErrorType.ErrorResponse,
				serviceStatusCode: HttpStatusCode.InternalServerError
			);

			Assert.AreEqual( HttpStatusCode.BadGateway, exception.ProposedStatusCode );
		}

		[Test]
		public void ServiceException_Http302Response_Proposes502() {
			ServiceException exception = new StubServiceException(
				errorType: ServiceErrorType.ErrorResponse,
				serviceStatusCode: HttpStatusCode.Found
			);

			Assert.AreEqual( HttpStatusCode.BadGateway, exception.ProposedStatusCode );
		}
    }
}
