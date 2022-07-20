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
To perform the build process for a concrete object, we need a builder. By convention, the builder class is named **xxxBuilder**, and it has a public method **Build()** that returns a concrete object. For example, we have a **Person**, and then we create an **PersonBuilder** class like the code snippet below.

```CSharp
public class PersonBuilder
{
   private string _firstName;
   private string _lastName;
   private DateTime _dateOfBirth;
   private char _sex;

   public PersonBuilder()
   {
   }

   public PersonBuilder WithFirstName(string firstName)
   {
      _firstName = firstName;
      return this;
   }

   public PersonBuilder WithLastName(string lastName)
   {
      _lastName = lastName;
      return this;
   }

   public PersonBuilder WithDateOfBirth(DateTime dateOfBirth)
   {
      _dateOfBirth = dateOfBirth;
      return this;
   }

   public PersonBuilder WithSex(char sex)
   {
      _sex = sex;
      return this;
   }

   public Person Build()
   {
      return new Person
      {
         FirstName = _firstName,
         LastName = _lastName,
         DateOfBirth = _dateOfBirth,
         Sex = _sex
      };
   }
}
```

# Usage
This extension allows to create automacally the **Builder** class from the entity.

## Steps
1. Select the entity file and press right click


2. Select the option **Run Builder Generator** option

