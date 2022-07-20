using AutoFixture;
using System.ComponentModel.DataAnnotations;

namespace People.Domain.Entities
{
	public class PersonBuilder
	{
		private readonly Fixture fixture;
		private Person person;

		public PersonBuilder()
		{
			person = new Person();
			fixture = new Fixture();
			fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
			fixture.Behaviors.Add(new OmitOnRecursionBehavior());
		}


		public PersonBuilder SetFixtureData()
		{
			this.person = fixture.Create<Person>();
			return this;
		}


		public List<Person> GetFixtureDataList(int quantity)
		{
			return fixture.Build<Person>().CreateMany(quantity).ToList();
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
