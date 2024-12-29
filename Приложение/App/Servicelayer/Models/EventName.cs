﻿using System;
using System.Collections.Generic;

namespace Servicelayer.Models;

public partial class EventName
{
    public int EventNameId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
