using System.Collections.Generic;

namespace TaskManager.ViewModels
{
    public class TaskViewM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string HelpdeskTicket { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string Progress { get; set; }
        public string Pic { get; set; }
        public bool Active { get; set; }
        public string Requestor { get; set; }
        public List<string> Assignee { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public string EstimateDate { get; set; }
    }
}
