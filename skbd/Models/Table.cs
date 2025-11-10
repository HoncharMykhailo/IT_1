using System;
using System.Collections.Generic;

namespace skbd.Models;

public partial class Table
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DbId { get; set; }

    public virtual Dbase Db { get; set; } = null!;

    public virtual ICollection<Field> Fields { get; set; } = new List<Field>();

    public virtual ICollection<Row> Rows { get; set; } = new List<Row>();
}
