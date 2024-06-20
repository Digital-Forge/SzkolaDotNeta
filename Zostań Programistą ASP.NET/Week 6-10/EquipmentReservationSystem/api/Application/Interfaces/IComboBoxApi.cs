namespace Application.Interfaces
{
    public interface IComboBoxApi<T>
    {
        class SearchOption
        {
            public string Search { get; set; }
            public int? Take { get; set; }
            public List<T>? SerchingPosition { get; set; }
        }

        class ResponsData
        {
            public List<PositionData> Data { get; set; } = new();
            public List<PositionData> SerchingPosition { get; set; } = new();

            public class PositionData
            {
                public T Code { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
