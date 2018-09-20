namespace Game
{
    /// <summary>
    /// 移动步骤的详细信息
    /// </summary>
    public class Step
    {
        public Step(int id, MoveDir dir, int step = 1)
        {
            this.Id = id;
            this.Dir = dir;
            this.Stp = step;
        }

        /// <summary>
        /// 移动方向
        /// </summary>
        public MoveDir Dir { get; }

        /// <summary>
        /// 被移动的目标的ID
        /// </summary>
        public int Id { get; }

        public int Stp { get; }

        public override string ToString()
        {
            return Id + " " + Dir + " " + Stp;
        }
    }
}
