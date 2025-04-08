using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.RazorPages;
   using Microsoft.AspNetCore.Mvc.Rendering;
   using Microsoft.Data.Sqlite;
   using System.Collections.Generic;
// i canNOT figure out how to make a comment in a .cshtml file, but ill cover it here:
// in _Layout.cshtml, that code is used to set up the page - from padding and margins to classes (which can further be edited in _Layout.css - my favorite!) and overall structure of the page. 
   public class UgliestDogsModel : PageModel 
   {
       public List<SelectListItem> DogList { get; set; }
       public Dog SelectedDog { get; set; }

       public void OnGet() // tells the program to call the loadDogList class and access the list to begin the process of selecting an ugly dog winner
       {
           LoadDogList();
       }

       public void OnPost(string selectedDog) 
       {
           LoadDogList();
           if (!string.IsNullOrEmpty(selectedDog))
           {
               SelectedDog = GetDogById(int.Parse(selectedDog));
           }
       }

       private void LoadDogList() // this allows us to load the dog list from the db to access the data
       {
           DogList = new List<SelectListItem>();
           using (var connection = new SqliteConnection("Data Source=UgliestDogs.db"))
           {
               connection.Open();
               var command = connection.CreateCommand(); // sql connection
               command.CommandText = "SELECT Id, Name FROM Dogs"; // obtains id and name from dogs, run each time the selection is changed
               using (var reader = command.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       DogList.Add(new SelectListItem
                       {
                           Value = reader.GetInt32(0).ToString(),
                           Text = reader.GetString(1)
                       });
                   }
               }
           }
       }

       private Dog GetDogById(int id)
       {
           using (var connection = new SqliteConnection("Data Source=UgliestDogs.db")) // allows us to run and access daa from the DB using sql connection. below, sql commands are set and run each time the dog is changed to update picture and information
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT * FROM Dogs WHERE Id = @Id";
               command.Parameters.AddWithValue("@Id", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new Dog
                       {
                           Id = reader.GetInt32(0),
                           Name = reader.GetString(1),
                           Breed = reader.GetString(2),
                           Year = reader.GetInt32(3),
                           ImageFileName = reader.GetString(4)
                       };
                   }
               }
           }
           return null;
       }
   }

   public class Dog // getters and setters for Dog class (used to pass data back to code)
   {
       public int Id { get; set; }
       public string Name { get; set; }
       public string Breed { get; set; }
       public int Year { get; set; }
       public string ImageFileName { get; set; }
   }