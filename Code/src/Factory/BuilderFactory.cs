using BuilderGeneratorFactory.Define;
using BuilderGeneratorFactory.Dto;
using BuilderGeneratorFactory.Exceptions;
using BuilderGeneratorFactory.Extension;
using BuilderGeneratorFactory.Options;
using System.Collections.Generic;
using System.Linq;
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

         #region Get-Data

         string usings = GetUsings(fileContent);
         List<PropertyInfo> propertiesInfo = GetPropertiesInfo(fileContent);
         string nameSpace = GetNameSpace(fileContent);
         string className = GetClassName(fileContent);
         string variableClassName = className.ToCamelCase();
         string builderClassName = string.Format("{0}Builder", className);

         #endregion Get-Data

         GenerateUsings(builderContent, usings, builderOptions);

         GenerateNameSpace(builderContent, nameSpace);

         GenerateClass(builderContent, builderClassName);

         GenerateVariables(builderContent, propertiesInfo, className, variableClassName, builderOptions);

         GenerateBuilderConstructor(builderContent, builderClassName, className, variableClassName, builderOptions);

         GenerateFixtureMethods(builderContent, builderClassName, className, variableClassName, builderOptions);

         GenerateMethodsToSetValues(builderContent, builderClassName, variableClassName, propertiesInfo, builderOptions);

         GenerateMethodBuild(builderContent, className, variableClassName, propertiesInfo, builderOptions);

         builderContent.AppendLine(string.Format("\t{0}", BuilderConstants.CloseBrace));
         builderContent.AppendLine(BuilderConstants.CloseBrace);

         return builderContent.ToString();
      }

      private static void GenerateUsings(StringBuilder builderContent, string usings, BuilderOptions builderOptions)
      {
         if (builderOptions.EnableAutoFixture)
         {
            builderContent.AppendLine(BuilderConstants.UsingAutoFixture);
         }
         builderContent.Append(usings);
      }

      private static void GenerateNameSpace(StringBuilder builderContent, string nameSpace)
      {
         builderContent.Append(nameSpace);
         builderContent.AppendLine(BuilderConstants.OpenBrace);
      }

      private static void GenerateClass(StringBuilder builderContent, string builderClassName)
      {
         builderContent.AppendLine(string.Format("\tpublic class {0}", builderClassName));
         builderContent.AppendLine(string.Format("\t{0}", BuilderConstants.OpenBrace));
      }

      private static void GenerateVariables(StringBuilder builderContent, List<PropertyInfo> propertiesInfo, string className, string variableClassName, BuilderOptions builderOptions)
      {
         if (builderOptions.EnableAutoFixture)
         {
            builderContent.AppendLine(string.Format("\t\t{0}", BuilderConstants.FixtureVariable));
         }

         if (builderOptions.PropertiesType == BuilderPropertiesType.Variables)
         {
            GeneratePrivateVariables(builderContent, propertiesInfo);
         }
         else
         {
            GeneratePrivateClassVariable(builderContent, className, variableClassName);
         }
      }

      private static void GeneratePrivateVariables(StringBuilder content, List<PropertyInfo> properties)
      {
         foreach (PropertyInfo property in properties)
         {
            content.AppendLine(string.Format("\t\tprivate {0} _{1};", property.Type, property.CamelCaseName));
         }
      }

      private static void GeneratePrivateClassVariable(StringBuilder content, string className, string variableClassName)
      {
         content.AppendLine(string.Format("\t\tprivate {0} {1};", className, variableClassName));
      }

      private static void GenerateBuilderConstructor(StringBuilder content, string builderClassName, string className, string variableClassName, BuilderOptions builderOptions)
      {
         content.AppendLine();
         content.AppendLine(string.Format("\t\tpublic {0}()", builderClassName));
         content.AppendLine(string.Format("\t\t{0}", BuilderConstants.OpenBrace));

         if (builderOptions.PropertiesType == BuilderPropertiesType.Fields)
         {
            content.AppendLine(string.Format("\t\t\t{0} = new {1}();", variableClassName, className));
         }

         if (builderOptions.EnableAutoFixture)
         {
            content.AppendLine(string.Format("\t\t\t{0}", BuilderConstants.AssignFixtureVariable));
            content.AppendLine(string.Format("\t\t\t{0}", BuilderConstants.FixtureBehaviorsOfType));
            content.AppendLine(string.Format("\t\t\t{0}", BuilderConstants.FixtureBehaviorsAdd));
         }

         content.AppendLine(string.Format("\t\t{0}", BuilderConstants.CloseBrace));
         content.AppendLine();
      }

      private static void GenerateFixtureMethods(StringBuilder builderContent, string builderClassName, string className, string variableClassName, BuilderOptions builderOptions)
      {
         if (builderOptions.EnableAutoFixture && builderOptions.PropertiesType == BuilderPropertiesType.Fields)
         {
            GenerateSetFixtureData(builderContent, builderClassName, className, variableClassName);
            GeneratGetFixtureDataList(builderContent, className);
         }
      }

      private static void GenerateSetFixtureData(StringBuilder content, string builderClassName, string className, string variableClassName)
      {
         content.AppendLine();
         content.AppendLine(string.Format("\t\tpublic {0} SetFixtureData()", builderClassName));
         content.AppendLine(string.Format("\t\t{0}", BuilderConstants.OpenBrace));

         content.AppendLine(string.Format("\t\t\tthis.{0} = fixture.Create<{1}>();", variableClassName, className));
         content.AppendLine(string.Format("\t\t\t{0}", BuilderConstants.ReturnThis));

         content.AppendLine(string.Format("\t\t{0}", BuilderConstants.CloseBrace));
         content.AppendLine();
      }

      private static void GeneratGetFixtureDataList(StringBuilder content, string className)
      {
         content.AppendLine();
         content.AppendLine(string.Format("\t\tpublic List<{0}> GetFixtureDataList(int quantity)", className));
         content.AppendLine(string.Format("\t\t{0}", BuilderConstants.OpenBrace));

         content.AppendLine(string.Format("\t\t\treturn fixture.Build<{0}>().CreateMany(quantity).ToList();", className));

         content.AppendLine(string.Format("\t\t{0}", BuilderConstants.CloseBrace));

         content.AppendLine();
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
         int indexOfNameSpace = fileContent.IndexOf(BuilderConstants.NameSpace);
         int indexOfOpenBrace = fileContent.IndexOf(BuilderConstants.OpenBrace);
         int nameSpaceLength = indexOfOpenBrace - indexOfNameSpace;
         return fileContent.Substring(indexOfNameSpace, nameSpaceLength);
      }

      private static string GetClassName(string fileContent)
      {
         return Regex.Match(fileContent, BuilderConstants.ClassRegex).Groups["Name"].Value;
      }

      private static void GenerateMethodsToSetValues(StringBuilder content, string builderClassName, string variableClassName, List<PropertyInfo> properties, BuilderOptions builderOptions)
      {
         string camelCaseName = string.Empty;

         foreach (PropertyInfo property in properties)
         {
            camelCaseName = property.Name.ToCamelCase();

            content.AppendLine(string.Format("\t\tpublic {0} With{1}({2} {3})", builderClassName, property.Name, property.Type, camelCaseName));
            content.AppendLine(string.Format("\t\t{0}", BuilderConstants.OpenBrace));

            if (builderOptions.PropertiesType == BuilderPropertiesType.Variables)
            {
               content.AppendLine(string.Format("\t\t\t_{0} = {1};", camelCaseName, camelCaseName));
            }
            else
            {
               content.AppendLine(string.Format("\t\t\tthis.{0}.{1} = {2};", variableClassName, property.Name, camelCaseName));
            }

            content.AppendLine(string.Format("\t\t\t{0}", BuilderConstants.ReturnThis));

            content.AppendLine(string.Format("\t\t{0}", BuilderConstants.CloseBrace));
            content.AppendLine();
         }
      }

      private static void GenerateMethodBuild(StringBuilder content, string className, string variableClassName, List<PropertyInfo> properties, BuilderOptions builderOptions)
      {
         content.AppendLine("\t\tpublic " + className + " Build()");
         content.AppendLine(string.Format("\t\t{0}", BuilderConstants.OpenBrace));

         if (builderOptions.PropertiesType == BuilderPropertiesType.Variables)
         {
            content.AppendLine("\t\t\treturn new " + className);
            content.AppendLine(string.Format("\t\t\t{0}", BuilderConstants.OpenBrace));

            string settingProperty = string.Empty;

            for (int index = 0; index < properties.Count; ++index)
            {
               settingProperty = string.Format("\t\t\t\t{0} = _{1}{2}", properties[index].Name, properties[index].Name.ToCamelCase(), (index + 1 < properties.Count) ? "," : string.Empty);
               content.AppendLine(settingProperty);
            }
            content.AppendLine(string.Format("\t\t\t{0};", BuilderConstants.CloseBrace));
         }
         else
         {
            content.AppendLine(string.Format("\t\t\t return this.{0};", variableClassName));
         }

         content.AppendLine(string.Format("\t\t{0}", BuilderConstants.CloseBrace));
      }
   }
}