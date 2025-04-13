using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class Photo
{
    public int Id { get; set; }

    public string? FileName { get; set; }

    public byte[]? FileData { get; set; }
}
