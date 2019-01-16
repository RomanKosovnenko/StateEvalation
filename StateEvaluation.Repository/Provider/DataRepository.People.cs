using StateEvaluation.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StateEvaluation.Repository.Providers
{
    public partial class DataRepository : DbContext
    {
        /// <summary>
        /// Get person by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public People GetPerson(string id)
        {
            var person = People.Single(item => item.Id.ToString() == id);
            return person;
        }

        /// <summary>
        /// Get all people
        /// </summary>
        /// <returns></returns>
        public IEnumerable<People> GetPeople()
        {
            var people = this.People.Select(item => item).OrderByDescending(item => item.UserId);
            return people;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="person">newPerson</param>
        public void UpdatePerson(People newPerson)
        {
            var person = this.People.Single(item => item.Id == newPerson.Id);
            person.Id = newPerson.Id;
            person.Firstname = newPerson.Firstname;
            person.Lastname = newPerson.Lastname;
            person.Middlename = newPerson.Middlename;
            person.Birthday = newPerson.Birthday;
            person.Workposition = newPerson.Workposition;
            person.Expedition = newPerson.Expedition;
            person.Number = newPerson.Number;
            person.UserId = newPerson.UserId;
            this.Entry(person).State = EntityState.Modified;
            this.SaveChanges();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        public void CreatePerson(People person)
        {
            People.Add(person);
            SaveChanges();
        }

        /// <summary>
        /// Delete person
        /// </summary>
        /// <param name="id">personId</param>
        public void DeletePerson(string id)
        {
            var person = People.Single(item => item.Id.ToString() == id);
            People.Remove(person);
            SaveChanges();
        }

        /// <summary>
        /// Get all userIds
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetUserIds()
        {
            return People
                .Select(item => item.UserId.Trim())
                .Distinct()
                .OrderByDescending(item => item)
                .ToList();
        }

        /// <summary>
        /// Get peolpe by query
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        public IEnumerable<People> GetPeople(Func<People, bool> query)
        {
            return People.Where(query).OrderByDescending(item => item.UserId);
        }

        /// <summary>
        /// Get userIds for filters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetUserIdsFilter()
        {
            return GetUserIds().ToList();
        }

        /// <summary>
        /// Get professions of people for filters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetProfessionsFilter()
        {
            return People
                .Select(item => item.Workposition.Trim())
                .Distinct()
                .OrderBy(item => item)
                .ToList();
        }

        /// <summary>
        /// Get expedition codes of people for filters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetExpeditionCodesFilter()
        {
            return People
                .Select(item => item.Expedition)
                .Distinct()
                .OrderByDescending(item => Convert.ToInt32(item))
                .ToList();
        }

        /// <summary>
        /// Get people's numbers for filters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetPeopleNumbersFilter()
        {
            return People
                .Select(item => item.Number)
                .Distinct()
                .OrderBy(item => Convert.ToInt32(item))
                .ToList();
        }
    }
}
