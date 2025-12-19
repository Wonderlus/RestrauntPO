using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Restraunt.Messages
{
    /// <summary>
    /// Message for navigating to a specific page
    /// </summary>
    public class NavigateToPageMessage : ValueChangedMessage<string>
    {
        public NavigateToPageMessage(string pageName) : base(pageName) { }
    }

    /// <summary>
    /// Message for logout action
    /// </summary>
    public class LogoutMessage { }
}
