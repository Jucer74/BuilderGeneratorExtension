using People.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace People.Domain.Entities
{
   public class Person: EntityBase
   {
      public Person(string firstName, string lastName, DateTime dateOfBirth, char sex)
      {
         FirstName = firstName;
         LastName = lastName;
         DateOfBirth = dateOfBirth;
         Sex = sex;
      }

      public Person()
      {

      }

      [Required]
      public string FirstName { get; set; }

      [Required]
      public string LastName { get; set; }

      public DateTime DateOfBirth { get; set; }

      [Required]
      public char Sex { get; set; }
   }
}