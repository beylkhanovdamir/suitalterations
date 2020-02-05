using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.SuitAlterations
{
	public class SuitAlterationDto
	{
		public SuitAlterationId Id { get; set; }
		public int LeftSleeveLength { get; set; }
		public int RightSleeveLength { get; set; }
		public int LeftTrouserLength { get; set; }
		public int RightTrouserLength { get; set; }
		public SuitAlterationStatus Status { get; set; }
		public string AlterationTitle { get; set; }
	}
}