using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.Documents;
using System.Collections.Generic;


namespace mydocumentdbdemo
{
    class Program
    {
        const string MyLinkCollection = "dbs/Mycosmosdb/colls/MyCollection/";
        static void Main(string[] args)
        {
            var EndPoint = "https://adcosmos.documents.azure.com:443/";
            var AuthKey = "zHiAlvuGjptfMk0ePImeK3duVdBNLq4MnkBFQOWnMNJE48YN3PgEHB4X8RhIjDtUx6W2zFmZP6M9Lyk1JU975g==";
           
            DocumentClient Client = new DocumentClient(new Uri(EndPoint), AuthKey);
           
            for(; ; )
            {
                Console.WriteLine("1: Create A Document \t 2: Read All Documents \t 3:Read Current Document  4: Update A Doument \t 5:Delete A Document \t 6:Exit \n\n Enter your choice");
                string Choice = (Console.ReadLine());
                switch (Choice)
                {
                    case "1":

                        string InsertId = Guid.NewGuid().ToString();
                        Console.WriteLine("Enter Name ");
                        String InsertName = Console.ReadLine();
                        Console.WriteLine("Enter age ");
                        int InsertAge = Convert.ToInt32(Console.ReadLine());
                        CreateDocument(Client, InsertName, InsertAge);
                        break;


                    case "2":
                        GetEmployees(Client);
                        break;


                    case "3":
                        Console.WriteLine("Enter Name of file you want to read ");
                        String ReadName = Console.ReadLine();
                        ReadCurrent(Client, ReadName);
                        break;


                    case "4":
                        Console.WriteLine("Enter ID of document which is to be updated ");
                        String UpdateId = Console.ReadLine();

                        Console.WriteLine("Enter the updated name");
                        String UpdateName = Console.ReadLine();

                        Console.WriteLine("Enter the updated age ");
                        int UpdateAge = Convert.ToInt32(Console.ReadLine());


                        Console.WriteLine("1: edit name \t 2: edit age \t 3:update both \n Enter your choice");
                        int Ch = Convert.ToInt32(Console.ReadLine());
                        switch (Ch)
                        {
                            case 1:

                              UpdateNameInDocument(Client, UpdateId, UpdateName);
                                break;

                            case 2:
                           
                               UpdateAgeInDocument(Client, UpdateId, UpdateAge);
                                break;
                            case 3:
                                Console.WriteLine("update name and age");
                               UpdateDocument(Client, UpdateId, UpdateName, UpdateAge);
                                break;
                        }
                        break;


                    case "5":
                        Console.WriteLine("Enter Id of file you want to delete ");
                        String IdToDelete = Console.ReadLine();
                       DeleteDocument(Client, IdToDelete);
                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("Invalid Choice");
                        break;

                }
                
            }
            
            

        }



  #region Employee Create
        //create a document
        private static void CreateDocument(DocumentClient client,string Name,int Age)
        {
            Employee _employee1 = new Employee();
           
            _employee1.Name = Name;
            _employee1.Age = Age;
     
            Task createEmployee = client.UpsertDocumentAsync
            (MyLinkCollection, _employee1);
            Task.WaitAll(createEmployee);
            Console.WriteLine("Document is inserted");
        }
        #endregion

        #region Employee Read

        //Read a document

