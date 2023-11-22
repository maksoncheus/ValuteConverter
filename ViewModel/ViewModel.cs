using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ValuteConverter
{
    internal class ViewModel : NotifyPropertyChanged
    {
        private string _leftText;
        public string LeftText { get => _leftText; set { _leftText = UnifyInput(value); OnPropertyChanged(nameof(LeftText)); } }

        private string _rightText;
        public string RightText { get => _rightText; set { _rightText = UnifyInput(value); OnPropertyChanged(nameof(RightText)); } }
        private string _dateText;
        public string DateText { get => _dateText; set { _dateText = value; OnPropertyChanged(nameof(DateText)); } }
        internal ViewModel()
        {
            DateText = DateOnly.FromDateTime(DateTime.Now).ToString();
        }
        private string UnifyInput(string input)
        {
            if (string.IsNullOrWhiteSpace( input ))
                return string.Empty;
            input = input.Replace('.', ',');
            if(input.StartsWith(','))
                return string.Empty;
            if (input.Count(c => c == ',') > 1)
                input = new string(input.Take(input.Length - 1).ToArray());
            return input;
        }
    }
}
