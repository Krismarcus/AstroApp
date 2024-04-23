using AstroApp.Data.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data.Models
{
    public partial class PlanetInRetrogradeDetails : ObservableObject
    {
        [ObservableProperty]
        private Planet planetInRetrograde;
        [ObservableProperty]
        private string planetInRetrogradeInfo;
    }
}
