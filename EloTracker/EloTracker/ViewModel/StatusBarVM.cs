using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloTracker.ViewModel
{
    public class StatusBarVM : ViewModelBase
    {
        private string _status = "";

        public string Status
        {
            get { return _status; }
            set
            {
                if (Status != value)
                {
                    _status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }

        public string Version { get; }
        public string BugReportURL { get; }

        public StatusBarVM(string reportURL)
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version = string.Format("Version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion);
            }
            else
            {
                Version = "Undeployed Version";
            }

            BugReportURL = reportURL;
        }
    }
}
