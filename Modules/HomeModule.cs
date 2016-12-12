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
          Get["category/edit/{id}"] = parameters => {
            Category SelectedCategory = Category.Find(parameters.id);
            return View["category_edit.cshtml", SelectedCategory];
          };
          Patch["category/edit/{id}"] = parameters => {
            Category SelectedCategory = Category.Find(parameters.id);
            SelectedCategory.Update(Request.Form["category-name"]);
            return View["success.cshtml"];
          };
          Get["/tasks/new"] = _ => {
            List<Category> AllCategories = Category.GetAll();
            return View["tasks_form.cshtml", AllCategories];
          };
          Post["/tasks/new"] = _ => {
            Task newTask = new Task(Request.Form["task-description"], Request.Form["due-date"]);
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
            var AllTasks = Task.GetAll();
            model.Add("category", selectedCategory);
            model.Add("categoryTasks", categoryTasks);
            model.Add("allTasks", AllTasks);
            return View["category.cshtml", model];
          };
          Get["tasks/{id}"] = parameters => {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Task SelectedTask = Task.Find(parameters.id);
            List<Category> taskCategories = SelectedTask.GetCategories();
            List<Category> allCategories = Category.GetAll();
            model.Add("task", SelectedTask);
            model.Add("taskCategories", allCategories);
            model.Add("allCategories", allCategories);
            return View["task.cshtml", model];
          };
          Post["task/add_category"] = _ => {
            Category category = Category.Find(Request.Form["category-id"]);
            Task task = Task.Find(Request.Form["task-id"]);
            task.AddCategory(category);
            return View["success.cshtml"];
          };
          Post["category/add_task"] = _ => {
            Category category = Category.Find(Request.Form["category-id"]);
            Task task = Task.Find(Request.Form["task-id"]);
            category.AddTask(task);
            return View["success.cshtml"];
          };
        }
      }
    }
