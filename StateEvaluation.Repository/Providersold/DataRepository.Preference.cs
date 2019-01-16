using StateEvaluation.Common.Enums;
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
        /// Create Preference test
        /// </summary>
        /// <param name="preference">preference</param>
        public void CreatePreferenceTest(Preference preference)
        {
            Preference.InsertOnSubmit(preference);
            SubmitChanges();
        }

        /// <summary>
        /// Get preference test by id
        /// </summary>
        /// <param name="id">preferenceId</param>
        /// <returns></returns>
        public Preference GetPreferenceTest(Guid id)
        {
            var preference = Preference.Single(item => item.Id == id);
            return preference;
        }

        /// <summary>
        /// Get all Preference tests
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Preference> GetPreferenceTests()
        {
            var items = this.Preference.Select(item => item).OrderByDescending(item => item.Date);
            return items;
        }

        /// <summary>
        /// Delete Preference test by id
        /// </summary>
        /// <param name="id">preferenceId</param>
        public void DeletePreferenceTest(string id)
        {
            var preference = Preference.Single(item => item.Id.ToString() == id);
            Preference.DeleteOnSubmit(preference);
            SubmitChanges();
        }

        /// <summary>
        /// Update Preference test
        /// </summary>
        /// <param name="newPreference">newPreference</param>
        public void UpdatePreferenceTest(Preference newPreference)
        {
            var items = Preference.Single(item => item.Id == newPreference.Id);
            items.Oder1 = newPreference.Oder1;
            items.Oder2 = newPreference.Oder2;
            items.Preference1 = newPreference.Preference1;
            items.Preference2 = newPreference.Preference2;
            items.RelaxTable1 = newPreference.RelaxTable1;
            items.RelaxTable2 = newPreference.RelaxTable2;
            items.ShortOder1 = newPreference.ShortOder1;
            items.ShortOder2 = newPreference.ShortOder2;
            items.Compare = newPreference.Preference1 == newPreference.Preference2 ? StringBooleanValues.True : StringBooleanValues.False;
            SubmitChanges();
        }

        /// <summary>
        /// Get Preference tests by query
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        public IEnumerable<Preference> GetPreferenceTests(Func<Preference, bool> query)
        {
            var preferences = this.Preference.Where(query).OrderByDescending(item => item.Date);
            return preferences;
        }

        /// <summary>
        /// Get short colors numbers of preference for filters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetShortColorsNumbersFilters()
        {
            return PreferenceValues.ShortColorsNumbersList;
        }

        /// <summary>
        /// Get colors numbers of preference for filters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetColorsNumbersFilters()
        {
            return PreferenceValues.ColorsNumbersList;
        }

        /// <summary>
        /// Get preferences values for filters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> PreferencesFilters()
        {
            return PreferenceValues.Preferences;
        }
    }
}
