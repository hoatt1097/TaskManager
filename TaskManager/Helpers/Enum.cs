namespace TaskManager.Helpers
{
    public class Enum
    {

        public static string GetTaskStatusValue(string status)
        {
            switch (status)
            {
                case "OPEN":
                    return "Open";
                case "IN PROGRESS":
                    return "In Progress";
                case "WAITING APPROVAL":
                    return "Waiting Approval";
                case "WAITING USER":
                    return "Waiting User";
                case "CLOSE":
                    return "Close";
                case "DONE":
                    return "Done";
                default:
                    return "";
            }
        }
    }
}
