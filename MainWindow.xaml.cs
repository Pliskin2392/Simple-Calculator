using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static List<string> liszSymbols = new List<string>(new string[] { "*", "/", "+", "-" }); // List to compare operations
        private static List<double> lidoValues = new List<double>(); // list to store input values
        private static List<string> liszOperations = new List<string>(); // list to store input operations symbols
        private static List<string> liszOperationsHistory = new List<string>(); // list to show operations history
        private static bool boClearList = false; // Clear list when a number was clicked after an operation 
        public MainWindow()
        {
            InitializeComponent();
            LastOperations.ItemsSource = liszOperationsHistory;
            One.Click += MyButtonClick;
            Two.Click += MyButtonClick;
            Three.Click += MyButtonClick;
            Four.Click += MyButtonClick;
            Five.Click += MyButtonClick;
            Six.Click += MyButtonClick;
            Seven.Click += MyButtonClick;
            Eight.Click += MyButtonClick;
            Nine.Click += MyButtonClick;
            Zero.Click += MyButtonClick;
            Period.Click += MyButtonClick;
            Multiply.Click += MyButtonClick;
            Division.Click += MyButtonClick;
            BackSpace.Click += MyButtonClick;
            Rest.Click += MyButtonClick;
            Add.Click += MyButtonClick;
            Enter.Click += MyButtonClick;
            Clear.Click += MyButtonClick;
        }
        // ********************************** Capture button's click event ***********************************
        void MyButtonClick(object sender, EventArgs e)
        {
            try
            {
                Button button = sender as Button;
                string szTemp = "";
                if(!liszSymbols.Contains(button.Content) && button.Content.ToString() != "Enter" && boClearList)
                {
                    CurrentText.Text = "";
                    boClearList = false;
                }
                if (button.Content.ToString() == "C")
                {
                    ClearLists();
                    CurrentText.Text = "";
                }
                else if(button.Content.ToString() == "<")
                {
                    if (CurrentText.Text.Length != 0)
                        CurrentText.Text = CurrentText.Text.Remove(CurrentText.Text.Length - 1, 1);
                }
                else if (button.Content.ToString() != "Enter")
                {
                    if (!liszSymbols.Contains(button.Content))
                    {
                        if (button.Content.ToString() == "." && !CurrentText.Text.Contains("."))
                            CurrentText.Text += button.Content.ToString();
                        else if (button.Content.ToString() != ".")
                            CurrentText.Text += button.Content.ToString();
                    }
                    else
                    {
                        lidoValues.Add(double.Parse(CurrentText.Text));
                        liszOperations.Add(button.Content.ToString());
                        HistoryText.Text += CurrentText.Text + button.Content.ToString();
                        CurrentText.Text = "";
                    }
                }
                else if (button.Content.ToString() == "Enter")
                {
                    if (!string.IsNullOrEmpty(CurrentText.Text))
                    {
                        lidoValues.Add(double.Parse(CurrentText.Text));
                        if (button.Content.ToString() != "C")
                        {
                            HistoryText.Text += CurrentText.Text;
                            szTemp = HistoryText.Text;
                        }
                        double doResutl = 0;
                        for (int i = 0; i < liszOperations.Count; i++)
                        {
                            doResutl = DoOperation(lidoValues[i], lidoValues[i + 1], liszOperations[i]);
                            lidoValues[i + 1] = doResutl;
                        }
                        if (doResutl != 0)
                            CurrentText.Text = doResutl.ToString("N4");
                        else
                            CurrentText.Text = "0";
                        ClearLists();
                        boClearList = true;
                        liszOperationsHistory.Insert(0, $"{szTemp} = {CurrentText.Text}");
                    }                   
                }
                CurrentText.Focus();
            }
            catch
            {
                AlertBar("Invalid input", 2);
            }
        }
        // ********************************** Basic operations between two numbers **********************************
        double DoOperation(double _doFirstVal, double _doSecondVal, string _szOperation)
        {
            switch (_szOperation)
            {
                case "+":
                    return _doFirstVal + _doSecondVal;
                case "*":
                    return _doFirstVal * _doSecondVal;
                case "/":
                    if (_doSecondVal > 0)
                        return _doFirstVal / _doSecondVal;
                    return 0;
                case "-":
                    return _doFirstVal - _doSecondVal;
                default:
                    return 0;
            }
        }
        // ********************************** Bind physical keyboard keys with buttons **********************************
        void KeyDownMethod(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key);
            if (e.Key == Key.Escape)
            {
                ClearLists();
                CurrentText.Text = "";
                return;
            }
            if (e.Key == Key.D7 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                MyButtonClick(Division, null);
                return;
            }
            if ((e.Key == Key.D8 || e.Key == Key.OemPlus) && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                MyButtonClick(Multiply, null);
                return;
            }
            switch (e.Key)
            {
                case Key.D0:
                    MyButtonClick(Zero, null);
                    break;
                case Key.D1:
                    MyButtonClick(One, null);
                    break;
                case Key.D2:
                    MyButtonClick(Two, null);
                    break;
                case Key.D3:
                    MyButtonClick(Three, null);
                    break;
                case Key.D4:
                    MyButtonClick(Four, null);
                    break;
                case Key.D5:
                    MyButtonClick(Five, null);
                    break;
                case Key.D6:
                    MyButtonClick(Six, null);
                    break;
                case Key.D7:
                    MyButtonClick(Seven, null);
                    break;
                case Key.D8:
                    MyButtonClick(Eight, null);
                    break;
                case Key.D9:
                    MyButtonClick(Nine, null);
                    break;
                case Key.OemPeriod:
                    if (!CurrentText.Text.Contains("."))
                        MyButtonClick(Period, null);
                    break;
                case Key.Return:
                    MyButtonClick(Enter, null);
                    break;
                case Key.Back:
                    if (CurrentText.Text.Length != 0)
                        CurrentText.Text = CurrentText.Text.Remove(CurrentText.Text.Length - 1, 1);
                    break;
                case Key.OemMinus:
                    MyButtonClick(Rest, null);
                    break;
                case Key.OemPlus:
                    MyButtonClick(Add, null);
                    break;
                case Key.OemQuestion:
                    MyButtonClick(Division, null);
                    break;
                default:
                    break;
            }
        }
        // ********************************** Clear values from both lists **********************************
        void ClearLists()
        {
            lidoValues.Clear();
            liszOperations.Clear();
            HistoryText.Text = "";
        }
        // ********************************** Shows notification in alertbar **********************************
        public void AlertBar(string szMessage, int iCode)
        {
            msg.Clear();
            switch (iCode)
            {
                case 0:
                    msg.SetSuccessAlert(szMessage, 5);
                    break;
                case 1:
                    msg.SetWarningAlert(szMessage, 5);
                    break;
                case 2:
                    msg.SetDangerAlert(szMessage, 5);
                    break;
            }
        }
        // ********************************** Show/hide operations history panel **********************************
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LastOperations.Items.Refresh();
            if (!LastOperations.IsVisible)
            {
                LastOperations.Visibility = System.Windows.Visibility.Visible;
                ClearHistoryList.Visibility = System.Windows.Visibility.Visible;
            }
            else
                HideHistoryPanel();
        }
        // ********************************** clear data from operations history panel **********************************
        private void ClearHistoryList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            liszOperationsHistory.Clear();
            LastOperations.Items.Refresh();
            HideHistoryPanel();
        }
        // ********************************** get value from selected item in history panel **********************************
        private void LastOperations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LastOperations.SelectedIndex == -1)
                return;
            try
            {
                string szSelectedResult = LastOperations.SelectedItem.ToString().Split('=')[1].Trim();
                CurrentText.Text = szSelectedResult;
                HideHistoryPanel();
                LastOperations.SelectedIndex = -1;
            }
            catch
            {
                AlertBar("Invalid input", 2);
            }
        }
        // ********************************** Hide operations history panel **********************************
        private void HideHistoryPanel()
        {
            LastOperations.Visibility = System.Windows.Visibility.Hidden;
            ClearHistoryList.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
