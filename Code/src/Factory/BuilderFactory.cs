using BuilderGeneratorFactory.Define;
using BuilderGeneratorFactory.Dto;
using BuilderGeneratorFactory.Exceptions;
using BuilderGeneratorFactory.Options;
using System.Text;
using System.Text.RegularExpressions;

namespace BuilderGeneratorFactory
{
   public static class BuilderFactory
   {
      public static string Generate(string fileContent, BuilderOptions builderOptions)
      {
         if (!IsValidNameSpace(fileContent))
         {
            throw new NameSpaceException("NameSpace not Found");
         }

         StringBuilder builderContent = new StringBuilder();

         string usings = GetUsings(fileContent);

         builderContent.Append(usings);

         fileContent = fileContent.Substring(usings.Length);

         var propertiesInfo = GetPropertiesInfo(fileContent);

         var nameSpace = GetNameSpace(fileContent);

         builderContent.Append(nameSpace);
         builderContent.AppendLine("{");

         string className = GetClassName(fileContent);
         string builderClassName = string.Format("{0}Builder", className);

         builderContent.AppendLine(string.Format("\tpublic class {0}", builderClassName));
         builderContent.AppendLine(string.Format("\t{0}", BuilderConstants.OpenBrace));

         if (builderOptions.PropertiesType == BuilderPropertiesType.Variables)
         {
            GeneratePrivateVariables(builderContent, propertiesInfo);
         }
         else
         {
         }

         return builderContent.ToString();
      }

      private static bool IsValidNameSpace(string fileContent)
      {
         return fileContent.IndexOf(BuilderConstants.NameSpace) >= 0;
      }

      private static string GetUsings(string fileContent)
      {
         return fileContent.Substring(0, fileContent.IndexOf(BuilderConstants.NameSpace));
      }

      private static List<PropertyInfo> GetPropertiesInfo(string fileContent)
      {
         List<PropertyInfo> propertiesInfo = new List<PropertyInfo>();

         foreach (Match match in Regex.Matches(fileContent, BuilderConstants.PropertyRegex))
         {
            propertiesInfo.Add(new PropertyInfo(match.Groups["Type"].Value, match.Groups["Name"].Value));
         }

         if (!propertiesInfo.Any<PropertyInfo>())
         {
            throw new PropertiesException("Not Properties Found.");
         }

         return propertiesInfo;
      }

      private static string GetNameSpace(string fileContent)
      {
         return fileContent.Substring(fileContent.IndexOf(BuilderConstants.NameSpace), fileContent.IndexOf(BuilderConstants.OpenBrace));
      }

      private static string GetClassName(string fileContent)
      {
         return Regex.Match(fileContent, BuilderConstants.ClassRegex).Groups["Name"].Value;
      }

      private static void GeneratePrivateVariables(StringBuilder content, List<PropertyInfo> properties)
      {
         foreach (PropertyInfo property in properties)
         {
            content.AppendLine(string.Format("\t\tprivate {0} _{1};", property.Type, property.CamelCaseName));
         }
      }
   }
}