        private static Task<List<Employee>> GetEmployees(DocumentClient Client)
        {
            var Manager = Client.CreateDocumentQuery<Employee>
            (MyLinkCollection).AsEnumerable();
            foreach (var item in Manager)
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Name);
                Console.WriteLine(item.Age);
                Console.WriteLine("************");
            }
           return Task.Run(() => Manager.ToList());

        }
       

        // Read A Document - Where Name == "John Doe"
        private static void ReadCurrent(DocumentClient Client,string Name)
        { 
        var EmployeeDetail =
                Client.CreateDocumentQuery<Employee>
                (MyLinkCollection)
                    .Where(e => e.Name == Name)
                    .AsEnumerable().OrderBy(Employee => Employee.Id);
            
            if (EmployeeDetail==null)
            {
                Console.WriteLine("no document Exists");
            }
            Console.WriteLine("-------- Read a document---------");
            foreach (var item in EmployeeDetail)
            {
                
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Name);
                Console.WriteLine(item.Age);
                Console.WriteLine("-------------------------------");
            }
        }
        #endregion

        #region Employee Update
        //Update a Document
        private static void UpdateDocument(DocumentClient Client, String Id, String Name, int Age)
        {
            // Update a Document

            var employeeToUpdate =
              Client.CreateDocumentQuery<Employee>(MyLinkCollection)
                    .Where(e => e.Id == Id)
                    .AsEnumerable()
                    .FirstOrDefault();

            Document doc = GetDocument(Client, employeeToUpdate.Id);
            Employee employeUpdated = employeeToUpdate;
            employeUpdated.Age = Age;
            employeUpdated.Name = Name;
            Task updateEmployee = Client.ReplaceDocumentAsync(doc.SelfLink,
                employeUpdated);
            Task.WaitAll(updateEmployee);


            var EmployeeDetail =
               Client.CreateDocumentQuery<Employee>
               (MyLinkCollection)
                   .Where(e => e.Id == employeeToUpdate.Id)
                   .AsEnumerable().First();


            Console.WriteLine("-------- Read a updated document---------");
            Console.WriteLine(EmployeeDetail.Id);
            Console.WriteLine(EmployeeDetail.Name);
            Console.WriteLine(EmployeeDetail.Age);
            Console.WriteLine("-------------------------------");
        }

        private static void UpdateNameInDocument(DocumentClient Client, String Id, String Name)
        {
            // Update a Document

            var employeeToUpdate =
              Client.CreateDocumentQuery<Employee>(MyLinkCollection)
                    .Where(e => e.Id == Id)
                    .AsEnumerable()
                    .FirstOrDefault();

            Document doc = GetDocument(Client, employeeToUpdate.Id);
            Employee employeUpdated = employeeToUpdate;

            employeUpdated.Name = Name;
            Task updateEmployee = Client.ReplaceDocumentAsync(doc.SelfLink,
                employeUpdated);
            Task.WaitAll(updateEmployee);


            var EmployeeDetail =
               Client.CreateDocumentQuery<Employee>
               (MyLinkCollection)
                   .Where(e => e.Id == employeeToUpdate.Id)
                   .AsEnumerable().First();


            Console.WriteLine("-------- Read a updated document---------");
            Console.WriteLine(EmployeeDetail.Id);
            Console.WriteLine(EmployeeDetail.Name);
            Console.WriteLine(EmployeeDetail.Age);
            Console.WriteLine("-------------------------------");
        }
        private static void UpdateAgeInDocument(DocumentClient Client, String Id, int Age)
        {
            // Update a Document

            var employeeToUpdate =
              Client.CreateDocumentQuery<Employee>(MyLinkCollection)
                    .Where(e => e.Id == Id)
                    .AsEnumerable()
                    .FirstOrDefault();

            Document doc = GetDocument(Client, employeeToUpdate.Id);
            Employee employeUpdated = employeeToUpdate;

            employeUpdated.Age = Age;
            Task updateEmployee = Client.ReplaceDocumentAsync(doc.SelfLink,
                employeUpdated);
            Task.WaitAll(updateEmployee);


            var EmployeeDetail =
               Client.CreateDocumentQuery<Employee>
               (MyLinkCollection)
                   .Where(e => e.Id == employeeToUpdate.Id)
                   .AsEnumerable().First();


            Console.WriteLine("-------- Read a updated document---------");
            Console.WriteLine(EmployeeDetail.Id);
            Console.WriteLine(EmployeeDetail.Name);
            Console.WriteLine(EmployeeDetail.Age);
            Console.WriteLine("-------------------------------");
        }

        private static Document GetDocument(DocumentClient Client, string Id)
        {
            return Client.CreateDocumentQuery(MyLinkCollection)
                   .Where(e => e.Id == Id)
                   .AsEnumerable()
                   .First();
        }
        #endregion

        #region Employee delete
        //Delete a document
        private static void DeleteDocument(DocumentClient Client, String Id)
        {
            var employeeToDelete =
              Client.CreateDocumentQuery<Employee>(MyLinkCollection)
                    .Where(e => e.Id == Id)
                    .AsEnumerable()
                    .First();

            Document DocumenttoDelete = GetDocument(Client, employeeToDelete.Id);


            Task DeleteEmployee = Client.DeleteDocumentAsync(DocumenttoDelete.SelfLink);

            Task.WaitAll(DeleteEmployee);
            Console.WriteLine("----List after deleting---");
            var Employees = Client.CreateDocumentQuery<Employee>
         (MyLinkCollection).AsEnumerable();
            foreach (var employee in Employees)
            {
                Console.WriteLine(employee.Id);
                Console.WriteLine(employee.Name);
                Console.WriteLine(employee.Age);
                Console.WriteLine("----------------------------------");
            }
        }
        #endregion
    }

}

