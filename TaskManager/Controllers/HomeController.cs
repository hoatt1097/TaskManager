using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManager.Context;
using TaskManager.Extension;
using TaskManager.Helpers;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskManagerContext _context;
        public INotyfService _notyfService { get; }

        public HomeController(TaskManagerContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Index(string? userId, string? filter)
        {
            var currentUser = UserHelper.GetCurrentUser(HttpContext);

            // GET data to show filter
            var projectDB = _context.Projects.AsNoTracking().ToList();
            if (userId != null)
            {
                var projects = _context.Tasks.Where(m => m.PIC == userId || m.Assignee.Contains(userId))
                                               .Select(m => m.ProjectId)
                                               .Distinct().OrderBy(m => m)
                                               .ToList();
                projectDB = _context.Projects.Where(p => projects.Contains(p.Id)).ToList();
            }
            
            List<ProjectViewM> projectViews = new List<ProjectViewM>();
            foreach (var x in projectDB)
            {
                var newP = new ProjectViewM
                {
                    Id = x.Id,
                    Name = x.Name.Trim(),
                    Status = x.Status.Trim(),
                    Active = x.Active
                };
                projectViews.Add(newP);
            }
            ViewBag.projectViews = projectViews;



            var usertDB = _context.Users.AsNoTracking().OrderBy(n => n.Id).ToList();
            if (userId != null)
            {
                usertDB = _context.Users.Where(u => u.Id.ToString() == userId).OrderBy(n => n.Id).ToList();
            }
            List<UserViewM> userViews = new List<UserViewM>();
            foreach (var x in usertDB)
            {
                var newP = new UserViewM
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName.Trim(),
                    Active = x.Active
                };
                userViews.Add(newP);
            }
            ViewBag.userViews = userViews;

            var assigneeDB = _context.Users.Where(u => u.Active == true).OrderBy(n => n.Id).ToList();
            List<UserViewM> assignees = new List<UserViewM>();
            foreach (var x in assigneeDB)
            {
                var newP = new UserViewM
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName.Trim(),
                    Active = x.Active
                };
                assignees.Add(newP);
            }
            ViewBag.assignees = assignees;


            // Get filter
            ViewBag.filterViewM = new FilterViewM();
            FilterViewM filterViewM = null;
            if (filter != null)
            {
                var filterDB = _context.Settings.FirstOrDefault(a => a.Key == "FILTER" && a.UserId == int.Parse(currentUser.UserId));
                if (filterDB != null && filterDB.Value != null)
                {
                    filterViewM = JsonSerializer.Deserialize<FilterViewM>(filterDB.Value);
                    ViewBag.filterViewM = filterViewM;
                }
            }
            






            // Get table mode
            var tablMS = _context.Settings.FirstOrDefault(m => m.UserId.ToString() == currentUser.UserId && m.Key == "TABLE_MODE");
            if (tablMS == null)
            {
                ViewBag.tableMode = "Normal";
            } else
            {
                ViewBag.tableMode = tablMS.Value;
            }

            TableV tableV = new TableV();

            List<int> projectIdList = null;
            if (userId != null)
            {
                
                if (filter != null)
                {
                    projectIdList = _context.Tasks.Where(m => m.PIC == userId || m.Assignee.Contains(userId))
                                                .Select(m => m.ProjectId)
                                                .Distinct().OrderBy(m => m)
                                                .ToList();
                    if (filterViewM != null && filterViewM.Project != null && filterViewM.Project.First() != "all")
                    {
                        projectIdList = projectIdList.Where(a => filterViewM.Project.Contains(a.ToString())).ToList();
                    }
                }
                else 
                {
                    projectIdList = _context.Tasks.Where(m => m.PIC == userId || m.Assignee.Contains(userId))
                                                .Select(m => m.ProjectId)
                                                .Distinct().OrderBy(m => m)
                                                .ToList();
                    var projectClosedID = _context.Projects.Where(p => p.Status == "CLOSED").Select(m => m.Id).ToList();
                    projectIdList = projectIdList.Except(projectClosedID).ToList();
                }
                
            } 
            else
            {
                if (filter != null)
                {
                    projectIdList = _context.Projects.Select(m => m.Id).Distinct().ToList();
                    if (filterViewM != null && filterViewM.Project != null && filterViewM.Project.First() != "all")
                    {
                        projectIdList = filterViewM.Project.Select(int.Parse).ToList();
                    }
                }
                else
                {
                    projectIdList = _context.Projects.Where(p => p.Status != "CLOSED").Select(m => m.Id).Distinct().ToList();
                }
            }

            tableV.TotalPoject = projectIdList.Count();
            int totalTask = 0;

            List<TableProjectV> tableProjectVs = new List<TableProjectV>();
            foreach (var projectID in projectIdList)
            {
                var originProject = _context.Projects.FirstOrDefault(x => x.Id == projectID);
                if(originProject == null)
                {
                    continue;
                }

                var tableProjectV = new TableProjectV();
                List<TableTaskV> tableTaskVs = new List<TableTaskV>();

                List<Tasks> tasks = new List<Tasks>();
                if (userId == null)
                {
                    if (filter != null)
                    {
                        if (filterViewM == null || filterViewM.Assignee == null || filterViewM.Assignee.First() == "all")
                        {
                            if(filterViewM.TaskStatus == null)
                            {
                                tasks = _context.Tasks.Where(x => x.ProjectId == projectID 
                                                                && x.Status != "DONE")
                                                 .OrderBy(n => n.CreationDate).ToList();
                            }
                            if (filterViewM.TaskStatus != null && filterViewM.TaskStatus.First() == "all")
                            {
                                tasks = _context.Tasks.Where(x => x.ProjectId == projectID)
                                                 .OrderBy(n => n.CreationDate).ToList();
                            }
                            if (filterViewM.TaskStatus != null && filterViewM.TaskStatus.First() != "all")
                            {
                                tasks = _context.Tasks.Where(x => x.ProjectId == projectID 
                                                                && filterViewM.TaskStatus.Contains(x.Status))
                                                 .OrderBy(n => n.CreationDate).ToList();
                            }
                            
                        } 
                        else
                        {
                            if (filterViewM.TaskStatus == null)
                            {
                                tasks = _context.Tasks.Where(x => x.ProjectId == projectID && x.Status != "DONE")
                                                                .AsEnumerable()
                                                                .Where(x => filterViewM.Assignee.Contains(x.PIC) || filterViewM.Assignee.All(assignee => x.Assignee.Contains(assignee)))
                                                                .OrderBy(n => n.CreationDate).ToList();
                            }
                            if (filterViewM.TaskStatus != null && filterViewM.TaskStatus.First() == "all")
                            {
                                tasks = _context.Tasks.Where(x => x.ProjectId == projectID)
                                                    .AsEnumerable()
                                                    .Where(x => filterViewM.Assignee.Contains(x.PIC) || filterViewM.Assignee.All(assignee => x.Assignee.Contains(assignee)))
                                                    .OrderBy(n => n.CreationDate).ToList();
                            }
                            if (filterViewM.TaskStatus != null && filterViewM.TaskStatus.First() != "all")
                            {
                                tasks = _context.Tasks.Where(x => x.ProjectId == projectID && filterViewM.TaskStatus.Contains(x.Status))
                                                .AsEnumerable()
                                                .Where(x => filterViewM.Assignee.Contains(x.PIC) || filterViewM.Assignee.All(assignee => x.Assignee.Contains(assignee)))
                                                 .OrderBy(n => n.CreationDate).ToList();
                            }
                        }
                        
                    } 
                    else
                    {
                        tasks = _context.Tasks.Where(x => x.ProjectId == projectID && x.Status != "DONE")
                                            .OrderBy(n => n.CreationDate).ToList();
                    }
                }
                else
                {
                    if (filter != null)
                    {
                        if (filterViewM.TaskStatus == null)
                        {
                            tasks = _context.Tasks.Where(x => x.ProjectId == projectID
                                                            && (x.PIC == userId || x.Assignee.Contains(userId))
                                                            && x.Status != "DONE")
                                             .OrderBy(n => n.CreationDate).ToList();
                        }
                        if (filterViewM.TaskStatus != null && filterViewM.TaskStatus.First() == "all")
                        {
                            tasks = _context.Tasks.Where(x => x.ProjectId == projectID
                                                && (x.PIC == userId || x.Assignee.Contains(userId))
                                                )
                                             .OrderBy(n => n.CreationDate).ToList();
                        }
                        if (filterViewM.TaskStatus != null && filterViewM.TaskStatus.First() != "all")
                        {
                            tasks = _context.Tasks.Where(x => x.ProjectId == projectID
                                                            && (x.PIC == userId || x.Assignee.Contains(userId))
                                                            && filterViewM.TaskStatus.Contains(x.Status))
                                             .OrderBy(n => n.CreationDate).ToList();
                        }
                    }
                    else
                    {
                        tasks = _context.Tasks.Where(x => x.ProjectId == projectID
                                                    && (x.PIC == userId || x.Assignee.Contains(userId))
                                                    && x.Status != "DONE")
                                            .OrderBy(n => n.CreationDate).ToList();
                    }
                }
                
                foreach (var task in tasks)
                {
                    totalTask++;
                    var user = task.PIC != null ? _context.Users.FirstOrDefault(x => x.Id.ToString() == task.PIC) : null;

                    var newP = new TableTaskV
                    {
                        Index = totalTask,
                        Id = task.Id,
                        ProjectId = task.ProjectId,
                        ProjectName = task.ProjectName,
                        Subject = task.Subject,
                        Description = task.Description,
                        Action = task.Action,
                        HelpdeskTicket = task.HelpdeskTicket != null ? "#" + task.HelpdeskTicket.Replace("#", "") : "",
                        StatusCode = task.Status,
                        Status = Helpers.Enum.GetTaskStatusValue(task.Status),
                        Progress = task.Progress,
                        PicId = task.PIC,
                        Pic = user != null ? user.DisplayName : "",
                        Requestor = task.Requestor,
                        Assignee = task.Assignee,
                        StartDate = !string.IsNullOrEmpty(task.StartDate) ? DateTime.ParseExact(task.StartDate, "yyyy-MM-dd", null).ToString("MM/dd/yy") : string.Empty, 
                        DueDate = !string.IsNullOrEmpty(task.DueDate) ? DateTime.ParseExact(task.DueDate, "yyyy-MM-dd", null).ToString("MM/dd/yy") : string.Empty,
                        EstimateDate = !string.IsNullOrEmpty(task.EstimateDate) ? DateTime.ParseExact(task.EstimateDate, "yyyy-MM-dd", null).ToString("MM/dd/yy") : string.Empty,
                        UpdateTime  = Utilities.GetStringByFormat(task.LastUpdateDate, task.CreationDate),
                        Active = true
                    };
                    tableTaskVs.Add(newP);
                }

                
                tableProjectV.Id = projectID;
                tableProjectV.Name = originProject.Name;
                tableProjectV.Total = tasks.Count();
                tableProjectV.Tasks = tableTaskVs;

                if (tableProjectV.Total != 0)
                {
                    tableProjectVs.Add(tableProjectV);
                }
                
            }

            tableV.TotalTask = totalTask;
            tableV.Projects = tableProjectVs;

            ViewData["Project"] = new SelectList(_context.Projects.Where(x => x.Active == true), "Id", "Name");
            ViewData["PIC"] = new SelectList(_context.Users.Where(x => x.Active == true), "Id", "DisplayName");

            ViewBag.TableV = tableV;
            return View();
        }

        public IActionResult GetTaskDetail(string Id)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                var task = _context.Tasks.FirstOrDefault(x => x.Id.ToString() == Id);

                var user = (task != null && task.PIC != null) ? _context.Users.FirstOrDefault(x => x.Id.ToString() == task.PIC) : null;
                var tableTaskV = new TableTaskV
                {
                    Id = task.Id,
                    ProjectId = task.ProjectId,
                    ProjectName = task.ProjectName,
                    Subject = task.Subject,
                    Description = task.Description,
                    Action = task.Action,
                    HelpdeskTicket = task.HelpdeskTicket != null ? "#" + task.HelpdeskTicket.Replace("#", "") : "",
                    StatusCode = task.Status,
                    Status = Helpers.Enum.GetTaskStatusValue(task.Status),
                    Progress = task.Progress,
                    PicId = task.PIC,
                    Pic = user != null ? user.DisplayName : "",
                    Requestor = task.Requestor,
                    Assignee = task.Assignee,
                    AssigneeName = task.Assignee != null ? JsonSerializer.Deserialize<List<string>>(task.Assignee) : new List<string>(),
                    StartDate = task.StartDate,
                    DueDate = task.DueDate,
                    EstimateDate = task.EstimateDate,
                    LastestUpdateTime = task.LastUpdateDate != null ? "Latest update: " + task.LastUpdateDate?.ToString("yyyy/MM/dd HH:mm:ss") : "Latest update: ",
                    Active = true
                };

                return Json(new { success = true, message = "Successfully!", tableTaskV = tableTaskV });
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, message = "Exist Project!", tableTaskV = "" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something is error", tableTaskV = "" });
            }

        }

        [HttpPost]
        public IActionResult AddOrUpdateProject(ProjectViewM projectViewM)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                var action = projectViewM.Action;

                var project = _context.Projects.FirstOrDefault(m => m.Name.ToUpper() == projectViewM.Name.ToUpper());

                if (action == "CREATE")
                {
                    if (project != null)
                    {
                        return Json(new { success = false, message = $"Project exist!" });
                    }
                    else
                    {
                        Projects newProject = new Projects
                        {
                            Name = projectViewM.Name,
                            Status = projectViewM.Status,
                            Active = true,
                            CreationDate = DateTime.Now,
                            CreatedBy = int.Parse(currentUser.UserId)
                        };
                        _context.Projects.Add(newProject);
                    }
                    
                } 
                else 
                {
                    if (project == null)
                    {
                        return Json(new { success = false, message = $"Project not exist!" });
                    }
                    if (action == "UPDATE")
                    {
                        project.Status = projectViewM.Status;
                        _context.Projects.Update(project);
                    }
                    else if (action == "DELETE")
                    {
                        var task = _context.Tasks.Where(x => x.ProjectId == project.Id).ToList();
                        if (task.Count > 0)
                        {
                            return Json(new { success = false, message = "Can not Delete. Project has task" });
                        } else
                        {
                            _context.Projects.Remove(project);
                        }
                        
                    }
                }
                

                _context.SaveChanges();
                return Json(new { success = true, message = "Successfully!" });
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false,  message = "Exist Project!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something is error" });
            }
        }

        public List<ProjectViewM> GetProjects(string searchText, string type)
        {
            var projectDB = _context.Projects.AsNoTracking().ToList();
            List<ProjectViewM> projectViews = new List<ProjectViewM>();
            foreach (var x in projectDB)
            {
                var newP = new ProjectViewM
                {
                    Id = x.Id,
                    Name = x.Name.Trim(),
                    Status = x.Status.Trim()
                };
                projectViews.Add(newP);
            }

            if (type == "dropdown" || searchText == "" || searchText == "a" || searchText == "all" || searchText == "*")
            {
                return projectViews;
            }
            else
            {
                var data = projectViews.Where(x => x.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .Take(10).ToList();
                return data;
            }
           
        }

        public List<DropdownView> GetUsers()
        {
            var users = _context.Users.Where(x => x.Active == true).OrderBy(n => n.Id).ToList();
            List<DropdownView> views = new List<DropdownView>();
            foreach (var x in users)
            {
                var newP = new DropdownView
                {
                    Id = x.Id,
                    Name = x.DisplayName.Trim()
                };
                views.Add(newP);
            }
            return views;
        }



        public IActionResult CreateOrUpdateTask(TaskViewM task, string action)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                if (action == "NEW")
                {
                    var project = _context.Projects.FirstOrDefault(m => m.Id == task.ProjectId);
                    Tasks newTask = new Tasks
                    {
                        ProjectId = task.ProjectId,
                        ProjectName = project.Name,
                        Subject = task.Subject,
                        Description = task.Description,
                        Action = task.Action,
                        HelpdeskTicket = task.HelpdeskTicket,
                        Status = task.Status,
                        Progress = task.Progress,
                        PIC = task.Pic,
                        Requestor = task.Requestor,
                        Assignee = JsonSerializer.Serialize(task.Assignee),
                        StartDate = task.StartDate,
                        DueDate = task.DueDate,
                        EstimateDate = task.EstimateDate,
                        Active = true,
                        CreationDate = DateTime.Now,
                        CreatedBy = int.Parse(currentUser.UserId),
                        LastUpdateDate = null,
                        LastUpdatedBy = null
                    };
                    _context.Tasks.Add(newTask);
                    _context.SaveChanges();
                    task.Id = newTask.Id;
                } 
                else {
                    var updateTask = _context.Tasks.FirstOrDefault(x => x.Id == task.Id);

                    if (updateTask == null)
                    {
                        return Json(new { success = false, message = $"Task not exist!" });
                    }

                    if (action == "UPDATE")
                    {
                        var project = _context.Projects.FirstOrDefault(m => m.Id == task.ProjectId);

                        updateTask.ProjectId = task.ProjectId;
                        updateTask.ProjectName = project.Name;
                        updateTask.Subject = task.Subject;
                        updateTask.Description = task.Description;
                        updateTask.Action = task.Action;
                        updateTask.HelpdeskTicket = task.HelpdeskTicket;
                        updateTask.Status = task.Status;
                        updateTask.Progress = task.Progress;
                        updateTask.PIC = task.Pic;
                        updateTask.Requestor = task.Requestor;
                        updateTask.Assignee = JsonSerializer.Serialize(task.Assignee);
                        updateTask.StartDate = task.StartDate;
                        updateTask.DueDate = task.DueDate;
                        updateTask.EstimateDate = task.EstimateDate;
                        updateTask.Active = true;
                        updateTask.LastUpdateDate = DateTime.Now;
                        updateTask.LastUpdatedBy = int.Parse(currentUser.UserId);
                        _context.Tasks.Update(updateTask);
                        _context.SaveChanges();
                        task.Id = updateTask.Id;
                    }
                    else
                    {
                        _context.Tasks.Remove(updateTask);
                        _context.SaveChanges();
                        task.Id = updateTask.Id;
                    }
                }
                
                return Json(new { success = true, message = "Successfully!", task = task });
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, message = "Exist Project!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something is error" });
            }
        }

        public IActionResult ChangeTableMode()
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                var setting = _context.Settings.FirstOrDefault(m => m.UserId.ToString() == currentUser.UserId && m.Key == "TABLE_MODE");

                if(setting == null)
                {
                    var newSetting = new Settings
                    {
                        UserId = int.Parse(currentUser.UserId),
                        Key = "TABLE_MODE",
                        Value = "Lite"
                    };
                    _context.Settings.Add(newSetting);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Successfully!", mode = "Lite" });
                } 
                else
                {
                    if(setting.Value == "Lite")
                    {
                        setting.Value = "Normal";
                    } 
                    else
                    {
                        setting.Value = "Lite";
                    }
                    _context.Settings.Update(setting);
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Successfully!", mode = setting.Value });
                }
                
                
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something is error" });
            }
        }

        public IActionResult UpdatePIC(string taskID, string assignID)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                var task = _context.Tasks.Where(t => t.Id == int.Parse(taskID)).FirstOrDefault();

                if (task != null)
                {
                    task.PIC = assignID;
                    task.LastUpdateDate = DateTime.Now;
                    task.LastUpdatedBy = int.Parse(currentUser.UserId);
                    _context.Tasks.Update(task);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Something is error" });
                }


            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something is error" });
            }
        }

        public IActionResult UpdateTaskStatus(string taskID, string status)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                var task = _context.Tasks.Where(t => t.Id == int.Parse(taskID)).FirstOrDefault();

                if (task != null)
                {
                    task.Status = status;
                    task.LastUpdateDate = DateTime.Now;
                    task.LastUpdatedBy = int.Parse(currentUser.UserId);
                    _context.Tasks.Update(task);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Something is error" });
                }


            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something is error" });
            }
        }

        public IActionResult UpdateFilter(FilterViewM filterViewM, string action)
        {
            try
            {
                var currentUser = UserHelper.GetCurrentUser(HttpContext);
                var setting = _context.Settings.FirstOrDefault(m => m.UserId.ToString() == currentUser.UserId && m.Key == "FILTER");

                if (action == "CLEAR")
                {
                    if (setting == null)
                    {
                        return Json(new { success = true, message = "Successfully!" });
                    } 
                    else
                    {
                        setting.Value = null;
                        _context.Settings.Update(setting);
                        _context.SaveChanges();
                        return Json(new { success = true, message = "Successfully!" });
                    }
                } 
                else
                {
                    string filterString = JsonSerializer.Serialize(filterViewM);
                    if (setting == null)
                    {
                        setting = new Settings
                        {
                            UserId = int.Parse(currentUser.UserId),
                            Key = "FILTER",
                            Value = filterString
                        };
                        _context.Settings.Add(setting);
                        _context.SaveChanges();
                        return Json(new { success = true, message = "Successfully!" });
                    }
                    else
                    {
                        setting.Value = filterString;
                        _context.Settings.Update(setting);
                        _context.SaveChanges();
                        return Json(new { success = true, message = "Successfully!" });
                    }
                }

                return Json(new { success = false, message = "Something is error!" });

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something is error" });
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
