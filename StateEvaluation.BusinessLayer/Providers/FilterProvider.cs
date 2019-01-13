using StateEvaluation.Repository.Providers;
using StateEvaluation.Common.Helpers;
using StateEvaluation.Repository.Models;
using StateEvaluation.Common.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StateEvaluation.Common.Constants;

namespace StateEvaluation.BussinesLayer.Providers
{
    public class FilterProvider
    {
        private DataRepository _dataRepository;

        public FilterProvider(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public IEnumerable Filter(object filter)
        {
            if (filter.GetType().Name == typeof(PeopleFilterVM).Name)
            {
                return FilterPeople(filter);
            }
            if(filter.GetType().Name == typeof(PreferenceFilterVM).Name)
            {
                return FilterPreference(filter);
            }
            if (filter.GetType().Name == typeof(SubjectiveFeelingFilterVM).Name)
            {
                return FilterSubjectiveFeeling(filter);
            }

            return new string[0];
        }
        public void Clear(object filter, List<System.Windows.Controls.ListBox> listBoxes)
        {
            foreach (System.Windows.Controls.ListBox listBox in listBoxes)
            {
                listBox.UnselectAll();
            }
            Clear(filter);
        }
        public void Clear(object filter)
        {
            if (filter != null)
            {
                foreach (var i in filter.GetType().GetProperties())
                {
                    if (i.Name.Contains("Date"))
                    {
                        i.SetValue(filter, null);
                        continue;
                    }
                    if (i.Name.Contains("Color"))
                    {
                        i.SetValue(filter, string.Empty);
                        continue;
                    }
                    if (bool.TryParse(i.GetValue(filter).ToString(), out bool res))
                    {
                        i.SetValue(filter, false);
                        continue;
                    }
                    // i.SetValue(filter, string.Empty);
                }
            }
        }

        private IEnumerable<People> FilterPeople(object filter)
        {
            IEnumerable<People> people;
            if (filter != null)
            {
                PeopleFilterVM peopleFilter = (PeopleFilterVM)filter;

                DateTime dateTo = peopleFilter.DateTo != null ? DateTime.Parse(peopleFilter.DateTo.ToString()) : new DateTime();
                DateTime dateFrom = peopleFilter.DateFrom != null ? DateTime.Parse(peopleFilter.DateFrom.ToString()) : new DateTime();

                people = _dataRepository.GetPeople(GetBaseQuery(peopleFilter))
                    .Where(_ => (dateFrom.Ticks == 0 || DateTime.Parse(_.Birthday).Ticks >= dateFrom.Ticks)
                        && (dateTo.Ticks == 0 || DateTime.Parse(_.Birthday).Ticks <= dateTo.Ticks));

                return people;
            }
            people = _dataRepository.GetPeople();
            return people;
        }

        private IEnumerable<Preference> FilterPreference(object filter)
        {
            IEnumerable<Preference> preferences;
            if (filter != null)
            {
                PreferenceFilterVM preferenceFilter = (PreferenceFilterVM)filter;

                DateTime dateTo = preferenceFilter.DateTo != null ? DateTime.Parse(preferenceFilter.DateTo.ToString()) : new DateTime();
                DateTime dateFrom = preferenceFilter.DateFrom != null ? DateTime.Parse(preferenceFilter.DateFrom.ToString()) : new DateTime();

                //get people regarding person number and expedition
                List<string> allowedUserIds = new List<string>();
                var people = _dataRepository.GetPeople(GetBaseQuery(preferenceFilter));
                foreach (var person in people.ToList())
                {
                    allowedUserIds.Add(person.UserId.ToString().Trim());
                }
                
                preferences = _dataRepository.GetPreferenceTests(GetPreferenceQuery(preferenceFilter, dateTo, dateFrom, allowedUserIds.ToArray()));
                
                return preferences;
            }
            preferences = _dataRepository.GetPreferenceTests();
            return preferences;
        }

        private IEnumerable<SubjectiveFeeling> FilterSubjectiveFeeling(object filter)
        {
            IEnumerable<SubjectiveFeeling> subjectiveFeelings;
            if (filter != null)
            {
                var subjectiveFeelingFilter = (SubjectiveFeelingFilterVM)filter;

                DateTime dateTo = subjectiveFeelingFilter.DateTo != null ? DateTime.Parse(subjectiveFeelingFilter.DateTo.ToString()) : new DateTime();
                DateTime dateFrom = subjectiveFeelingFilter.DateFrom != null ? DateTime.Parse(subjectiveFeelingFilter.DateFrom.ToString()) : new DateTime();

                //get people regarding person number and expedition
                List<string> allowedUserIds = new List<string>();
                var people = _dataRepository.GetPeople(GetBaseQuery(subjectiveFeelingFilter));
                foreach (var person in people.ToList())
                {
                    allowedUserIds.Add(person.UserId.ToString().Trim());
                }
            
                subjectiveFeelings = _dataRepository.GetSubjecriveFeelings(GetSubjectiveFeelingQuery(subjectiveFeelingFilter, dateTo, dateFrom, allowedUserIds.ToArray()));
                return subjectiveFeelings;
            }
            subjectiveFeelings = _dataRepository.GetSubjecriveFeelings();
            return subjectiveFeelings;
        }

        private Func<People, bool> GetBaseQuery(object filter)
        {
            var peopleFilter = (BaseFilterVM)filter;
            return (People _) =>
                (peopleFilter.Professions.Count == 0 || peopleFilter.Professions.Contains(_.Workposition.Trim())) &&
                (peopleFilter.UserIds.Count == 0 ? (
                    (peopleFilter.Expeditions.Count == 0 || peopleFilter.Expeditions.Contains(_.Expedition.ToString())) &&
                    (peopleFilter.People.Count == 0 || peopleFilter.People.Contains(_.Number.ToString()))
                ) : peopleFilter.UserIds.Contains(_.UserId.Trim()));
        }

        private Func<Preference, bool> GetPreferenceQuery(object filter, DateTime dateTo, DateTime dateFrom, string[] allowedUserIds)
        {
            var preferenceFilter = (PreferenceFilterVM)filter;
            return (Preference _) => 
                (dateFrom.Ticks == 0 || _.Date.Ticks >= dateFrom.Ticks) && (dateTo.Ticks == 0 || _.Date.Ticks <= dateTo.Ticks) &&
                 OrderComparer.Compare(_.ShortOder1, preferenceFilter.Color1in3Filter, preferenceFilter.Color2in3Filter, preferenceFilter.Color3in3Filter) &&
                 OrderComparer.Compare(_.Oder1, preferenceFilter.Color1in12Filter, preferenceFilter.Color2in12Filter,
                    preferenceFilter.Color3in12Filter, preferenceFilter.Color4in12Filter, preferenceFilter.Color5in12Filter,
                    preferenceFilter.Color6in12Filter, preferenceFilter.Color7in12Filter, preferenceFilter.Color8in12Filter,
                    preferenceFilter.Color9in12Filter, preferenceFilter.Color10in12Filter, preferenceFilter.Color11in12Filter,
                    preferenceFilter.Color12in12Filter) &&
                (preferenceFilter.PreferenceFilter.Count() == 0 || preferenceFilter.PreferenceFilter.Contains(_.Preference1.Trim())) &&
                allowedUserIds.Contains(_.UserId.Trim());
        }

        private Func<SubjectiveFeeling, bool> GetSubjectiveFeelingQuery(object filter, DateTime dateTo, DateTime dateFrom, string[] allowedUserIds)
        {
            var subjectiveFeelingFilter = (SubjectiveFeelingFilterVM)filter;
            return (SubjectiveFeeling _) =>
                (dateFrom.Ticks == 0 || _.Date.Ticks >= dateFrom.Ticks) &&
                (dateTo.Ticks == 0 || _.Date.Ticks <= dateTo.Ticks) &&
                allowedUserIds.Contains(_.UserId.Trim()) &&
                (subjectiveFeelingFilter.IsFeeling ? (
                        (_.GeneralWeaknes == subjectiveFeelingFilter.GeneralWeakness) &&
                        (_.PoorAppetite == subjectiveFeelingFilter.PoorAppetite) &&
                        (_.PoorSleep == subjectiveFeelingFilter.PoorSleep) &&
                        (_.BadMood == subjectiveFeelingFilter.BadMood) &&
                        (_.HeavyHead == subjectiveFeelingFilter.HeavyHead) &&
                        (_.SlowThink == subjectiveFeelingFilter.SlowThink)
                    ) : true
                );
        }
    }
}
