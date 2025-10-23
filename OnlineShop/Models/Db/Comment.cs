using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.Db;

public partial class Comment
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string CommentText { get; set; } = null!;

    public int ProductId { get; set; }

    public DateTime CreateDate { get; set; }
}