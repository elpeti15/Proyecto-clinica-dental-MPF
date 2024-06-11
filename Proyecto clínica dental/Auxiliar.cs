using System;

class Auxiliar : Empleado
{
    public Auxiliar(string login, string password)
        : base(login, password)
    {
        Login = login;
        Password = password;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

