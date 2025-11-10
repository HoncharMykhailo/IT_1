using System;
using System.Collections.Generic;

namespace skbd.Models;

public partial class Field
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TableId { get; set; }

    public int TypeId { get; set; }

    public virtual Table Table { get; set; } = null!;

    public virtual Type Type { get; set; } = null!;

    public virtual ICollection<Value> Values { get; set; } = new List<Value>();
}
