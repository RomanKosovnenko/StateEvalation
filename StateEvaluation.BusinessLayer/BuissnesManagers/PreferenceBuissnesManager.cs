using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using StateEvaluation.Repository.Providers;
using StateEvaluation.Repository.Models;
using StateEvaluation.Common.ViewModel;
using StateEvaluation.BussinesLayer.Enums;

namespace StateEvaluation.BussinesLayer.BuissnesManagers
{
    public class PreferenceBuissnesManager
    {
        private DataRepository _dataRepository = new DataRepository();
        private List<string> _color1in3s = new List<string>();
        private List<string> _color2in3s = new List<string>();
        private List<string> _color1in12s = new List<string>();
        private List<string> _color2in12s = new List<string>();
        private List<bool> _preference1 = new List<bool>();
        private List<bool> _preference2 = new List<bool>();
        private PreferenceVM _previouspreferenceVM = new PreferenceVM();

        public DataGrid PreferenceDataGrid { get; }
        public Button UpdatePrefernceBtn { get; }

        public PreferenceBuissnesManager(DataGrid preferenceDataGrid, Button updatePrefernceBtn)
        {
            PreferenceDataGrid = preferenceDataGrid;
            UpdatePrefernceBtn = updatePrefernceBtn;
        }

        public void Create(PreferenceVM preferenceVM)
        {
            try
            {
                if (IsValidPreferenseVM(preferenceVM))
                {
                    var preference = GetNewPreference(preferenceVM);

                    _dataRepository.InsertPreference(preference);
                    ClearInputsInternal(preferenceVM);

                    RefreshDataGrid();
                    MessageBox.Show("Test was created");
                }
                else
                {
                    MessageBox.Show("Oops, not all fields is filled!");
                }
            }
            catch
            {
                MessageBox.Show("Oops, error while creating");
            }
        }

        public void RemovePreference(string id)
        {
            try
            {
                _dataRepository.DeletePreference(id);
                RefreshDataGrid();
                MessageBox.Show("Preference was removed");
            }
            catch
            {
                MessageBox.Show("Oops, somesing was wrong, while deleting");
            }
        }

        public void UpdatePreference(PreferenceVM preferenceVM)
        {
            try
            {
                if (HasChanges(preferenceVM))
                {
                    if (IsValidPreferenseVM(preferenceVM))
                    {
                        var preference = GetNewPreference(preferenceVM, new Guid(preferenceVM.Id));
                        _dataRepository.UpdatePreference(preference);
                    }
                    else
                    {
                        MessageBox.Show("Oops, input form is not valid");
                    }
                }

                MessageBox.Show("Preference was updated");
            }
            catch
            {
                MessageBox.Show("Oops, error while updating");
            }
        }

        public void PrepareInputForm(PreferenceVM preferenceVM, Guid preferenceId)
        {
            try
            {
                var preference = _dataRepository.GeneratePreference(preferenceId);
                SetValueInTabs(preferenceVM, preference);
                _previouspreferenceVM = preferenceVM;
                
                ToggleButton(UpdatePrefernceBtn, Visibility.Visible);
            }
            catch
            {
                MessageBox.Show("Oops, error while binding input filds");
            }
        }

        public void ClearInputs(PreferenceVM preferenceVM)
        {
            ToggleButton(UpdatePrefernceBtn, Visibility.Hidden);
            ClearInputsInternal(preferenceVM);
        }

        public Preference GetPreference(Preference preference)
        {
            return _dataRepository.GetPreference(preference);
        }
        #region private methods
        private void ToggleButton(Button button, Visibility visibility)
        {
            button.Visibility = visibility;
        }

        private void ClearInputsInternal(PreferenceVM preferenceVM)
        {
            foreach (var i in preferenceVM.GetType().GetProperties())
            {
                i.SetValue(preferenceVM, string.Empty);
            }
        }

        private void RefreshDataGrid()
        {
            PreferenceDataGrid.ItemsSource = _dataRepository.GetPreferences();
        }

