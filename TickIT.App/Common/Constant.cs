using System;
using System.IO;


namespace TickIT.App.Common
{
    public static class Constant
    {
        public const string Create = "Create";
        public const string Update = "Update";
        public const string ConfirmDeleteMsg = "Are you sure you want to delete the Ticket?";
        public const string ConfirmDeleteWinTitle = "Delete Ticket?";
        public const string DeleteFailedMsg = "Couldn't delete the Ticket. Please try again";
        public const string DeleteFailedWinTitle = "Delete Unsuccessful";
        public const string Database = "Database";
        public const string Sqlite = "Sqlite";
        public const string AzureSql = "AzureSql";
        public const string ChangeDbTitleMsg = "Database Changed!";
        public const string DbChangeResultsIn = "This action results in switching to ";
        public const string ConfirmChangeDb = "Are you sure you want to change the database?";
        public const string Today = "Today";
        public const string Tomorrow = "Tomorrow";
        public const string TicketCompletedWinTitle = "Ticket Completed?";
        public const string TicketCompletedMsg = "Once a Ticket moved to Completed stage,it can't be brought back. are you sure that you want to move it to completed stage?";
        public const string Overdue = "Overdue";
        public const string ErrorOccured = "Error Occured";
        public const string DbSwitchSuccessMsg = "Successfully switched to ";
        public const string TicketDue = "Ticket due";
        public static Uri IconPath = new(Path.GetFullPath(@"Images/app_icon.ico"));
        public const string UpdateFailed = "Ticket can't be updated";
        public const string DefaultBrowserRegistryKeyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice";
        public const string GitHubRepoUrl = "https://github.com/nagababuthota984/ToDoTaskManager";
        public const string ProgId = "ProgId";
        public const string IEBrowserRegistryKeyName = @"HKEY_CLASSES_ROOT\https\shell\open\command";
        public static string ProjectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        public const string NoActiveInternet = "Make sure you have active internet connection";
        public const string AzureConnectionFailed = "Unable to connect to azure services";

    }
}
