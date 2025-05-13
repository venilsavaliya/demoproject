using System;
using System.Collections.Generic;

namespace DemoWeb.DAL.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Surname { get; set; }

    public bool Isdeleted { get; set; }
}
