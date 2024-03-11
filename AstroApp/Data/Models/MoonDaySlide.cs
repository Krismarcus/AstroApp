using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data.Models
{
    public partial class MoonDaySlide : ObservableObject
    {
        [ObservableProperty]
        private int moonDay;
        [ObservableProperty]
        private string moonDayInfo;
        [ObservableProperty]
        private DateTime transitionTime;
    }
}
