using System;

class Empleado
{
    public string Login { get; set; }
    public string Password { get; set; }

    public Empleado(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public override string ToString()
    {
        return Login + " - " + Password;
    }
}

