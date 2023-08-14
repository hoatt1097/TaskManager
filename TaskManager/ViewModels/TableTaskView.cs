using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace TaskManager.ViewModels
{
    public class TableV
    {
        public int TotalPoject { get; set; }
        public int TotalTask { get; set; }
        public List<TableProjectV> Projects { get; set; }
    }

    public class TableProjectV
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
        public List<TableTaskV> Tasks { get; set; }
    }


    public class TableTaskV
    {
        public int Index { get; set; }
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
        public string PicId { get; set; }
        public string Pic { get; set; }
        public bool Active { get; set; }
        public string Requestor { get; set; }
        public string Assignee { get; set; }
        public List<string> AssigneeName { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public string EstimateDate { get; set; }
        public string CreationDate { get; set; }
        public string UpdateTime { get; set; }
    }
}
