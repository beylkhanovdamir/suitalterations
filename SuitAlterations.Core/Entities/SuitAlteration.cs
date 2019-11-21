using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SuitAlterations.Core.Common;
using SuitAlterations.Core.Data;

namespace SuitAlterations.Core.Entities {
	public class SuitAlteration : IEntity {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public int CustomerId { get; set; }
		public Customer Customer { get; set; }
		public int LeftSleeveLength { get; set; }
		public int RightSleeveLength { get; set; }
		public int LeftTrouserLength { get; set; }
		public int RightTrouserLength { get; set; }
		public SuitAlterationStatus Status { get; set; }
	}
}