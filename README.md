# Builder Generator Extension
This is a Visual Studio Extension, that allows generating a class that implements the Builder Pattern from a Model, Dto, or Entity Class.

# Overview
The Builder design pattern simplifies the process of creating a complex object separating the construction from its representation allows you to produce different types and representations of an object using the same construction code.

The models, Dtos and entities have the following structure, for example:

```csharp
public class Person
{
   [Required]
   public string FirstName { get; set; }

   [Required]
   public string LastName { get; set; }

   public DateTime DateOfBirth { get; set; }

   [Required]
   public char Sex { get; set; }
}
```

To perform the build process for a concrete object, we need a builder. By convention, the builder class is named as **xxxBuilder**, and it has a public method **Build()** that returns a concrete object. For example, we have an Person class (the definition is shown below), and we want to build test Address objects.




