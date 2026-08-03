using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models;

namespace TaskFlow.Controllers;

public class HomeController : Controller
{
    private readonly TaskFlowDbContext _context;

    public HomeController(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search, TaskState? status)
    {
        var tasks = _context.Tasks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.Trim();
            tasks = tasks.Where(task =>
                task.Title.Contains(normalized) ||
                (task.Description != null && task.Description.Contains(normalized)) ||
                (task.Assignee != null && task.Assignee.Contains(normalized)));
        }

        if (status.HasValue)
        {
            tasks = tasks.Where(task => task.Status == status.Value);
        }

        var allTasks = await tasks
            .OrderByDescending(task => task.Priority)
            .ThenBy(task => task.DueDate ?? DateTime.MaxValue)
            .ToListAsync();

        var viewModel = new TaskDashboardViewModel
        {
            Tasks = allTasks,
            SearchTerm = search,
            SelectedStatus = status,
            TotalTasks = allTasks.Count,
            InProgressCount = allTasks.Count(task => task.Status == TaskState.InProgress),
            CompletedCount = allTasks.Count(task => task.Status == TaskState.Completed),
            OverdueCount = allTasks.Count(task =>
                task.Status != TaskState.Completed && task.DueDate.HasValue && task.DueDate.Value.Date < DateTime.UtcNow.Date)
        };

        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View(new TaskItem());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskItem task)
    {
        if (!ModelState.IsValid)
        {
            return View(task);
        }

        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Task '{task.Title}' was created successfully.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskItem task)
    {
        if (id != task.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(task);
        }

        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask is null)
        {
            return NotFound();
        }

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.Priority = task.Priority;
        existingTask.Status = task.Status;
        existingTask.Assignee = task.Assignee;
        existingTask.DueDate = task.DueDate;
        existingTask.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = $"Task '{existingTask.Title}' was updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Task '{task.Title}' was deleted.";

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
