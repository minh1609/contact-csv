using System;
using System.Globalization;
using System.IO.Enumeration;
using CsvHelper;
using MyApp;

//NOTE: Press 5 to exit the program and save data in csv
class App
{
    static void Main(string[] args)
    {
        //var p = new Person("helo,s,1234567890");
        var run = true;
        List<Person> contacts = LoadContacts();

        while (run == true)
        {
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. Update Contact");
            Console.WriteLine("3. Delete Contact");
            Console.WriteLine("4. List Contacts");
            Console.WriteLine("5. Save and exit");
            Console.WriteLine("6. Sort by name");
            Console.WriteLine("7. Search by name");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddContact(contacts);
                    break;
                case "2":
                    UpdateContact(contacts);
                    break;
                case "3":
                    DeleteContact(contacts);
                    break;
                case "4":
                    ListAll(contacts);
                    break;
                case "5":
                    SaveContacts(contacts);
                    return;
                case "6":
                    SortContactsByName(contacts);
                    break;
                case "7":
                    SearchContactsByName(contacts);
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    //Print
    static void ListAll(List<Person> contacts)
    {
        foreach (Person p in contacts)
        {
            Console.WriteLine(p);
        }
    }

    static void AddContact(List<Person> contacts)
    {
        Console.Write("Enter name: ");
        string name = Console.ReadLine();
        Console.Write("Enter email: ");
        string email = Console.ReadLine();
        Console.Write("Enter phone number: ");
        string phoneNumber = Console.ReadLine();

        int id = contacts.Count > 0 ? contacts.Max(c => c.Index) + 1 : 1;

        try
        {
            Person newContact = new Person(id, name, email, phoneNumber);
            contacts.Add(newContact);
            Console.WriteLine(newContact + " added");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    static void DeleteContact(List<Person> contacts)
    {
        Person contactToDelete = FindAndValidateContact(contacts);
        if (contactToDelete == null)
        {
            Console.WriteLine("Contact not found.");
            return;
        }

        contacts.Remove(contactToDelete);
        Console.WriteLine("Contact deleted.");
    }

    //Prompt and find contact by ID
    //Return null if contact does not exist
    static Person? FindAndValidateContact(List<Person> contacts)
    {
        Console.Write("Enter the ID:");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid input. Please enter a valid contact ID.");
            return null;
        }

        Person contact = contacts.FirstOrDefault(c => c.Index == id);
        return contact;
    }

    static void UpdateContact(List<Person> contacts)
    {
        // Console.Write("Enter the ID:");
        // int idToUpdate;
        // if (!int.TryParse(Console.ReadLine(), out idToUpdate))
        // {
        //     Console.WriteLine("Invalid input. Please enter a valid contact ID.");
        //     return;
        // }

        Person contactToUpdate = FindAndValidateContact(contacts);
        if (contactToUpdate == null)
        {
            Console.WriteLine("Contact not found.");
            return;
        }

        Console.WriteLine($"Current details: {contactToUpdate}");
        Console.Write("Enter new name: ");
        var name = Console.ReadLine();
        Console.Write("Enter new email: ");
        var newEmail = Console.ReadLine();
        Console.Write("Enter new phone number: ");
        var newPhone = Console.ReadLine();

        try
        {
            contactToUpdate.Update(contactToUpdate.Index, name, newEmail, newPhone);
            Console.WriteLine(contactToUpdate + " updated");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    //load data from csv to List<Person> obj
    //Invalid data will be ignored and deleted
    static List<Person> LoadContacts()
    {
        List<Person> contacts = new List<Person>();
        if (File.Exists(Config.fileName))
        {
            using (StreamReader reader = new StreamReader(Config.fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(',');
                    if (parts.Length != 4)
                    {
                        Console.WriteLine("Data must contains 4 fields");
                        continue;
                    }
                    try
                    {
                        Person p = new Person(int.Parse(parts[0]), parts[1], parts[2], parts[3]);
                        contacts.Add(p);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("1 contact is not loaded because " + e.Message);
                    }
                }
            }
        }
        return contacts;
    }

    static void SaveContacts(List<Person> contacts)
    {
        using (StreamWriter writer = new StreamWriter(Config.fileName))
        {
            foreach (var contact in contacts)
            {
                writer.WriteLine(contact);
            }
        }
    }

    static void SortContactsByName(List<Person> contacts)
    {
        contacts.Sort((x, y) => string.Compare(x.Name, y.Name));
        Console.WriteLine("Contacts sorted by name:");
        ListAll(contacts);
    }

    static void SearchContactsByName(List<Person> contacts)
    {
        Console.Write("Enter the name to search for: ");
        string searchTerm = Console.ReadLine().Trim();

        List<Person> searchResults = contacts.FindAll(contact =>
            contact.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
        );

        if (searchResults.Count == 0)
        {
            Console.WriteLine("No contacts found with the given name.");
        }
        else
        {
            Console.WriteLine("Search results:");
            ListAll(searchResults);
        }
    }
}
