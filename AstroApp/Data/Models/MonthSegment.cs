using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data.Models
{
    public partial class MonthSegment : ObservableObject
    {
        [ObservableProperty]
        DateTime monthStartDate;
        [ObservableProperty]
        DateTime monthEndDate;
        [ObservableProperty]
        private int duration;
    }
}