        private bool IsValidPreferenseVM(PreferenceVM preferenceVM)
        {
            _color1in3s = new List<string>() { preferenceVM.Color1in3, preferenceVM.Color2in3, preferenceVM.Color3in3 };

            _color2in3s = new List<string>() { preferenceVM.Color21in3, preferenceVM.Color22in3, preferenceVM.Color23in3 };

            _color1in12s = new List<string>() {
                preferenceVM.Color1in12, preferenceVM.Color2in12, preferenceVM.Color3in12,
                preferenceVM.Color4in12, preferenceVM.Color5in12, preferenceVM.Color6in12,
                preferenceVM.Color7in12, preferenceVM.Color8in12, preferenceVM.Color9in12,
                preferenceVM.Color10in12, preferenceVM.Color11in12, preferenceVM.Color12in12
            };

            _color2in12s = new List<string>() {
                preferenceVM.Color21in12, preferenceVM.Color22in12, preferenceVM.Color23in12,
                preferenceVM.Color24in12,  preferenceVM.Color25in12, preferenceVM.Color26in12,
                preferenceVM.Color27in12, preferenceVM.Color28in12,  preferenceVM.Color29in12,
                preferenceVM.Color210in12,  preferenceVM.Color211in12, preferenceVM.Color212in12
            };

            _preference1 = new List<bool>() { preferenceVM.Preference1Blue == StringBooleanValues.True, preferenceVM.Preference1Grey == StringBooleanValues.True,
                preferenceVM.Preference1Yellow == StringBooleanValues.True, preferenceVM.Preference1Red == StringBooleanValues.True };
            _preference2 = new List<bool>() { preferenceVM.Preference2Blue == StringBooleanValues.True, preferenceVM.Preference2Grey == StringBooleanValues.True,
                preferenceVM.Preference2Yellow == StringBooleanValues.True, preferenceVM.Preference2Red == StringBooleanValues.True };

            var userId = new List<string> { preferenceVM.UserId };

            if (false && (new List<List<string>> { userId, _color1in3s, _color2in3s, _color1in12s, _color2in12s }.Any(x => x.Any(y => y == null) || x.Count != x.Distinct().Count())
                || new List<List<bool>> { _preference1, _preference2 }.Any(x => x.All(y => y == false))
                || preferenceVM.TestDate == null))
            {
                return false;
            }
            return true;
        }

        private Preference GetNewPreference(PreferenceVM preferenceVM, Guid? id = null)
        {
            List<byte> Color1in3sByte = _color1in3s.Select(x => Byte.Parse(x)).ToList();
            List<byte> Color1in12sByte = _color1in12s.Select(x => Byte.Parse(x)).ToList();
            List<byte> Color2in3sByte = _color2in3s.Select(x => Byte.Parse(x)).ToList();
            List<byte> Color2in12sByte = _color2in12s.Select(x => Byte.Parse(x)).ToList();
            
            var Preference1Index = _preference1.IndexOf(true);
            var Preference2Index = _preference2.IndexOf(true);
            Preference preference = new Preference()
            {
                Id = id ?? Guid.NewGuid(),
                UserId = preferenceVM.UserId,
                Date = DateTime.Parse(preferenceVM.TestDate.ToString()),
                FavoriteColor = 0,
                ShortOder1 = String.Join(",", _color1in3s),
                Oder1 = String.Join(",", _color1in12s),
                Preference1 = new StateEvaluationDLL.DataStructures.Preference(Color1in3sByte, Color2in3sByte).Type.ToString(), 
                ShortOder2 = String.Join(",", _color2in3s),
                Oder2 = String.Join(",", _color2in12s),
                Preference2 = new StateEvaluationDLL.DataStructures.Preference(Color2in3sByte, Color2in12sByte).Type.ToString(),
                Compare = (_preference1.IndexOf(true) == _preference2.IndexOf(true)).ToString().ToLower(),
                RelaxTable1 = int.Parse(preferenceVM.ColorRelax1),
                RelaxTable2 = int.Parse(preferenceVM.ColorRelax2)
            };

            return preference;
        }

