namespace BusinessLogicLayer.Helper;

public class NotificationMessage
{
    // Success Messages
    public const string CreateSuccess = "{0} added successfully.";
    public const string UpdateSuccess = "{0} updated successfully.";
    public const string DeleteSuccess = "{0} deleted successfully.";

    // Failure Messages
    public const string EmailAlreadyExists = "Email already exists. Please use a different email address.";
    public const string AttendanceAlreadyExists = "Attendance already exists for this Day.";
    public const string CreateFailure = "Failed to add {0}. Please try again.";
    public const string UpdateFailure = "Failed to update {0}. Please try again.";
    public const string DeleteFailure = "Failed to delete {0}. Please try again.";

}
