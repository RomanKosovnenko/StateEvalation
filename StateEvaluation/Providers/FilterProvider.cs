using StateEvaluation.Helpers;
using StateEvaluation.Model;
using StateEvaluation.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace StateEvaluation.Providers
{
    public class FilterProvider
    {
        private PreferenceDB _preferenceDb = new PreferenceDB();

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

        public void Clear(object filter)
        {
            if (filter != null)
            {
                foreach (var i in filter.GetType().GetProperties())
                {
                    if (i.Name.Contains("UserId") || i.Name.Contains("Expedition") || i.Name.Contains("Preference") || i.Name.Contains("People") || i.Name.Contains("Profession"))
                    {
                        i.SetValue(filter, "All");
                        continue;
                    }
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
                    i.SetValue(filter, string.Empty);
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

                people = _preferenceDb.GetPeople(GetPeopleQuery(peopleFilter, dateFrom, dateTo));

                return people;
            }
            people = _preferenceDb.GetPeople();
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
                var people = _preferenceDb.GetPeople(GetBaseQuery(preferenceFilter, dateFrom, dateTo));
                foreach (var person in people.ToList())
                {
                    allowedUserIds.Add(person.UserId.ToString().Trim());
                }
                
                preferences = _preferenceDb.GetPreferences(GetPreferenceQuery(preferenceFilter, dateTo, dateFrom, allowedUserIds.ToArray()));
                
                return preferences;
            }
            preferences = _preferenceDb.GetPreferences();
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
                var people = _preferenceDb.GetPeople(GetBaseQuery(subjectiveFeelingFilter, dateFrom, dateTo));
                foreach (var person in people.ToList())
                {
                    allowedUserIds.Add(person.UserId.ToString().Trim());
                }
            
                subjectiveFeelings = _preferenceDb.GetSubjecriveFeelings(GetSubjectiveFeelingQuery(subjectiveFeelingFilter, dateTo, dateFrom, allowedUserIds.ToArray()));
                return subjectiveFeelings;
            }
            subjectiveFeelings = _preferenceDb.GetSubjecriveFeelings();
            return subjectiveFeelings;
        }

        private Func<People, bool> GetPeopleQuery(object filter, DateTime dateFrom, DateTime dateTo)
        {
            return GetBaseQuery(filter, dateFrom, dateTo);
        }
        private Func<People, bool> GetBaseQuery(object filter, DateTime dateFrom, DateTime dateTo)
        {
            var peopleFilter = (BaseFilterVM)filter;

            return (People _) =>
                (_.Workposition == peopleFilter.Profession || peopleFilter.Profession == "All")
                && (peopleFilter.UserId == "All"
                    ? (peopleFilter.PeopleFrom == "All" || _.Number >= int.Parse(peopleFilter.PeopleFrom)) && (peopleFilter.PeopleTo == "All" || _.Number <= int.Parse(peopleFilter.PeopleTo))
                    : _.UserId == peopleFilter.UserId)
                && (peopleFilter.UserId == "All"
                    ? (peopleFilter.ExpeditionFrom == "All" || _.Expedition >= int.Parse(peopleFilter.ExpeditionFrom)) && (peopleFilter.ExpeditionTo == "All" || _.Expedition <= int.Parse(peopleFilter.ExpeditionTo))
                    : _.UserId == peopleFilter.UserId)
                && (dateFrom.Ticks == 0 || DateTime.Parse(_.Birthday).Ticks >= dateFrom.Ticks)
                && (dateTo.Ticks == 0 || DateTime.Parse(_.Birthday).Ticks <= dateTo.Ticks);
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
                (preferenceFilter.PreferenceFilter == "All" || _.Preference1 == preferenceFilter.PreferenceFilter) &&
                allowedUserIds.Contains(_.UserId.Trim());
        }

        private Func<SubjectiveFeeling, bool> GetSubjectiveFeelingQuery(object filter, DateTime dateTo, DateTime dateFrom, string[] allowedUserIds)
        {
            var subjectiveFeelingFilter = (SubjectiveFeelingFilterVM)filter;
            return (SubjectiveFeeling _) =>
                (dateFrom.Ticks == 0 || _.Date.Ticks >= dateFrom.Ticks) &&
                (dateTo.Ticks == 0 || _.Date.Ticks <= dateTo.Ticks) && 
                allowedUserIds.Contains(_.UserId.Trim()) &&
                (!subjectiveFeelingFilter.IsFeeling ? true : 
                    ((subjectiveFeelingFilter.GeneralWeakness ? _.GeneralWeaknes == subjectiveFeelingFilter.GeneralWeakness : true) &&
                    (subjectiveFeelingFilter.PoorAppetite ? _.PoorAppetite == subjectiveFeelingFilter.PoorAppetite : true) &&
                    (subjectiveFeelingFilter.PoorSleep ? _.PoorSleep == subjectiveFeelingFilter.PoorSleep : true) &&
                    (subjectiveFeelingFilter.BadMood ? _.BadMood == subjectiveFeelingFilter.BadMood : true) &&
                    (subjectiveFeelingFilter.HeavyHead ? _.HeavyHead == subjectiveFeelingFilter.HeavyHead : true) &&
                    (subjectiveFeelingFilter.SlowThink ? _.SlowThink == subjectiveFeelingFilter.SlowThink : true))
                );
        }
    }
}
