using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
  {
    public class CategoryTest : IDisposable
    {
      public CategoryTest()
      {
        DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
      }

      [Fact]
      public void Test_CategoriesEmptyAtFirst()
      {
        int result = Category.GetAll().Count;

        Assert.Equal(0, result);
      }

      [Fact]
      public void Test_Equal_ReturnsTrueForSameName()
      {
        Category firstCategory = new Category("Household chores");
        Category secondCategory = new Category("Household chores");

        Assert.Equal(firstCategory, secondCategory);
      }

      [Fact]
      public void Test_Save_SavesCategoryToDatabase()
      {
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        List<Category> result = Category.GetAll();
        List<Category> testList = new List<Category>{testCategory};

        Assert.Equal(testList, result);
      }

      [Fact]
      public void Test_Find_FindsCategoryInDatabase()
      {
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        Category foundCategory = Category.Find(testCategory.GetId());

        Assert.Equal(testCategory, foundCategory);
      }

      [Fact]
      public void Test_GetTasks_RetrievesAllTasksWithCategory()
      {
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        Task firstTask = new Task("Mow the lawn", DateTime.Today);
        firstTask.Save();
        Task secondTask = new Task("Do the dishes", DateTime.Today);
        secondTask.Save();

        List<Task> testTaskList = new List<Task> {firstTask, secondTask};
        List<Task> resultTaskList = testCategory.GetTasks();
      }
      [Fact]
      public void Test_Update_UpdatesCategoryInDatabase()
      {
        //Arrange
        string name = "Home stuff";
        Category testCategory = new Category(name);
        testCategory.Save();
        string newName = "Work stuff";

        //Act
        testCategory.Update(newName);

        string result = testCategory.GetName();

        //Assert
        Assert.Equal(newName, result);
      }

      [Fact]
    public void Test_AddTask_AddsTaskToCategory()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask = new Task("Mow the lawn", DateTime.Today);
      testTask.Save();

      Task testTask2 = new Task("Water the garden", DateTime.Today);
      testTask2.Save();

      //Act
      testCategory.AddTask(testTask);
      testCategory.AddTask(testTask2);

      List<Task> result = testCategory.GetTasks();
      List<Task> testList = new List<Task>{testTask, testTask2};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_GetTasks_ReturnsAllCategoryTasks()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask1 = new Task("Mow the lawn", DateTime.Today);
      testTask1.Save();

      Task testTask2 = new Task("Buy plane ticket", DateTime.Today);
      testTask2.Save();

      //Act
      testCategory.AddTask(testTask1);
      List<Task> savedTasks = testCategory.GetTasks();
      List<Task> testList = new List<Task> {testTask1};

      //Assert
      Assert.Equal(testList, savedTasks);
    }

    [Fact]
    public void Test_Delete_DeletesCategoryAssociationsFromDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", DateTime.Today);
      testTask.Save();

      string testName = "Home stuff";
      Category testCategory = new Category(testName);
      testCategory.Save();

      //Act
      testCategory.AddTask(testTask);
      testCategory.Delete();

      List<Category> resultTaskCategories = testTask.GetCategories();
      List<Category> testTaskCategories = new List<Category> {};

      //Assert
      Assert.Equal(testTaskCategories, resultTaskCategories);
    }
    
      public void Dispose()
      {
        Category.DeleteAll();
        Task.DeleteAll();
      }
    }
  }
