using System;
using System.Collections.Generic;

namespace skbd.Models;

public partial class Row
{
    public int Id { get; set; }

    public int TableId { get; set; }

    public virtual Table Table { get; set; } = null!;

    public virtual ICollection<Value> Values { get; set; } = new List<Value>();
}
