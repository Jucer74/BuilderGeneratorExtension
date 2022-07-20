using System.ComponentModel.DataAnnotations;

namespace People.Domain.Entities
{
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
}
