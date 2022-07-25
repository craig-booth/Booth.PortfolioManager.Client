using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

using Booth.WpfControls;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels
{
    class LogonViewModel : PopupWindow
    {
        private readonly RestClient _RestClient;
        public string UserName { get; set; }
        public string Password { get; set; }

        public LogonViewModel(RestClient restClient)
        {
            _RestClient = restClient;
            LogonCommand = new RelayCommand(Logon);
            CancelCommand = new RelayCommand(Cancel);

            Commands.Add(new DialogCommand("Logon", LogonCommand) { IsDefault = true });
            Commands.Add(new DialogCommand("Cancel", CancelCommand) { IsCancel = true });
        }

        public RelayCommand LogonCommand { get; private set; }
        private async void Logon()
        {
            await _RestClient.Authenticate(UserName, Password);

            _RestClient.SetPortfolio(new Guid("5d5de669-726c-4c5d-bb2e-6520c924db90"));

            Close();

            OnLoggedOn(EventArgs.Empty);

        }

        public RelayCommand CancelCommand { get; private set; }
        private void Cancel()
        {
            Close();
        }

        public event EventHandler LoggedOn;

        private void OnLoggedOn(EventArgs e)
        {
            LoggedOn?.Invoke(this, e);
        }
    }
}
