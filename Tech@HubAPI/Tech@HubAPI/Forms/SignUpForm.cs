using System;
using System.Text.RegularExpressions;

public class SignUpForm
{
    public SignUpForm(string firstName, string lastName, string username, string password, string email, string birthDate)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Username = username;
        this.Password = password;
        this.Email = email;
        this.BirthDate = birthDate;
    }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string BirthDate { get; set; }

    public void Validate()
    {
        string pattern = @"([A-Za-z0-9.]+)@([A-Za-z]+).(com|edu)";
        Match emailMatch = Regex.Match(this.Email, pattern);
        if (!emailMatch.Success)
        {
            throw new ArgumentException("Invalid Email.");
        }

        if (!DateTime.TryParse(this.BirthDate, out DateTime birthDate))
        {
            throw new ArgumentException("Invalid birth data.");
        }

        string pwd_capitals = @"[A-Z]+";
        string pwd_symbols = "[^A-Za-z0-9]+";

        if (Password.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters.");
        }
        else if (!Regex.IsMatch(Password, pwd_capitals))
        {
            throw new ArgumentException("Password must include a capital letter.");
        }
        else if (!Regex.IsMatch(Password, pwd_symbols))
        {
            throw new ArgumentException("Password must include a symbol.");
        }
    }
}
