using Nancy;
using System.Collections.Generic;
using Nancy.ViewEngines.Razor;


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
            return View["categories_form.cshtml"];
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
        }
      }
    }
