using System.ComponentModel.DataAnnotations;

namespace SuitAlterations.API.SuitAlterations
{
	public class PlaceOrderToSuitAlterationRequest
	{
		[Range(-5, 5, ErrorMessage = "Can be up to plus or minus 5 cm"),
		 Required(ErrorMessage = "This field is required")]
		public int LeftSleeveLength { get; set; }

		[Range(-5, 5, ErrorMessage = "Can be up to plus or minus 5 cm"),
		 Required(ErrorMessage = "This field is required")]
		public int RightSleeveLength { get; set; }

		[Range(-5, 5, ErrorMessage = "Can be up to plus or minus 5 cm"),
		 Required(ErrorMessage = "This field is required")]
		public int LeftTrouserLength { get; set; }

		[Range(-5, 5, ErrorMessage = "Can be up to plus or minus 5 cm"),
		 Required(ErrorMessage = "This field is required")]
		public int RightTrouserLength { get; set; }
	}
}