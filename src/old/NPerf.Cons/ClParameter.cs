namespace NPerf.Cons
{
    using System;

    public class ClParameter
    {
        private bool unique;

        public ClParameter(string name, string shortName, string description, bool unique, bool alone)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (shortName == null)
            {
                throw new ArgumentNullException("shortName");
            }

            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            this.unique = unique;
            this.IsAlone = alone;
            this.Name = name;
            this.ShortName = shortName;
            this.Description = description;
        }

        public bool IsUnique
        {
            get
            {
                return this.unique || this.IsAlone;
            }
        }

        public bool IsAlone { get; private set; }

        public string Name { get; private set; }

        public string ShortName { get; private set; }

        public string Description { get; private set; }

        public string ToHelpString()
        {
            return String.Format(
                "--{0} -{1}, {2} {3} {4}",
                this.Name,
                this.ShortName,
                this.Description,
                this.IsUnique ? "unique" : "multiple",
                this.IsAlone ? "alone" : "with value");
        }
    }
}
