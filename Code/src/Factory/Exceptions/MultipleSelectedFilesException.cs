using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BuilderGeneratorFactory.Exceptions
{
   [ExcludeFromCodeCoverage]
   [Serializable]
   public class MultipleSelectedFilesException : BusinessException
   {
      public MultipleSelectedFilesException()
      {
      }

      public MultipleSelectedFilesException(string message) : base(message)
      {
      }

      public MultipleSelectedFilesException(string message, Exception innerException)
          : base(message, innerException)
      {
      }

      // Without this constructor, deserialization will fail
      protected MultipleSelectedFilesException(SerializationInfo info, StreamingContext context)
          : base(info, context)
      {
      }
   }
}