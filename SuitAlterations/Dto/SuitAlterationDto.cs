using System.ComponentModel.DataAnnotations;
using SuitAlterations.Core.Common;

namespace SuitAlterations.Dto {
	public class SuitAlterationDto {
		public int Id { get; set; }
		[Range(-5, 5), Required]
		public int LeftSleeveLength { get; set; }
		[Range(-5, 5), Required]
		public int RightSleeveLength { get; set; }
		[Range(-5, 5), Required]
		public int LeftTrouserLength { get; set; }
		[Range(-5, 5), Required]
		public int RightTrouserLength { get; set; }
		public SuitAlterationStatus Status { get; set; }
		public string AlterationTitle { get; set; }
	}
}