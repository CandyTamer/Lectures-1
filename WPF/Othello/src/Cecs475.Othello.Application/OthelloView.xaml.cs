﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cecs475.Othello.Application {
	/// <summary>
	/// Interaction logic for OthelloView.xaml
	/// </summary>
	public partial class OthelloView : UserControl {
		public static SolidColorBrush RED_BRUSH = new SolidColorBrush(Colors.Red);
		public static SolidColorBrush GREEN_BRUSH = new SolidColorBrush(Colors.Green);

		public OthelloView() {
			InitializeComponent();
		}

		private void Border_MouseEnter(object sender, MouseEventArgs e) {
			Border b = sender as Border;
			var square = b.DataContext as OthelloSquare;
			var vm = FindResource("vm") as OthelloViewModel;
			if (vm.PossibleMoves.Contains(square.Position)) {
				b.Background = RED_BRUSH;
			}
		}

		private void Border_MouseLeave(object sender, MouseEventArgs e) {
			Border b = sender as Border;
			b.Background = GREEN_BRUSH;
		}

		public OthelloViewModel Model {
			get { return FindResource("vm") as OthelloViewModel; }
		}

		private void Border_MouseUp(object sender, MouseButtonEventArgs e) {
			Border b = sender as Border;
			var square = b.DataContext as OthelloSquare;
			var vm = FindResource("vm") as OthelloViewModel;
			if (vm.PossibleMoves.Contains(square.Position)) {
				vm.ApplyMove(square.Position);
			}
		}
	}

	/// <summary>
	/// Converts from an integer player number to an Ellipse representing that player's token.
	/// </summary>
	public class OthelloSquarePlayerConverter : IValueConverter {
		private static SolidColorBrush BLUE_BRUSH = new SolidColorBrush(Colors.Blue);
		private static SolidColorBrush PURPLE_BRUSH = new SolidColorBrush(Colors.Purple);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int player = (int)value;
            if (player == 0) {
                return null;
            }

            Rectangle token = new Rectangle() {
                Fill = GetFillBrush(player),
                Height = 30,
                Width = 40
			};
			return token;
		}

		private static SolidColorBrush GetFillBrush(int player) {
			if (player == 1)
				return PURPLE_BRUSH;
			return BLUE_BRUSH;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
