﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerOfControl.Models;
using PowerOfControl.Services;

namespace PowerOfControl.Controllers;

[Produces("application/json")]
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly TaskService _tasksService;

    public TasksController(TaskService tasksService)
    {
        _tasksService = tasksService;
    }

    // POST: api/Tasks/AddTask
    [Authorize]
    [HttpPost]
    public IActionResult AddTask([FromBody] TaskData task)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Attempt to add a new task
        if (_tasksService.CreateTask(task))
        {
            return Ok(new { message = "Task successfully added" });
        }
        else
        {
            return BadRequest(new { message = "Error adding task" });
        }
    }

    // POST: api/Tasks/GetNotArchivedTasks
    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetNotArchivedTasks()
    {
        // Attempt to update the user
        var currentUserID = User.FindFirst("id")?.Value;

        if (currentUserID != null)
        {
            List<TaskDataDto> tasks = _tasksService.GetNotArchivedTasks(int.Parse(currentUserID));
            // Attempt to add a new task
            if (tasks != null)
            {
                return Ok(new { message = "Task data get successful", tasksList = tasks });
            }
            else
            {
                return Ok(new { message = "There are no tasks" });
            }
        }
        else
        {
            return Ok(new { message = "User not authorized" });
        }
    }

    // POST: api/Tasks/GetArchivedTasks
    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetArchivedTasks()
    {
        // Attempt to update the user
        var currentUserID = User.FindFirst("id")?.Value;

        List<TaskData> tasks = _tasksService.GetArchivedTasks(int.Parse(currentUserID));

        // Attempt to add a new task
        if (tasks != null)
        {
            return Ok(new { message = "Task data get successful", tasksList = tasks });
        }
        else
        {
            return Ok(new { message = "There are no tasks" });
        }
    }

    // POST: api/Tasks/UpdateTask
    [AllowAnonymous]
    [HttpPatch]
    public IActionResult UpdateTask([FromBody] TaskUpdateDto request)
    {
        // Attempt to add a new task
        if (_tasksService.UpdateTask(request))
        {
            return Ok(new { message = "Task update successfully" });
        }
        else
        {
            return BadRequest(new { message = "Task update failed" });
        }
    }

    // POST: api/Tasks/UpdateTaskPin
    [AllowAnonymous]
    [HttpPatch]
    public IActionResult UpdateTaskPin([FromBody] TaskStatusUpdateDto request)
    {
        // Attempt to add a new task
        if (_tasksService.UpdatePin(request))
        {
            return Ok(new { message = "Task pin update successfully" });
        }
        else
        {
            return BadRequest(new { message = "Task pin update failed" });
        }
    }

    // POST: api/Tasks/CopyTask
    [AllowAnonymous]
    [HttpPost]
    public IActionResult CopyTask([FromBody] TaskStatusUpdateDto request)
    {
        // Attempt to add a new task
        if (_tasksService.CopyTask(request.id))
        {
            return Ok(new { message = "Task copy successfully" });
        }
        else
        {
            return BadRequest(new { message = "Task copy failed" });
        }
    }

    // POST: api/Tasks/ArchiveTask
    [AllowAnonymous]
    [HttpPatch]
    public IActionResult ArchiveTask([FromBody] TaskStatusUpdateDto request)
    {
        // Attempt to add a new task
        if (_tasksService.ArchiveTask(request))
        {
            return Ok(new { message = "Task archive successfully" });
        }
        else
        {
            return BadRequest(new { message = "Task archive failed" });
        }
    }

    // POST: api/Tasks/DeleteTask
    [AllowAnonymous]
    [HttpDelete]
    public IActionResult DeleteTask([FromBody] TaskStatusUpdateDto request)
    {
        // Attempt to add a new task
        if (_tasksService.DeleteTask(request.id))
        {
            return Ok(new { message = "Task deleted successfully" });
        }
        else
        {
            return BadRequest(new { message = "Task delete failed" });
        }
    }
}