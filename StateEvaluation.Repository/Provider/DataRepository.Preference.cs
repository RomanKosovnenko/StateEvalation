using StateEvaluation.Common.Enums;
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
        /// Create Preference test
        /// </summary>
        /// <param name="preference">preference</param>
        public void CreatePreferenceTest(Preference preference)
        {
            Preference.Add(preference);
            this.SaveChanges();
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
            Preference.Remove(preference);
            SaveChanges();
        }

        /// <summary>
        /// Update Preference test
        /// </summary>
        /// <param name="newPreference">newPreference</param>
        public void UpdatePreferenceTest(Preference newPreference)
        {
            var preference = Preference.Single(item => item.Id == newPreference.Id);
            preference.Oder1 = newPreference.Oder1;
            preference.Oder2 = newPreference.Oder2;
            preference.Preference1 = newPreference.Preference1;
            preference.Preference2 = newPreference.Preference2;
            preference.RelaxTable1 = newPreference.RelaxTable1;
            preference.RelaxTable2 = newPreference.RelaxTable2;
            preference.ShortOder1 = newPreference.ShortOder1;
            preference.ShortOder2 = newPreference.ShortOder2;
            preference.Compare = newPreference.Preference1 == newPreference.Preference2 ? StringBooleanValues.True : StringBooleanValues.False;

            this.Entry(preference).State = EntityState.Modified;
            SaveChanges();
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
