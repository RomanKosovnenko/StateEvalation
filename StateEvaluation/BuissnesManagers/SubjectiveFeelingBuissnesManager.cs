using StateEvaluation.Model;
using StateEvaluation.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StateEvaluation.Providers
{
    public class SubjectiveFeelingBuissnesManager
    {
        private PreferenceDB _preferenceDb = new PreferenceDB();

        public DataGrid SubjectiveFeelingDataGrid { get; }
        public Button UpdateSubjectiveFeelingBtn { get; }

        public SubjectiveFeelingBuissnesManager(DataGrid subjectiveFeelingDataGrid, Button updateSubjectiveFeelingBtn)
        {
            SubjectiveFeelingDataGrid = subjectiveFeelingDataGrid;
            UpdateSubjectiveFeelingBtn = updateSubjectiveFeelingBtn;
        }

        public void Update(SubjectiveFeelingVM subjectiveFeelingMV, Guid subjectiveFeelingId)
        {
            try
            {
                var subjectiveFeeling = GenerateSubjectiveFeeling(subjectiveFeelingMV, subjectiveFeelingId);
                _preferenceDb.UpdateSubjectiveFeeling(subjectiveFeeling);
                ClearInputs(subjectiveFeelingMV);

                RefreshDataGrid();
                
                //hide update button
                UpdateSubjectiveFeelingBtn.Visibility = Visibility.Hidden;

                MessageBox.Show("Subjective feeling was updated");
            }
            catch
            {
                MessageBox.Show("Oops, error while updating");
            }
        }

        public void Remove(Guid subjectiveFeelingId)
        {
            try
            {
                _preferenceDb.RemoveSubjectiveFeeling(subjectiveFeelingId);
                RefreshDataGrid();

                MessageBox.Show("Subjective feeling was removed");
            }
            catch
            {
                MessageBox.Show("Oops, error while removing");
            }
        }

        public void Create(SubjectiveFeelingVM subjectiveFeelingVM)
        {
            try
            {
                if (IsValidSubjectiveFeelingVM(subjectiveFeelingVM))
                {
                    SubjectiveFeeling subjectiveFeeling = GenerateSubjectiveFeeling(subjectiveFeelingVM);
                    _preferenceDb.CreateSubjectiveFeeling(subjectiveFeeling);
                    ClearInputs(subjectiveFeelingVM);

                    RefreshDataGrid();
                    MessageBox.Show("Subjective feeling was created");
                }
                else
                {
                    MessageBox.Show("Oops, error while creating");
                }
            }
            catch
            {
                MessageBox.Show("Oops, error while creating");
            }
        }

        public void PrepareInputForm(SubjectiveFeelingVM subjectiveFeelingVM, Guid subjectiveFeelingId)
        {
            try
            {
                var subjectiveFeeling = _preferenceDb.GetSubjectiveFeeling(subjectiveFeelingId);
                SetInputForm(subjectiveFeeling, subjectiveFeelingVM);

                //show save subjective feeling button
                UpdateSubjectiveFeelingBtn.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("Oops, error while binding input filds");
            }
        }

        #region private methods

        private void RefreshDataGrid()
        {
            SubjectiveFeelingDataGrid.ItemsSource = _preferenceDb.GetSubjecriveFeelings();
        }

        private bool IsValidSubjectiveFeelingVM(SubjectiveFeelingVM subjectiveFeeling)
        {
            if (!DateTime.TryParse((subjectiveFeeling.Date).ToString(), out DateTime fealingDate) ||
                 string.IsNullOrEmpty(subjectiveFeeling.UserId))
            {
                return false;
            }
            return true;
        }

        private void ClearInputs(SubjectiveFeelingVM subjectiveFeelingVM)
        {
            subjectiveFeelingVM.Date = new object();
            subjectiveFeelingVM.BadMood = false;
            subjectiveFeelingVM.GeneralWeaknes = false;
            subjectiveFeelingVM.SlowThink = false;
            subjectiveFeelingVM.PoorSleep = false;
            subjectiveFeelingVM.HeavyHead = false;
            subjectiveFeelingVM.PoorAppetite = false;
            subjectiveFeelingVM.UserId = string.Empty;
        }

        private SubjectiveFeeling GenerateSubjectiveFeeling(SubjectiveFeelingVM subjectiveFeelingVM, Guid? id = null)
        {
            return new SubjectiveFeeling()
            {
                Date = DateTime.Parse(subjectiveFeelingVM.Date.ToString()),
                BadMood = subjectiveFeelingVM.BadMood,
                GeneralWeaknes = subjectiveFeelingVM.GeneralWeaknes,
                SlowThink = subjectiveFeelingVM.SlowThink,
                PoorSleep = subjectiveFeelingVM.PoorSleep,
                HeavyHead = subjectiveFeelingVM.HeavyHead,
                PoorAppetite = subjectiveFeelingVM.PoorAppetite,
                Id = id ?? Guid.NewGuid(),
                UserId = subjectiveFeelingVM.UserId
            };
        }

        private void SetInputForm(SubjectiveFeeling subjectiveFeeling, SubjectiveFeelingVM subjectiveFeelingVM)
        {
            subjectiveFeelingVM.Id = subjectiveFeeling.Id.ToString();
            subjectiveFeelingVM.Date = subjectiveFeeling.Date;
            subjectiveFeelingVM.BadMood = subjectiveFeeling.BadMood;
            subjectiveFeelingVM.GeneralWeaknes = subjectiveFeeling.GeneralWeaknes;
            subjectiveFeelingVM.SlowThink = subjectiveFeeling.SlowThink;
            subjectiveFeelingVM.PoorSleep = subjectiveFeeling.PoorSleep;
            subjectiveFeelingVM.HeavyHead = subjectiveFeeling.HeavyHead;
            subjectiveFeelingVM.PoorAppetite = subjectiveFeeling.PoorAppetite;
            subjectiveFeelingVM.UserId = subjectiveFeeling.UserId;
        }
        #endregion
    }
}
