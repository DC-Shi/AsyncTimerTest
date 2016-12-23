using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTimerTest
{
    class ClassAsyncTimer : INotifyPropertyChanged
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

        #region Properties

        int interval = 100;
        bool isEnabled;
        string status;
        bool isInLoop = true;
        int tickCount = 0;

        /// <summary>
        /// The trigger interval of this timer.
        /// Note: if you set this to a very small number, the program might be hanged.
        /// </summary>
        public int Interval
        {
            get
            {
                return interval;
            }

            set
            {
                interval = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// This timer is enabled or not.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The status string of this timer.
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// This EventHandler would trigger after waiting interval.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        /// Start in async method.
        /// This method use Task.Delay.
        /// </summary>
        async void StartAsync()
        {
            Status = "Started in Async method";
            while (isInLoop)
            {
                if (IsEnabled)
                {
                    Tick(this, EventArgs.Empty);
                    Status = string.Format("Tick #{0}", ++tickCount);
                }

                /// This requires .Net 4.5
                await Task.Delay(Interval);
            }

            Status = "Stopped";
        }

        /// <summary>
        /// Start this timer.
        /// </summary>
        public void Start()
        {
            Status = "Starting";
            isInLoop = true;
            IsEnabled = true;
            StartAsync();
        }

        /// <summary>
        /// Stop this timer.
        /// </summary>
        public void Stop()
        {
            isInLoop = false;
            Status = "Stopping...";
        }
    }
}
