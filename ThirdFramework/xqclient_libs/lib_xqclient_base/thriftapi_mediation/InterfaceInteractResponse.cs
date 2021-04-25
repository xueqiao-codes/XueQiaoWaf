using System;
using lib.xqclient_base.thriftapi_mediation.Interface;
using System.Text;

namespace lib.xqclient_base.thriftapi_mediation
{
    public class InterfaceInteractResponse : IInterfaceInteractResponse
    {
        public string Servant { get; private set; }

        public string InterfaceName { get; private set; }

        public IInterfaceInteractInformation InteractInformation { get; set; }

        public Exception SourceException { get; private set; }

        public bool HasTransportException { get; private set; }

        public int? HttpResponseStatusCode { get; private set; }

        public IInterfaceCustomParsedExceptionResult CustomParsedExceptionResult { get; set; }

        public InterfaceInteractResponse(string servant, 
            string interfaceName, 
            Exception sourceException,
            bool hasTransportException,
            int? httpResponseStatusCode)
        {
            this.Servant = servant;
            this.InterfaceName = interfaceName;
            this.SourceException = sourceException;
            this.HasTransportException = hasTransportException;
            this.HttpResponseStatusCode = httpResponseStatusCode;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("InterfaceInteractResponse{");
            sb.Append($"interfaceName:{InterfaceName}");
            sb.Append($",servant:{Servant}");
            sb.Append($",HasTransportException:{HasTransportException}");
            sb.Append($",HttpResponseStatusCode:{HttpResponseStatusCode}");
            sb.Append($",BusinessErrorCode:{CustomParsedExceptionResult?.BusinessErrorCode}");
            sb.Append($",BusinessErrorCode:{CustomParsedExceptionResult?.BusinessErrorCode}");
            sb.Append($",BusinessErrorMessage:{CustomParsedExceptionResult?.BusinessErrorMessage}");
            sb.Append($",SessionInvalid:{CustomParsedExceptionResult?.SessionInvalid}");
            sb.Append($",SourceException:{SourceException?.ToString()}");
            sb.Append("}");
            return sb.ToString();
        }
    }

    public class InterfaceInteractResponse<T> : InterfaceInteractResponse, IInterfaceInteractResponse<T>
    {
        public T CorrectResult { get; private set; }

        public InterfaceInteractResponse(string servant, 
            string interfaceName, 
            Exception sourceException,
            bool hasTransportException,
            int? httpResponseStatusCode,
            T correctResult) : base(servant, interfaceName, sourceException, hasTransportException, httpResponseStatusCode)
        {
            this.CorrectResult = correctResult;
        }
    }
}
