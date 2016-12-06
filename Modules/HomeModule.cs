using Nancy;
using System.Collections.Generic;
using Nancy.ViewEngines.Razor;
using ToDoList.Objects;

namespace ToDoList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
        {
          Get["/"] = _ => {
            List<Category> AllCategories = Category.GetAll();
            return View["index.cshtml"];
          };
          Get["/categories"] = _ => {
            var allCategories = Category.GetAll();
            return View["categories.cshtml", allCategories];
          };
          Get["/tasks"] = _ => {
            List<Task> AllTasks = Task.GetAll();
            return View ["tasks.cshtml", AllTasks];
          };
          Get["/categories/new"] = _ => {
            return View["category_form.cshtml"];
          };
          Post["/categories/new"] = _ => {
            Category newCategory = new Category(Request.Form["category-name"]);
            newCategory.Save();
            return View["success.cshtml"];
          };
          Get["/tasks/new"] = _ => {
            List<Category> AllCategories = Category.GetAll();
            return View["tasks_form.cshtml", AllCategories];
          };
          Post["/tasks/new"] = _ => {
            Task newTask = new Task(Request.Form["task-description"], Request.Form["category-id"], Request.Form["due-date"]);
            newTask.Save();
            return View["success.cshtml"];
          };
          Post["/tasks/delete"] = _ => {
            Task.DeleteAll();
            return View["cleared.cshtml"];
          };
          Get["/categories/{id}"] = parameters => {
            Dictionary<string, object> model = new Dictionary<string, object>();
            var selectedCategory = Category.Find(parameters.id);
            var categoryTasks = selectedCategory.GetTasks();
            model.Add("category", selectedCategory);
            model.Add("tasks", categoryTasks);
            return View["category.cshtml", model];
          };

          Get["/categories/{id}/tasks/new"] = parameters => {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(parameters.id);
            List<Task> allTasks = selectedCategory.GetTasks();
            model.Add("category", selectedCategory);
            model.Add("tasks", allTasks);
            return View["category_tasks_form.cshtml", model];
          };
          Post["/tasks"] = _ => {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(Request.Form["category-id"]);
            List<Task> categoryTasks = selectedCategory.GetTasks();
            string taskDescription = Request.Form["task-description"];
            var dueDate = Request.Form["due-date"];
            Task newTask = new Task(taskDescription, dueDate);
            categoryTasks.Add(newTask);
            model.Add("tasks", categoryTasks);
            model.Add("category", selectedCategory);
            return View["category.cshtml", model];
          };
        }
      }
    }
