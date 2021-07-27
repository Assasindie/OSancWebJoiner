using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OSancJoiner
{
    public class JoinerViewModel : INotifyPropertyChanged
    {
        private string _accessToken;
        public string accessToken
        {
            get
            {
                return _accessToken;
            }
            set
            {
                this._accessToken = value;
                OnPropertyChanged();
            }
        }
        private string _raidId;
        public string raidId
        {
            get
            {
                return _raidId;
            }set
            {
                this._raidId = value;
                OnPropertyChanged();
            }
        }

        private string _reactId;
        public string reactId
        {
            get
            {
                return _reactId;
            }
            set
            {
                this._reactId = value;
                OnPropertyChanged();
            }
        }

        private string _responseText;
        public string responseText
        {
            get
            {
                return _responseText;
            }
            set
            {
                this._responseText = value;
                OnPropertyChanged();
            }
        }

        private bool _isTryingtoJoin;
        public bool isTryingtoJoin
        {
            get
            {
                return _isTryingtoJoin;
            }
            set
            {
                this._isTryingtoJoin = value;
                OnPropertyChanged();
            }
        }

        public ICommand joinRaidCommand { get; set; }
        public ICommand stopJoinRaidCommand { get; set; }
        public ICommand saveInfoCommand { get; set; }
        public ICommand loadInfoCommand { get; set; }
        private ApiRequestService apiService;

        public JoinerViewModel()
        {
            this.apiService = new ApiRequestService(new System.Net.Http.HttpClient());
            this.joinRaidCommand = new DelegateCommand(async () => await AttemptJoinRaid());
            this.stopJoinRaidCommand = new DelegateCommand(async () => await stopJoinRaid());
            this.saveInfoCommand = new DelegateCommand(async () => await saveInfo());
            this.loadInfoCommand = new DelegateCommand(async () => await loadInfo());
            loadInfoCommand.Execute(null);
            if (this.reactId == null || this.reactId == "")
            {
                this.reactId = "28"; //inc
            }
            this.isTryingtoJoin = false;
        }

        public async Task loadInfo()
        {
            OSancRequest data = await FileHandler.LoadInfoFromFile();
            this.accessToken = data.accessToken;
            this.raidId = data.raidId.ToString();
            this.reactId = data.reactId.ToString();
        }

        public async Task saveInfo()    
        {
            await FileHandler.SaveInfoToFile(generateRequest());
        }

        public async Task stopJoinRaid()
        {
            this.isTryingtoJoin = false;
        }

        public async Task AttemptJoinRaid()
        {
            int count = 0;
            this.isTryingtoJoin = true;
            while (isTryingtoJoin)
            {
                try
                {
                    OSancRequest request = generateRequest();
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                    string response = await apiService.MakeGenericRequest("joinRaid", "POST", data);
                    this.responseText = await apiService.MakeGenericRequest("reactRaid", "POST", data);
                }
                catch (Exception e)
                {
                    this.isTryingtoJoin = false;
                    this.responseText = e.Message;
                }
                count++;
                if(count > 20)
                {
                    this.isTryingtoJoin = false;
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private OSancRequest generateRequest()
        {
            try
            {
                return new OSancRequest()
                {
                    accessToken = this.accessToken,
                    raidId = int.Parse(this.raidId),
                    reactId = int.Parse(this.reactId)

                };
            }catch(Exception e)
            {
                this.responseText = e.Message;
                return new OSancRequest();
            }
        }
    }
}
