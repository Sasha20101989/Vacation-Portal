

namespace Vacation_Portal.MVVM.Models
{
    public enum InviteResponseKind
    {
        /// <summary>
        /// The user has not yet accepted or declined.
        /// </summary>
        None,
        /// <summary>
        /// Invitation accepted; the user plans to attend.
        /// </summary>
        Accepted,
        /// <summary>
        /// Invitation declined; the user does not plan to attend.
        /// </summary>
        Declined
    }
}