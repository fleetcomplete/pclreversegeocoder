using System;
using System.Reactive.Linq;
using Acr.Notifications;
using Autofac;
using LocationWarning.Services;


namespace LocationWarning.Tasks
{
    public class NotificationTask : IStartable
    {
        readonly ILocationService location;
        readonly INotifications notifications;


        public NotificationTask(ILocationService location, INotifications notifications)
        {
            this.location = location;
            this.notifications = notifications;
        }


        public void Start()
        {
            this.location
                .WhenGeocodeAvailable()
                .Skip(1)
                .Subscribe(x =>
                {
                    this.notifications.Badge = this.notifications.Badge + 1;
                    this.notifications.Send(new Notification
                    {
                        Title = "Welcome",
                        Message = $"You have just arrived at {x.City}, {x.State}"
                        //Sound = ""
                    });
                });
        }
    }
}
