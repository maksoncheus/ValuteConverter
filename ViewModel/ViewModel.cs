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
        public string LeftText { get => _leftText; set { _leftText = UnifyInput(value); _rightText = (SelectedValuteLeft.ValuteCourse * Convert.ToDouble(_leftText) / SelectedValuteRight.ValuteCourse).ToString(); OnPropertyChanged(nameof(RightText)); OnPropertyChanged(nameof(LeftText)); } }

        public string RightText { get => _rightText; set { _rightText = UnifyInput(value); OnPropertyChanged(nameof(RightText)); } }
        
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
                OnPropertyChanged(nameof(SelectedValuteRight));
            }
        }
        public ViewModel()
        {
            DateText = DateOnly.FromDateTime(DateTime.Now).ToString();
            ValuteEntries = new();
            Task.Run(LoadValutes);
        }
        private static string UnifyInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            input = input.Replace('.', ',');
            if (input.StartsWith(','))
                return string.Empty;
            if (input.Count(c => c == ',') > 1)
                input = new string(input.Take(input.Length - 1).ToArray());
            return input;
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
    }
}