        private void SetValueInTabs(PreferenceVM preferenceVM, Preference preference)
        {
            var shortOrder1List = preference.ShortOder1.ToString().Split(',');
            var shortOrder2List = preference.ShortOder2.ToString().Split(',');
            var order1List = preference.Oder1.ToString().Split(',');
            var order2List = preference.Oder2.ToString().Split(',');

            preferenceVM.Id = preference.Id.ToString();
            preferenceVM.UserId = preference.UserId;
            preferenceVM.TestDate = preference.Date;

            preferenceVM.Color1in3 = shortOrder1List[0];
            preferenceVM.Color2in3 = shortOrder1List[1];
            preferenceVM.Color3in3 = shortOrder1List[2];

            preferenceVM.Color21in3 = shortOrder2List[0];
            preferenceVM.Color22in3 = shortOrder2List[1];
            preferenceVM.Color23in3 = shortOrder2List[2];

            preferenceVM.Color1in12 = order1List[0];
            preferenceVM.Color2in12 = order1List[1];
            preferenceVM.Color3in12 = order1List[2];
            preferenceVM.Color4in12 = order1List[3];
            preferenceVM.Color5in12 = order1List[4];
            preferenceVM.Color6in12 = order1List[5];
            preferenceVM.Color7in12 = order1List[6];
            preferenceVM.Color8in12 = order1List[7];
            preferenceVM.Color9in12 = order1List[8];
            preferenceVM.Color10in12 = order1List[9];
            preferenceVM.Color11in12 = order1List[10];
            preferenceVM.Color12in12 = order1List[11];

            preferenceVM.Color21in12 = order2List[0];
            preferenceVM.Color22in12 = order2List[1];
            preferenceVM.Color23in12 = order2List[2];
            preferenceVM.Color24in12 = order2List[3];
            preferenceVM.Color25in12 = order2List[4];
            preferenceVM.Color26in12 = order2List[5];
            preferenceVM.Color27in12 = order2List[6];
            preferenceVM.Color28in12 = order2List[7];
            preferenceVM.Color29in12 = order2List[8];
            preferenceVM.Color210in12 = order2List[9];
            preferenceVM.Color211in12 = order2List[10];
            preferenceVM.Color212in12 = order2List[11];

            preferenceVM.ColorRelax1 = preference.RelaxTable1.ToString();
            preferenceVM.ColorRelax2 = preference.RelaxTable2.ToString();

            ResolvePreference(preferenceVM, preference);
        }

        private void ResolvePreference(PreferenceVM preferenceVM, Preference preference)
        {
            int counter = 1;
            var preferenceValues = new List<string>() { preference.Preference1, preference.Preference2 };
            foreach(var preferenceValue in preferenceValues)
            {
                switch (preferenceValue.Trim())
                {
                    case PreferenceColors.Red:
                        preferenceVM.GetType().GetProperty($"Preference{counter}Red").SetValue(preferenceVM, StringBooleanValues.True);
                        break;
                    case PreferenceColors.Yellow:
                        preferenceVM.GetType().GetProperty($"Preference{counter}Yellow").SetValue(preferenceVM, StringBooleanValues.True);
                        break;
                    case PreferenceColors.Blue:
                        preferenceVM.GetType().GetProperty($"Preference{counter}Blue").SetValue(preferenceVM, StringBooleanValues.True);
                        break;
                    case PreferenceColors.Gray:
                        preferenceVM.GetType().GetProperty($"Preference{counter}Grey").SetValue(preferenceVM, StringBooleanValues.True);
                        break;
                }
                ++counter;
            }
        }

        private bool HasChanges(PreferenceVM preferenceVM)
        {
            foreach(var property in preferenceVM.GetType().GetProperties())
            {
                if(preferenceVM.GetType().GetProperty(property.Name) != _previouspreferenceVM.GetType().GetProperty(property.Name))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}