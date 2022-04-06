namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        public long ID { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        public double? Rating { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public long? UserID { get; set; }

        public long? ProductID { get; set; }

        public bool Status { get; set; }
    }
}
