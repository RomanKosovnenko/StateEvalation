using StateEvaluation.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Repository.Providers
{
    public partial class DataRepository : DbContext
    { 
        /// <summary>
        /// Remove subjective feeling
        /// </summary>
        /// <param name="subjectiveFeelingId">subjectiveFeelingId</param>
        public void RemoveSubjectiveFeeling(Guid subjectiveFeelingId)
        {
            var subjectiveFeeling = SubjectiveFeelings.Single(item => item.Id == subjectiveFeelingId);
            SubjectiveFeelings.Remove(subjectiveFeeling);
            SaveChanges();
        }

        /// <summary>
        /// Get suvjective feeling by id
        /// </summary>
        /// <param name="subjectiveFeelingId">subjectiveFeelingId</param>
        /// <returns></returns>
        public SubjectiveFeelings GetSubjectiveFeeling(Guid subjectiveFeelingId)
        {
            var subjectiveFeeling = SubjectiveFeelings.Single(item => item.Id == subjectiveFeelingId);
            return subjectiveFeeling;
        }

        /// <summary>
        /// Create new SubjectiveFeeling
        /// </summary>
        /// <param name="subjectiveFeeling">subjectiveFeeling</param>
        public void CreateSubjectiveFeeling(SubjectiveFeelings subjectiveFeeling)
        {
            SubjectiveFeelings.Add(subjectiveFeeling);
            SaveChanges();
        }

        /// <summary>
        /// Update existing subjectiveFeeling
        /// </summary>
        /// <param name="newSubjectiveFeeling">newSubjectiveFeeling</param>
        public void UpdateSubjectiveFeeling(SubjectiveFeelings newSubjectiveFeeling)
        {
            var subjectiveFeeling = SubjectiveFeelings.Single(item => item.Id == newSubjectiveFeeling.Id);
            subjectiveFeeling.BadMood = newSubjectiveFeeling.BadMood;
            subjectiveFeeling.Date = newSubjectiveFeeling.Date;
            subjectiveFeeling.GeneralWeaknes = newSubjectiveFeeling.GeneralWeaknes;
            subjectiveFeeling.HeavyHead = newSubjectiveFeeling.HeavyHead;
            subjectiveFeeling.PoorAppetite = newSubjectiveFeeling.PoorAppetite;
            subjectiveFeeling.SlowThink = newSubjectiveFeeling.SlowThink;
            subjectiveFeeling.UserId = newSubjectiveFeeling.UserId;

            this.Entry(subjectiveFeeling).State = EntityState.Modified;
            SaveChanges();
        }

        /// <summary>
        /// Get all SubjectiveFeeling
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SubjectiveFeelings> GetSubjecriveFeelings()
        {
            var items = this.SubjectiveFeelings.Select(item => item).OrderBy(item => item.Date);
            return items;
        }

        /// <summary>
        /// Get all SubjectiveFeeling by query
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        public IEnumerable<SubjectiveFeelings> GetSubjecriveFeelings(Func<SubjectiveFeelings, bool> query)
        {
            var items = SubjectiveFeelings.Where(query).OrderByDescending(item => item.Date);
            return items;
        }
    }
}
