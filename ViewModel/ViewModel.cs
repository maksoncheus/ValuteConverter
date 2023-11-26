using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ValuteConverter
{
    internal class ViewModel : NotifyPropertyChanged
    {
        private string _leftText;
        private string _rightText;

        private string _oneUnitRate;
        private string _dateText;

        private ValuteEntry _selectedValuteLeft;
        private ValuteEntry _selectedValuteRight;
        private ObservableCollection<ValuteEntry> _valuteEntries;

        private ICommand _changeSides;
        public string LeftText { get => _leftText; set { _leftText = InputHandler.Unify(value); ConvertLeftToRight(); OnPropertyChanged(nameof(LeftText)); } }

        public string RightText { get => _rightText; set { _rightText = InputHandler.Unify(value); ConvertRightToLeft();  OnPropertyChanged(nameof(RightText)); } }
        
        public string DateText { get => _dateText; set { _dateText = value; OnPropertyChanged(nameof(DateText)); } }
        public string OneUnitRate { get => _oneUnitRate; set { _oneUnitRate = value; OnPropertyChanged(nameof(OneUnitRate)); } }
        public ObservableCollection<ValuteEntry> ValuteEntries
        {
            get => _valuteEntries;
            set
            {
                _valuteEntries = value;
                SetFirstValutes();
                OnPropertyChanged(nameof(ValuteEntries));
            }
        }

        public ValuteEntry SelectedValuteLeft
        {
            get => _selectedValuteLeft;
            set
            {
                if (value == _selectedValuteRight)
                    ChangeSides.Execute(null);
                else _selectedValuteLeft = value;
                if (SelectedValuteRight != null)
                    ConvertLeftToRight();
                OnPropertyChanged(nameof(SelectedValuteLeft));
            }
        }
        public ValuteEntry SelectedValuteRight 
        {
            get => _selectedValuteRight;
            set
            {
                if (value == _selectedValuteLeft)
                    ChangeSides.Execute(null);
                else _selectedValuteRight = value;
                if (SelectedValuteLeft != null)
                    ConvertLeftToRight();
                OnPropertyChanged(nameof(SelectedValuteRight));
            }
        }
        public ViewModel()
        {
            DateText = DateOnly.FromDateTime(DateTime.Now).ToString();
            ValuteEntries = new();
            Task.Run(LoadValutes);
        }
        private async void LoadValutes()
        {
            ValuteEntries = new ObservableCollection<ValuteEntry>(CoinYepParser.GetValuteEntries( await SoapRequest.GetCursOnDateAsync(DateTime.Now)));
        }
        public ICommand ChangeSides
        {
            get => _changeSides ??= new CommonCommand(
                () =>
                {
                    (_selectedValuteRight, _selectedValuteLeft) = (_selectedValuteLeft, _selectedValuteRight);
                    ConvertLeftToRight();
                    OnPropertyChanged(nameof(SelectedValuteLeft));
                    OnPropertyChanged(nameof(SelectedValuteRight));
                }
                );
        }
        private void SetFirstValutes()
        {
            try
            {
                SelectedValuteLeft = ValuteEntries?[0] ?? null;
                SelectedValuteRight = ValuteEntries?[1] ?? null;
            }
            catch (Exception ex)
            {

            }
        }
        private double ConvertValutes(ValuteEntry left, ValuteEntry right)
        {
            return left.ValuteCourse  / right.ValuteCourse;
        }
        private void ConvertLeftToRight()
        {
            if (string.IsNullOrEmpty(_leftText))
                _rightText = string.Empty;
            else
                _rightText = Math.Round(Convert.ToDouble(_leftText) * ConvertValutes(_selectedValuteLeft, SelectedValuteRight), 2).ToString();
            OnPropertyChanged(nameof(RightText));
        }
        private void ConvertRightToLeft()
        {
            if (string.IsNullOrEmpty(_rightText))
                _leftText = string.Empty;
            else
                _leftText = Math.Round(Convert.ToDouble(_rightText) * ConvertValutes(SelectedValuteRight, SelectedValuteLeft), 2).ToString();
            OnPropertyChanged(nameof(LeftText));
        }
    }
}
