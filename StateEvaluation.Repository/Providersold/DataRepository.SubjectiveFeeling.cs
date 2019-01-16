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
        /// Remove subjective feeling
        /// </summary>
        /// <param name="subjectiveFeelingId">subjectiveFeelingId</param>
        public void RemoveSubjectiveFeeling(Guid subjectiveFeelingId)
        {
            var subjectiveFeeling = SubjectiveFeeling.Single(item => item.Id == subjectiveFeelingId);
            SubjectiveFeeling.DeleteOnSubmit(subjectiveFeeling);
            SubmitChanges();
        }

        /// <summary>
        /// Get suvjective feeling by id
        /// </summary>
        /// <param name="subjectiveFeelingId">subjectiveFeelingId</param>
        /// <returns></returns>
        public SubjectiveFeeling GetSubjectiveFeeling(Guid subjectiveFeelingId)
        {
            var subjectiveFeeling = SubjectiveFeeling.Single(item => item.Id == subjectiveFeelingId);
            return subjectiveFeeling;
        }

        /// <summary>
        /// Create new SubjectiveFeeling
        /// </summary>
        /// <param name="subjectiveFeeling">subjectiveFeeling</param>
        public void CreateSubjectiveFeeling(SubjectiveFeeling subjectiveFeeling)
        {
            SubjectiveFeeling.InsertOnSubmit(subjectiveFeeling);
            SubmitChanges();
        }

        /// <summary>
        /// Update existing subjectiveFeeling
        /// </summary>
        /// <param name="newSubjectiveFeeling">newSubjectiveFeeling</param>
        public void UpdateSubjectiveFeeling(SubjectiveFeeling newSubjectiveFeeling)
        {
            var subjectiveFeeling = SubjectiveFeeling.Single(item => item.Id == newSubjectiveFeeling.Id);
            subjectiveFeeling.BadMood = newSubjectiveFeeling.BadMood;
            subjectiveFeeling.Date = newSubjectiveFeeling.Date;
            subjectiveFeeling.GeneralWeaknes = newSubjectiveFeeling.GeneralWeaknes;
            subjectiveFeeling.HeavyHead = newSubjectiveFeeling.HeavyHead;
            subjectiveFeeling.PoorAppetite = newSubjectiveFeeling.PoorAppetite;
            subjectiveFeeling.SlowThink = newSubjectiveFeeling.SlowThink;
            subjectiveFeeling.UserId = newSubjectiveFeeling.UserId;
        }

        /// <summary>
        /// Get all SubjectiveFeeling
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SubjectiveFeeling> GetSubjecriveFeelings()
        {
            var items = this.SubjectiveFeeling.Select(item => item).OrderBy(item => item.Date);
            return items;
        }

        /// <summary>
        /// Get all SubjectiveFeeling by query
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        public IEnumerable<SubjectiveFeeling> GetSubjecriveFeelings(Func<SubjectiveFeeling, bool> query)
        {
            var items = SubjectiveFeeling.Where(query).OrderByDescending(item => item.Date);
            return items;
        }
    }
}
