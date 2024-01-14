using AstroApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.UI.ViewModels
{
    public class AstroEventsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<AstroEvent> activeAstroEvents;
        public ObservableCollection<AstroEvent> ActiveAstroEvents
        {
            get => activeAstroEvents;
            set
            {
                if (activeAstroEvents != value)
                {
                    activeAstroEvents = value;
                    OnPropertyChanged(nameof(ActiveAstroEvents));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AstroEventsViewModel()
        {
            ActiveAstroEvents = new ObservableCollection<AstroEvent>();
            // Initialize your collection here
        }
    }
}
