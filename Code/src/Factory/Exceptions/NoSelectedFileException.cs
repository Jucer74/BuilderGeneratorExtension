using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BuilderGeneratorFactory.Exceptions
{
   [ExcludeFromCodeCoverage]
   [Serializable]
   public class NoSelectedFileException : BusinessException
   {
      public NoSelectedFileException()
      {
      }

      public NoSelectedFileException(string message) : base(message)
      {
      }

      public NoSelectedFileException(string message, Exception innerException)
          : base(message, innerException)
      {
      }

      // Without this constructor, deserialization will fail
      protected NoSelectedFileException(SerializationInfo info, StreamingContext context)
          : base(info, context)
      {
      }
   }
}