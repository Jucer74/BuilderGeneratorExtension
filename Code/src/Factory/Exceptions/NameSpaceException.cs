using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BuilderGeneratorFactory.Exceptions
{
   [ExcludeFromCodeCoverage]
   [Serializable]
   public class NameSpaceException : BusinessException
   {
      public NameSpaceException()
      {
      }

      public NameSpaceException(string message) : base(message)
      {
      }

      public NameSpaceException(string message, Exception innerException)
          : base(message, innerException)
      {
      }

      // Without this constructor, deserialization will fail
      protected NameSpaceException(SerializationInfo info, StreamingContext context)
          : base(info, context)
      {
      }
   }
}