namespace MyApp;

using System.Text.RegularExpressions;

public class Person
{
    public int Index { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public Person(int index, string name, string email, string phone)
    {
        ValidateAndSet(index, name, email, phone);
    }

    public void Update(int index, string name, string email, string phone)
    {
        ValidateAndSet(index, name, email, phone);
    }

    private void ValidateAndSet(int index, string name, string email, string phone)
    {
        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        if (!regex.IsMatch(email))
            throw new ArgumentException("Email is not valid");

        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name can not be empty");

        if (phone.Length != 10 || !phone.All(char.IsDigit))
            throw new ArgumentException("Phone is not valid");

        Index = index;
        Name = name;
        Email = email;
        Phone = phone;
    }

    public override string ToString()
    {
        return $"{Index},{Name},{Email},{Phone}";
    }
}
