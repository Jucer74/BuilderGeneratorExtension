using System.ComponentModel.DataAnnotations;

namespace People.Domain.Entities
{
	public class PersonBuilder
	{
		private Person person;

		public PersonBuilder()
		{
			person = new Person();
		}

		public PersonBuilder WithFirstName(string firstName)
		{
			this.person.FirstName = firstName;
			return this;
		}

		public PersonBuilder WithLastName(string lastName)
		{
			this.person.LastName = lastName;
			return this;
		}

		public PersonBuilder WithDateOfBirth(DateTime dateOfBirth)
		{
			this.person.DateOfBirth = dateOfBirth;
			return this;
		}

		public PersonBuilder WithSex(char sex)
		{
			this.person.Sex = sex;
			return this;
		}

		public Person Build()
		{
			 return this.person;
		}
	}
}
