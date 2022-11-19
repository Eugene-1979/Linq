using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq
{
    internal class People : IEquatable<People?>
    {
        public People(string name, Point location, string about)
        {
            Name = name;
            Location = location;
            About = about;
        }

        public HashSet<People> Freands { get; set; } = new HashSet<People>();
        public string Name { get; set; }
        public Point Location { get; set; }
        public string About { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as People);
        }

        public bool Equals(People? other)
        {
            return other is not null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public override string ToString()
        {
         string q= Freands.Select(q => q.Name).Aggregate((a, b) => a + " " + b);

            return $"" +
            $" {nameof(Name)}={Name}, " +
            $"{{{nameof(Freands)}={q}" +                   
            $"{nameof(Location)}={Location.ToString()},{Environment.NewLine} " +
          /*  $"{nameof(About)}={About}}}" +*/
            $"{Environment.NewLine}{Environment.NewLine}";
        }

        public static bool operator ==(People? left, People? right)
        {
            return EqualityComparer<People>.Default.Equals(left, right);
        }

        public static bool operator !=(People? left, People? right)
        {
            return !(left == right);
        }
    }
}
