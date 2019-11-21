using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SuitAlterations.Core.Data;

namespace SuitAlterations.Core.Entities {
	public class Customer : IEntity {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[StringLength(50), Required]
		public string FirstName { get; set; }
		[StringLength(50), Required]
		public string LastName { get; set; }

		public List<SuitAlteration> SuitAlterations { get; set; }
	}
}