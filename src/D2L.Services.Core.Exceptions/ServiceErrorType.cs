namespace D2L.Services.Core.Exceptions {
	
	/// <summary>
	/// Indicates the reason that a
	/// <see cref="D2L.Services.Core.Exceptions.ServiceException">Service
	/// Exception</see> was thrown.
	/// </summary>
	public enum ServiceErrorType {
		/// <summary>
		/// A connection could not be established with the service or the
		/// connection was interrupted.
		/// <example>The target server is not running</example>
		/// <example>The connection was refused by the server.</example>
		/// </summary>
		ConnectionFailure,
		
		/// <summary>
		/// The connection to the service timed out.
		/// </summary>
		Timeout,
		
		/// <summary>
		/// The service did not respond with a status code indicating success
		/// <example>The service responded with 403 Forbidden</example>
		/// <example>The service responded with 500 Internal Server Error
		/// </example>
		/// </summary>
		ErrorResponse,
		
		/// <summary>
		/// The service responded with data that the client deemed invalid.
		/// <example>The service responded with a plain text string, but the
		/// client expected a JSON object</example>
		/// <example>The JSON data received from the service could be serialized
		/// to the desired object type.</example>
		/// <example>The service responded with a valid JSON object, but the
		/// data was not logically valid, such as a StartDate field having a
		/// date later than an EndDate field.</example>
		/// </summary>
		ClientError,
		
		/// <summary>
		/// The client, following the circuit breaker pattern, did not attempt
		/// to contact the service due to multiple recent timeouts or connection
		/// failures.
		/// </summary>
		CircuitOpen
	}
	
}
