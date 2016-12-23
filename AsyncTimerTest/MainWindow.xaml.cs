using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncTimerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window ,INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation.
        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            cat.Tick += Cat_Tick;

            textBox.DataContext = cat;
            checkBox.DataContext = cat;
            textBoxStatus.DataContext = cat;
            textBoxIncrement.DataContext = this;
        }

        private void Cat_Tick(object sender, EventArgs e)
        {
            progressBar.Value = progressBar.Minimum + (progressBar.Value + ProgressIncrementValue) % (progressBar.Maximum - progressBar.Minimum);
        }

        ClassAsyncTimer cat = new ClassAsyncTimer();

        double progressIncrementValue = 0.41;

        public double ProgressIncrementValue
        {
            get
            {
                return progressIncrementValue;
            }

            set
            {
                progressIncrementValue = value;
                NotifyPropertyChanged();
            }
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            cat.Start();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            cat.Stop();
        }

        private void textBoxStatus_TextChanged(object sender, TextChangedEventArgs e)
        {
            listBoxLog.Items.Add((sender as TextBox)?.Text);

            /// http://stackoverflow.com/questions/2337822/wpf-listbox-scroll-to-end-automatically
            if (VisualTreeHelper.GetChildrenCount(listBoxLog) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(listBoxLog, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }
    }
}
