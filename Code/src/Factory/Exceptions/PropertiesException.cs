using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BuilderGeneratorFactory.Exceptions
{
   [ExcludeFromCodeCoverage]
   [Serializable]
   public class PropertiesException : BusinessException
   {
      public PropertiesException()
      {
      }

      public PropertiesException(string message) : base(message)
      {
      }

      public PropertiesException(string message, Exception innerException)
          : base(message, innerException)
      {
      }

      // Without this constructor, deserialization will fail
      protected PropertiesException(SerializationInfo info, StreamingContext context)
          : base(info, context)
      {
      }
   }
}