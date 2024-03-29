﻿namespace BuilderGeneratorFactory.Define
{
   public enum BuilderPropertiesType
   {
      Fields,
      Variables
   }

   public static class BuilderConstants
   {
      public static readonly string UsingAutoFixture = "using AutoFixture;";
      public static readonly string FixtureVariable = "private readonly Fixture fixture;";
      public static readonly string AssignFixtureVariable = "fixture = new Fixture();";
      public static readonly string FixtureBehaviorsOfType = "fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));";
      public static readonly string FixtureBehaviorsAdd = "fixture.Behaviors.Add(new OmitOnRecursionBehavior());";
      public static readonly string NameSpace = "namespace";
      public static readonly string PropertyRegex = "(?>public)\\s+(?!class)((static|readonly)\\s)?(?<Type>(\\S+(?:<.+?>)?)(?=\\s+\\w+\\s*\\{\\s*get))\\s+(?<Name>[^\\s]+)(?=\\s*\\{\\s*get)";
      public static readonly string ClassRegex = "\\s+(class)\\s+(?<Name>[^\\s:]+)";
      public static readonly string OpenBrace = "{";
      public static readonly string CloseBrace = "}";
      public static readonly string ReturnThis = "return this;";
   }
}