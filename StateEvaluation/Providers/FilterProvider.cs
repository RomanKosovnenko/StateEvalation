using StateEvaluation.Helpers;
using StateEvaluation.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StateEvaluation.Providers
{
    public class FilterProvider
    {
        private PreferenceDB _preferenceDb = new PreferenceDB();

        public IEnumerable Filter(DataGrid dataGrid, object filter, int tabControl)
        {
            switch(tabControl)
            {
                case Enums.TabControl.People:
                    return FilterPeople(dataGrid, filter);
                case Enums.TabControl.Preferences:
                    return FilterPreference(dataGrid, filter);
                case Enums.TabControl.SubjectiveFeeling:
                    return FilterSubjectiveFeeling(dataGrid, filter);
                default:
                    return new List<object>();
            }
        }

        public void Clear(object filter)
        {
            if(filter != null)
            {
                foreach (var i in filter.GetType().GetProperties())
                {
                    if (i.Name.Contains("UserIds") || i.Name.Contains("Expedition") || i.Name.Contains("People") || i.Name.Contains("Profession"))
                    {
                        i.SetValue(filter, "All");
                        continue;
                    }
                    if (i.GetType() == typeof(bool))
                    {
                        i.SetValue(filter, false);
                        continue;
                    }
                    if (i.Name.Contains("Date"))
                    {
                        i.SetValue(filter, new DateTime());
                        continue;
                    }
                    i.SetValue(filter, string.Empty);
                }
            }
        }

        private IEnumerable<People> FilterPeople(DataGrid dataGrid, object filter)
        {
            IEnumerable<People> people;
            if (filter != null)
            {
                Filter peopleFilter = (Filter)filter;
                Regex re = new Regex(peopleFilter.UserId == "All"
                    ? "Ex" + RangeGenerator.Generate(peopleFilter.ExpeditionFrom, peopleFilter.ExpeditionTo) + "#" + RangeGenerator.Generate(peopleFilter.PeopleFrom, peopleFilter.PeopleTo)
                    : peopleFilter.UserId);
                bool query(People _) => re.IsMatch(_.UserId) && (_.Workposition == peopleFilter.Profession || peopleFilter.Profession == "All");
                    //&&
                    //((peopleFilter.DateFrom.Ticks == 0 || DateTime.Parse(_.Birthday) >= peopleFilter.DateFrom) 
                    //    && (DateTime.Parse(_.Birthday) <= peopleFilter.DateTo || peopleFilter.DateTo.Ticks == 0));

                people = _preferenceDb.GetPeople(query);
                dataGrid.ItemsSource = people;
                return people;
            }
            people = _preferenceDb.GetPeople();
            return people;
        }

        private IEnumerable<Preference> FilterPreference(DataGrid dataGrid, object filter)
        {
            IEnumerable<Preference> preferences;
            if (filter != null)
            {
                PreferenceFilter preferenceFilter = (PreferenceFilter)filter;
                Regex re = new Regex(preferenceFilter.UserId == "All"
                    ? "Ex" + RangeGenerator.Generate(preferenceFilter.ExpeditionFrom, preferenceFilter.ExpeditionTo) + "#" + RangeGenerator.Generate(preferenceFilter.PeopleFrom, preferenceFilter.PeopleTo)
                    : preferenceFilter.UserId);

                var people = _preferenceDb.GetPeople().Where(item => (item.Workposition == preferenceFilter.Profession || preferenceFilter.Profession == "All"));
                List<string> listOfPeople = new List<string>();
                foreach (var item in people.ToList())
                {
                    listOfPeople.Add(item.UserId.ToString());
                }

                bool query(Preference _) => (re.IsMatch(_.UserId)) &&
                                            ((preferenceFilter.DateFrom.Ticks == 0 || _.Date >= preferenceFilter.DateFrom) && (_.Date <= preferenceFilter.DateTo || preferenceFilter.DateTo.Ticks == 0)) &&
                                            (OrderComparer.Compare(_.ShortOder1, preferenceFilter.PreferenceShort1, preferenceFilter.PreferenceShort2, preferenceFilter.PreferenceShort3)) &&
                                            (OrderComparer.Compare(_.Oder1, preferenceFilter.Preference1, preferenceFilter.Preference2, preferenceFilter.Preference3, preferenceFilter.Preference4, preferenceFilter.Preference5, preferenceFilter.Preference6, preferenceFilter.Preference7, preferenceFilter.Preference8, preferenceFilter.Preference9, preferenceFilter.Preference10, preferenceFilter.Preference11, preferenceFilter.Preference12)) &&
                                            (_.Preference1 == preferenceFilter.Preference || preferenceFilter.Preference == "All") &&
                                            (listOfPeople.Contains(_.UserId));

                preferences = _preferenceDb.GetPreferences(query);
                dataGrid.ItemsSource = preferences;
                return preferences;
            }
            preferences = _preferenceDb.GetPreferences();
            return preferences;
        }

        private IEnumerable<SubjectiveFeeling> FilterSubjectiveFeeling(DataGrid dataGrid, object filter)
        {
            IEnumerable<SubjectiveFeeling> subjectiveFeelings;
            if (filter != null)
            {
                SubjectiveFeelingFilter subjectiveFeelingFilter = (SubjectiveFeelingFilter)filter;
                Regex re = new Regex(subjectiveFeelingFilter.UserId == "All"
                    ? "Ex" + RangeGenerator.Generate(subjectiveFeelingFilter.ExpeditionFrom, subjectiveFeelingFilter.ExpeditionTo) + "#" + RangeGenerator.Generate(subjectiveFeelingFilter.PeopleFrom, subjectiveFeelingFilter.PeopleTo)
                    : subjectiveFeelingFilter.UserId);

                var people = _preferenceDb.GetPeople().Where(item => (item.Workposition == subjectiveFeelingFilter.Profession || subjectiveFeelingFilter.Profession == "All"));
                List<string> listOfPeople = new List<string>();
                foreach (var item in people.ToList())
                {
                    listOfPeople.Add(item.UserId.ToString());
                }
                bool query(SubjectiveFeeling _) =>
                           (re.IsMatch(_.UserId)) &&
                           (subjectiveFeelingFilter.DateFrom.Ticks == 0 || _.Date >= subjectiveFeelingFilter.DateFrom) &&
                           (_.Date <= subjectiveFeelingFilter.DateTo || subjectiveFeelingFilter.DateTo.Ticks == 0) &&
                           (_.GeneralWeaknes == subjectiveFeelingFilter.GeneralWeakness) &&
                           (_.PoorAppetite == subjectiveFeelingFilter.PoorAppetite) &&
                           (_.PoorSleep == subjectiveFeelingFilter.PoorSleep) &&
                           (_.BadMood == subjectiveFeelingFilter.BadMood) &&
                           (_.HeavyHead == subjectiveFeelingFilter.HeavyHead) &&
                           (_.SlowThink == subjectiveFeelingFilter.SlowThink) &&
                           (listOfPeople.Contains(_.UserId));

                subjectiveFeelings = _preferenceDb.GetSubjecriveFeelings(query);
                dataGrid.ItemsSource = subjectiveFeelings;
                return subjectiveFeelings;
            }
            subjectiveFeelings = _preferenceDb.GetSubjecriveFeelings();
            return subjectiveFeelings;
        }
    }
}
