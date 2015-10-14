using System.Net;
using System;

namespace D2L.Services.Core.Exceptions {
	/// <summary>
	/// Represents an error that occured when attempting to contact a service or
	/// an error caused by the service's response.
	/// </summary>
	public abstract class ServiceException : Exception {
		
		private ServiceErrorType m_errorType;
		private HttpStatusCode m_serviceStatusCode;
		private HttpStatusCode m_proposedStatusCode;
		
		/// <summary>
		/// Initialize the properties of <c>ServiceException</c> using the
		/// provided values.
		/// </summary>
		/// 
		/// <param name="errorType">
		/// The type of error that resulted in this exception.
		/// See <see cref="D2L.Services.Core.Exceptions.ServiceErrorType">
		/// ServiceErrorType</see>. If the error type is <c>ErrorResponse</c>,
		/// you must provide a value for <paramref name="serviceStatusCode" />.
		/// </param>
		/// <param name="proposedStatusCode">The suggested status code for the
		/// consumer to respond with. If this exception is uncaught, the
		/// consumer should use this status code instead of 500 Internal Server
		/// Error.</param>
		/// <param name="message">The error message that explains the reason for
		/// the exception.</param>
		/// <param name="innerException">The exception that is the cause of the
		/// current exception</param>
		/// <param name="serviceStatusCode">The status code received from the
		/// service. This value is only meaningful when
		/// <paramref name="errorType" /> is <c>ErrorResponse</c></param>
		/// 
		/// <exception cref="System.ArgumentException">An error type of
		/// <c>ErrorResponse</c> was given, but no value was provided for
		/// <paramref name="serviceStatusCode"/>.</exception>
		protected ServiceException(
			ServiceErrorType errorType,
			HttpStatusCode proposedStatusCode,
			string message = null,
			Exception innerException = null,
			HttpStatusCode serviceStatusCode = default( HttpStatusCode )
		) : base( message, innerException ) {
			
			if(
				errorType == ServiceErrorType.ErrorResponse &&
				serviceStatusCode == default( HttpStatusCode )
			) {
				throw new ArgumentException(
					"You must supply a value for the serviceStatusCode " +
					"parameter when errorType is ErrorResponse.",
					
					"serviceStatusCode"
				);
			}
			
			m_errorType = errorType;
			m_proposedStatusCode = proposedStatusCode;
			m_serviceStatusCode = serviceStatusCode;
		}
		
		/// <summary>
		/// Initialize the properties of <c>ServiceException</c> using the
		/// provided values.
		/// 
		/// The <c>ProposedStatusCode</c> property is automatically determined
		/// by <paramref name="errorType"/> and, if applicable,
		/// <paramref name="serviceStatusCode"/>.
		/// </summary>
		/// 
		/// <param name="errorType">
		/// The type of error that resulted in this exception.
		/// See <see cref="D2L.Services.Core.Exceptions.ServiceErrorType">
		/// ServiceErrorType</see>. If the error type is <c>ErrorResponse</c>,
		/// you must provide a value for <paramref name="serviceStatusCode" />.
		/// </param>
		/// <param name="message">The error message that explains the reason for
		/// the exception.</param>
		/// <param name="innerException">The exception that is the cause of the
		/// current exception</param>
		/// <param name="serviceStatusCode">The status code received from the
		/// service. This value is only meaningful when
		/// <paramref name="errorType" /> is <c>ErrorResponse</c></param>
		/// 
		/// <exception cref="System.ArgumentException">An error type of
		/// <c>ErrorResponse</c> was given, but no value was provided for
		/// <paramref name="serviceStatusCode"/>.</exception>
		protected ServiceException(
			ServiceErrorType errorType,
			string message = null,
			Exception innerException = null,
			HttpStatusCode serviceStatusCode = default( HttpStatusCode )
		) : base( message, innerException ) {
			
			if(
				errorType == ServiceErrorType.ErrorResponse &&
				serviceStatusCode == default( HttpStatusCode )
			) {
				throw new ArgumentException(
					"You must supply a value for the serviceStatusCode " +
					"parameter when errorType is ErrorResponse.",
					
					"serviceStatusCode"
				);
			}
			
			m_errorType = errorType;
			m_serviceStatusCode = serviceStatusCode;
			
			switch( errorType ) {
				case ServiceErrorType.ConnectionFailure:
				case ServiceErrorType.ClientError:
					m_proposedStatusCode = HttpStatusCode.BadGateway;
					break;
				case ServiceErrorType.Timeout:
					m_proposedStatusCode = HttpStatusCode.GatewayTimeout;
					break;
				case ServiceErrorType.CircuitOpen:
					m_proposedStatusCode = HttpStatusCode.ServiceUnavailable;
					break;
				case ServiceErrorType.ErrorResponse:
					if(
						(int)serviceStatusCode >= 400 &&
						serviceStatusCode != HttpStatusCode.InternalServerError
					) {
						m_proposedStatusCode = serviceStatusCode;
					} else {
						m_proposedStatusCode = HttpStatusCode.BadGateway;
					}
					break;
				default:
					m_proposedStatusCode = HttpStatusCode.InternalServerError;
					break;
			}
		}
		
		/// <summary>The name of the service being contacted.</summary>
		public abstract string ServiceName { get; }
		
		/// <summary>The type of error that resulted in this exception.
		/// </summary>
		/// <seealso cref="D2L.Services.Core.Exceptions.ServiceErrorType" />
		public ServiceErrorType ErrorType { get { return m_errorType; } }
		
		/// <summary>The suggested status code for the consumer to respond with.
		/// If this exception is uncaught, the consumer should use this status
		/// code instead of 500 Internal Server Error.</summary>
		public HttpStatusCode ProposedStatusCode {
			get { return m_proposedStatusCode; }
		}
		
		/// <summary>The status code received from the service. This value is
		/// only meaningful when <c>ErrorType</c> is <c>ErrorResponse</c>.
		/// </summary>
		public HttpStatusCode ServiceStatusCode {
			get { return m_serviceStatusCode; }
		}
		
	}
}