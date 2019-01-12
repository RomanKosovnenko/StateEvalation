using StateEvaluation.Common.Constants;
using StateEvaluation.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace StateEvaluation.Repository.Providers
{
    public partial class DataRepository : DataContext
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
        public void UpdatePerson(People person)
        {
            var items = this.People.Where(item => item.Id == person.Id).Single<People>();
            items.Id = person.Id;
            items.Firstname = person.Firstname;
            items.Lastname = person.Lastname;
            items.Middlename = person.Middlename;
            items.Birthday = person.Birthday;
            items.Workposition = person.Workposition;
            items.Expedition = person.Expedition;
            items.Number = person.Number;
            items.UserId = person.UserId;
            SubmitChanges();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        public void CreatePerson(People person)
        {
            People.InsertOnSubmit(person);
            SubmitChanges();
        }

        /// <summary>
        /// Delete person
        /// </summary>
        /// <param name="id">personId</param>
        public void DeletePerson(string id)
        {
            var person = People.Single(item => item.Id.ToString() == id);
            People.DeleteOnSubmit(person);
            SubmitChanges();
        }

        /// <summary>
        /// Get all userIds
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetUserIds()
        {
            return People
                .Select(item => item.UserId)
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
        public IEnumerable<string> GetExpeditionCodesFilter()
        {
            return People
                .Select(item => item.Expedition.ToString())
                .Distinct()
                .OrderByDescending(item => Convert.ToInt32(item))
                .ToList();
        }

        /// <summary>
        /// Get people's numbers for filters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPeopleNumbersFilter()
        {
            return People
                .Select(item => item.Number.ToString())
                .Distinct()
                .OrderBy(item => Convert.ToInt32(item))
                .ToList();
        }
    }
}
