using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Task.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      Task firstTask = new Task("Mow the lawn", DateTime.Today);
      Task secondTask = new Task("Mow the lawn", DateTime.Today);

      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      Task testTask = new Task("Mow the lawn", DateTime.Today);

      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Task testTask = new Task("Mow the lawn", DateTime.Today);

      testTask.Save();
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      Task testTask = new Task("Mow the lawn", DateTime.Today);
      testTask.Save();
      Task foundTask = Task.Find(testTask.GetId());

      Assert.Equal(testTask, foundTask);
    }

    [Fact]
    public void Test_AddCategory_AddsCategoryToTask()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", DateTime.Today);
      testTask.Save();

      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      //Act
      testTask.AddCategory(testCategory);

      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_GetCategories_ReturnsAllTaskCategories()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", DateTime.Today);
      testTask.Save();

      Category testCategory1 = new Category("Home stuff");
      testCategory1.Save();

      Category testCategory2 = new Category("Work stuff");
      testCategory2.Save();

      //Act
      testTask.AddCategory(testCategory1);
      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category> {testCategory1};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Delete_DeletesTaskAssociationsFromDatabase()
    {
      //Arrange
      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      string testDescription = "Mow the lawn";
      DateTime testDateTime = DateTime.Today;
      Task testTask = new Task(testDescription, testDateTime);
      testTask.Save();

      //Act
      testTask.AddCategory(testCategory);
      testTask.Delete();

      List<Task> resultCategoryTasks = testCategory.GetTasks();
      List<Task> testCategoryTasks = new List<Task> {};

      //Assert
      Assert.Equal(testCategoryTasks, resultCategoryTasks);
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }
  }
}
