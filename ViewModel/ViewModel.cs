using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ValuteConverter
{
    internal class ViewModel : NotifyPropertyChanged
    {
        private string _leftText;
        private ValuteEntry _selectedValuteLeft;
        private ValuteEntry _selectedValuteRight;
        private ObservableCollection<ValuteEntry> _valuteEntries;
        private ICommand _changeSides;
        public string LeftText { get => _leftText; set { _leftText = UnifyInput(value); OnPropertyChanged(nameof(LeftText)); } }

        private string _rightText;
        public string RightText { get => _rightText; set { _rightText = UnifyInput(value); OnPropertyChanged(nameof(RightText)); } }
        private string _dateText;
        public string DateText { get => _dateText; set { _dateText = value; OnPropertyChanged(nameof(DateText)); } }
        public ObservableCollection<ValuteEntry> ValuteEntries { get => _valuteEntries; set { _valuteEntries = value; SetFirstValutes(); OnPropertyChanged(nameof(ValuteEntries)); } }

        public ValuteEntry SelectedValuteLeft { get => _selectedValuteLeft; set { if (value == _selectedValuteRight) ChangeSides.Execute(null); else _selectedValuteLeft = value; OnPropertyChanged(nameof(SelectedValuteLeft)); } }
        public ValuteEntry SelectedValuteRight { get => _selectedValuteRight; set { if (value == _selectedValuteLeft) ChangeSides.Execute(null); else _selectedValuteRight = value; OnPropertyChanged(nameof(SelectedValuteRight)); } }
        public ViewModel()
        {
            DateText = DateOnly.FromDateTime(DateTime.Now).ToString();
            Task.Run( () => ValuteEntries = new ObservableCollection<ValuteEntry>(CoinYepParser.GetValuteEntries(SoapRequest.GetCursOnDateAsync(DateTime.Now).Result)));
            
            //SoapRequest.GetCursOnDateAsync(DateTime.Now);
        }
        private string UnifyInput(string input)
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
            DataTable td = SoapRequest.GetCursOnDateAsync(DateTime.Now).Result;
            ValuteEntries = new ObservableCollection<ValuteEntry>(CoinYepParser.GetValuteEntries(SoapRequest.GetCursOnDateAsync(DateTime.Now).Result));
        }
        public ICommand ChangeSides
        {
            get => _changeSides ??= new CommonCommand(
                () =>
                {
                    ValuteEntry temp = _selectedValuteLeft;
                    _selectedValuteLeft = _selectedValuteRight;
                    _selectedValuteRight = temp;
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
