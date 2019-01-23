using StateEvaluation.Repository.Providers;
using StateEvaluation.Repository.Models;
using StateEvaluation.Common.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using StateEvaluation.Common.Constants;

namespace StateEvaluation.BussinesLayer.BuissnesManagers
{
    public class SubjectiveFeelingBuissnesManager
    {
        private DataRepository _dataRepository = new DataRepository();

        private DataGrid _subjectiveFeelingDataGrid { get; }
        private Button _updateSubjectiveFeelingBtn { get; }

        public SubjectiveFeelingBuissnesManager(DataGrid subjectiveFeelingDataGrid, Button updateSubjectiveFeelingBtn)
        {
            _subjectiveFeelingDataGrid = subjectiveFeelingDataGrid;
            _updateSubjectiveFeelingBtn = updateSubjectiveFeelingBtn;
        }

        public bool TryUpdateFeeling(SubjectiveFeeling feeling, string changedProperty, string newValue)
        {
            try
            {
                if (changedProperty == "Date")
                {
                    feeling.GetType().GetProperty(changedProperty).SetValue(feeling, DateTime.Parse(newValue));
                }
                else
                {
                    feeling.GetType().GetProperty(changedProperty).SetValue(feeling, bool.Parse(newValue));
                }
                _dataRepository.UpdateSubjectiveFeeling(feeling);
                MessageBox.Show(MessageBoxConstants.FeelUpdated);
                return true;
            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.WrongFields);
                return false;
            }
        }

        public void Update(SubjectiveFeelingVM subjectiveFeelingMV, Guid subjectiveFeelingId)
        {
            try
            {
                var subjectiveFeeling = GenerateSubjectiveFeeling(subjectiveFeelingMV, subjectiveFeelingId);
                _dataRepository.UpdateSubjectiveFeeling(subjectiveFeeling);
                ClearInputsInternal(subjectiveFeelingMV);

                RefreshDataGrid();

                ToggleButton(_updateSubjectiveFeelingBtn, Visibility.Hidden);

                MessageBox.Show(MessageBoxConstants.FeelUpdated);
            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.ErrorUpdating);
            }
        }

        public void Remove(Guid subjectiveFeelingId)
        {
            try
            {
                _dataRepository.RemoveSubjectiveFeeling(subjectiveFeelingId);
                RefreshDataGrid();

                MessageBox.Show(MessageBoxConstants.FeelDeleted);
            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.ErrorFeelDelete);
            }
        }

        public void Create(SubjectiveFeelingVM subjectiveFeelingVM)
        {
            try
            {
                if (IsValidSubjectiveFeelingVM(subjectiveFeelingVM))
                {
                    SubjectiveFeeling subjectiveFeeling = GenerateSubjectiveFeeling(subjectiveFeelingVM);
                    _dataRepository.CreateSubjectiveFeeling(subjectiveFeeling);
                    ClearInputsInternal(subjectiveFeelingVM);

                    RefreshDataGrid();
                    MessageBox.Show(MessageBoxConstants.FeelCreated);
                }
                else
                {
                    MessageBox.Show(MessageBoxConstants.ErrorFeelCreate);
                }
            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.ErrorFeelCreate);
            }
        }

        public void PrepareInputForm(SubjectiveFeelingVM subjectiveFeelingVM, Guid subjectiveFeelingId)
        {
            try
            {
                var subjectiveFeeling = _dataRepository.GetSubjectiveFeeling(subjectiveFeelingId);
                SetInputForm(subjectiveFeeling, subjectiveFeelingVM);

                ToggleButton(_updateSubjectiveFeelingBtn, Visibility.Visible);
            }
            catch
            {
                MessageBox.Show(MessageBoxConstants.ErrorBinding);
            }
        }

        public void ClearInputs(SubjectiveFeelingVM subjectiveFeelingVM)
        {
            ClearInputsInternal(subjectiveFeelingVM);
            ToggleButton(_updateSubjectiveFeelingBtn, Visibility.Hidden);
        }

        #region private methods
        private void ToggleButton(Button button, Visibility visibility)
        {
            button.Visibility = visibility;
        }

        private void RefreshDataGrid()
        {
            _subjectiveFeelingDataGrid.ItemsSource = _dataRepository.GetSubjecriveFeelings();
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

        private void ClearInputsInternal(SubjectiveFeelingVM subjectiveFeelingVM)
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
