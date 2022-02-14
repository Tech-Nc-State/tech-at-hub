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

        if (Password.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters.");
        }
    }
}
