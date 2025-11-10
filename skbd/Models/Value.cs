using System;
using System.Collections.Generic;

namespace skbd.Models;

public partial class Value
{
    public int Id { get; set; }

    public int RowId { get; set; }

    public int FieldId { get; set; }

    public string? Val { get; set; }

    public virtual Field Field { get; set; } = null!;

    public virtual Row Row { get; set; } = null!;
}
