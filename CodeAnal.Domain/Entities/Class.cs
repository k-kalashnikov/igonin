namespace CodeAnal.Domain.Entities
{
    public class Class
    {
        public string Name { get; set; }
        public Type SystemType { get; set; }

        public override string ToString()
        {
            return $"{SystemType.Name}\n" +
                $"Methods count = {SystemType.GetMethods().Count()}\n" +
                $"Variables count = {SystemType.GetFields().Count()}\n";
        }

    }
}
