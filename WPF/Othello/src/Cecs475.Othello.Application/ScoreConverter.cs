using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cecs475.Othello.Model;
using System.Windows.Data;
using System.Globalization;

namespace Cecs475.Othello.Application {
    public class ScoreConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                int score = (int)value;
                if (score > 0) {
                    return String.Format("Black is winning by {0}", score);
                }
                else if (score < 0) {
                    return String.Format("White is winning by {0}", -score);
                }
                else {
                    return ("Tie game");
                }
            }
            catch (Exception e) {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
