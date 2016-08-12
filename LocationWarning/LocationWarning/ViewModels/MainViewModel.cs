using System;


namespace LocationWarning.ViewModels
{
    public class MainViewModel : AbstractViewModel
    {
        public MainViewModel(InfoViewModel info, HistoryViewModel history, ErrorListViewModel errors)
        {
            this.Info = info;
            this.History = history;
            this.ErrorList = errors;
        }


        public override void OnActivate()
        {
            base.OnActivate();
            this.Info.OnActivate();
            this.History.OnActivate();
            this.ErrorList.OnActivate();
        }


        public override void OnDeactivate()
        {
            base.OnDeactivate();
            this.Info.OnDeactivate();
            this.History.OnDeactivate();
            this.ErrorList.OnDeactivate();
        }


        public InfoViewModel Info { get; }
        public HistoryViewModel History { get; }
        public ErrorListViewModel ErrorList { get; }
    }
}
