//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API_Service.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Carts
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Products Products { get; set; }
    }
}
