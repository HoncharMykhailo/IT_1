using System;
using System.Collections.Generic;

namespace skbd.Models;

public partial class Dbase
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}
